using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Valve.VR;

public class TextGenerationControl : MonoBehaviour
{
    public OpenAILLMs openai;
    public string output;
    public bool getResponse;
    public StreamAudioFromURL streamAudioFromURL;
    private GameObject _chatImg;
    private GameObject _chatImgClose;
    private TMP_Text _text;
    private GameObject _soulEverythingControl;
    private VoiceService _voiceService;
    //private string insertHistory = "";
    private AIHistory _history;
    private bool changeSaying;
    private SoulInformation _targetSoulInfo;
    private bool doAction;
    private bool detactAction;
    

    
    // Start is called before the first frame update
    void Start()
    {
        getResponse = false;
        changeSaying = false;
        doAction = false;
        detactAction = false;
        _soulEverythingControl = gameObject;
        _history = GameObject.Find("AIAgent").GetComponent<AIHistory>();
    }

    private void openChatImg() // 打开对话对象的文字提示框
    {
        SoulInformation information = _targetSoulInfo;

        if (!information.chatboxIsOpen)
        {
            information.OpenChatBox();
        }
        
    }

    private void closeChatImg() // 关闭对话对象文字提示框
    {
        SoulInformation information = _targetSoulInfo;

        if (information.chatboxIsOpen)
        {
            information.CloseChatBox();
        }
    }

    public void UpdateTargeSoul(SoulInformation soulInfo)
    {
        if (_targetSoulInfo == null || _targetSoulInfo.soulName != soulInfo.soulName)
        {
            Debug.Log("UpdateSoul " + soulInfo.name);
            if (_targetSoulInfo != null && _targetSoulInfo.chatboxIsOpen)
            {
                _targetSoulInfo.CloseChatBox();
            }
            _targetSoulInfo = soulInfo;
            _text = soulInfo.GetChatText();
            _voiceService = soulInfo.gameObject.GetComponent<VoiceService>();
        }
        

    }

    public void ChatToCharactor(string system, string prompt)
    {
        if (!getResponse)
        {
            output = "";
            //_text.text = "";
            getResponse = true;
            //insertHistory = "";
            _history.UpdateTalkingMemory("小朋友对当前角色进行了对话:" + prompt);
            Debug.Log("Talk Prompt " + prompt);
            openai.ChatToGPT(system, prompt);
            changeSaying = false;
            doAction = false;
        }
    }

    // Update is called once per frame
    
    void Update()
    {
        SoulInformation information = _soulEverythingControl.GetComponent<SoulEverythingControl>().GetTargetSoul().GetComponent<SoulInformation>();
        
        if (getResponse && openai.sayingName!="") 
        {
            if (!changeSaying && openai.sayingName != "")
            {
                for (int i = 0; i < _history.souls.Count; i++)
                {
                    if (openai.sayingName.Contains(_history.souls[i].soulName))
                    {
                        Debug.Log("还真找到了");
                        UpdateTargeSoul(_history.souls[i]);
                        changeSaying = true;
                        //openChatImg();
                        break;
                    }
                }
            }
            
            if (getResponse && openai.outputFinished) getResponse = false;
            output = openai.saying;
            
            _text.text = output;
            if (openai.outputFinished)
            {
                changeSaying = false;
                _history.UpdateTalkingMemory(information.soulName + "进行了回复:" + output);
                if (output != "(" + information.soulName + "正在思考中)")
                {
                    StartCoroutine(GenerateAndPlayVoice(output));
                }
                getResponse = false; // Reset the flag
                detactAction = true;
                Debug.Log("GPT output: " + openai.output);
                
                //closeChatImg();
            }

            

        }
        
        if (!doAction && detactAction && openai.output.Contains(">"))
        {
            string rawOutput = openai.output.Split(">")[1];
            for (int i = 0; i < _history.souls.Count; i++)
            {
                if (rawOutput.Contains(_history.souls[i].action))
                {
                    _history.UpdateActionMemory(_history.souls[i].soulName + "做了动作" + _history.souls[i].action);
                    _history.souls[i].SoulAction(); // 执行动作
                    doAction = true;
                    detactAction = false;
                    break;
                }
            }
        }
        
    }

    private IEnumerator GenerateAndPlayVoice(string textToSpeak) // 文字生成语音
    {
        _voiceService.voiceText = textToSpeak; // Set the text to be spoken
        Task<string> voiceGenerationTask = _voiceService.VoiceGeneration(); // Start the voice generation task

        // Wait until the voiceGenerationTask is completed
        while (!voiceGenerationTask.IsCompleted)
        {
            yield return null;
        }
        if (voiceGenerationTask.Exception != null)
        {
            Debug.LogError(voiceGenerationTask.Exception);
        }
        else
        {
            string voiceUrl = voiceGenerationTask.Result;
            //Debug.Log("Voice at voiceUrl " + voiceUrl);
            if (!string.IsNullOrEmpty(voiceUrl))
            {
                openChatImg();
                getResponse = false;
                StartCoroutine(streamAudioFromURL.GetAudioClip(voiceUrl));
            }
        }
    }
    
}
