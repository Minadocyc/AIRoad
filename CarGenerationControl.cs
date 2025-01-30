using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Valve.VR;
using System.IO;
using DG.Tweening;

public class CarGenerationControl : MonoBehaviour
{
    public string carName;

    private GameObject currentChatBox;
    public GameObject leftChatBox;
    public GameObject rightChatBox;
    public TMP_Text leftchatBoxText;
    public TMP_Text rightchatBoxText;
    private bool initialSpawn = true;
    private int spawnCounter = 0;
    public OpenAILLMsAtCarSpawn openai;
    public string output;
    //public bool getResponse;
    private TMP_Text _text;
    public StreamAudioFromURL streamAudioFromURL;
    private GameObject _soulEverythingControl;
    //private string insertHistory = "";
    private AIHistory _history;
    //private bool changeSaying;
    //private SoulInformation _targetSoulInfo;
    private bool doAction;
    private bool detactAction;
    public bool isRoadOne;
    private bool spawnCompleted = false;
    private GameObject lastHand;
    private bool lastHanded;
    private KeyboardManager keyboardManager;
    public Queue<string> nameQueue = new Queue<string>();
    public Queue<string> dialogQueue = new Queue<string>();
    public Queue<bool> actionQueue = new Queue<bool>();
    public Queue<bool> speakQueue = new Queue<bool>();
    public Queue<bool> humanQueue = new Queue<bool>();

    private GameObject lastCar;

    public GameObject pink1One;
    public GameObject pink2One;
    public GameObject pink3One;
    public GameObject pink4One;
    public GameObject blue1One;
    public GameObject blue2One;
    public GameObject blue3One;
    public GameObject blue4One;
    public GameObject purple1One;
    public GameObject purple2One;
    public GameObject purple3One;
    public GameObject purple4One;
    public GameObject orange1One;
    public GameObject orange2One;
    public GameObject orange3One;
    public GameObject orange4One;
    public GameObject pink1HandOne;
    public GameObject pink2HandOne;
    public GameObject pink3HandOne;
    public GameObject pink4HandOne;
    public GameObject blue1HandOne;
    public GameObject blue2HandOne;
    public GameObject blue3HandOne;
    public GameObject blue4HandOne;
    public GameObject purple1HandOne;
    public GameObject purple2HandOne;
    public GameObject purple3HandOne;
    public GameObject purple4HandOne;
    public GameObject orange1HandOne;
    public GameObject orange2HandOne;
    public GameObject orange3HandOne;
    public GameObject orange4HandOne;
    public GameObject pink1Two;
    public GameObject pink2Two;
    public GameObject pink3Two;
    public GameObject pink4Two;
    public GameObject blue1Two;
    public GameObject blue2Two;
    public GameObject blue3Two;
    public GameObject blue4Two;
    public GameObject purple1Two;
    public GameObject purple2Two;
    public GameObject purple3Two;
    public GameObject purple4Two;
    public GameObject orange1Two;
    public GameObject orange2Two;
    public GameObject orange3Two;
    public GameObject orange4Two;
    public GameObject pink1HandTwo;
    public GameObject pink2HandTwo;
    public GameObject pink3HandTwo;
    public GameObject pink4HandTwo;
    public GameObject blue1HandTwo;
    public GameObject blue2HandTwo;
    public GameObject blue3HandTwo;
    public GameObject blue4HandTwo;
    public GameObject purple1HandTwo;
    public GameObject purple2HandTwo;
    public GameObject purple3HandTwo;
    public GameObject purple4HandTwo;
    public GameObject orange1HandTwo;
    public GameObject orange2HandTwo;
    public GameObject orange3HandTwo;
    public GameObject orange4HandTwo;
    public GameObject pink1OnePass;
    public GameObject pink2OnePass;
    public GameObject pink3OnePass;
    public GameObject blue1OnePass;
    public GameObject blue2OnePass;
    public GameObject blue3OnePass;
    public GameObject purple1OnePass;
    public GameObject purple2OnePass;
    public GameObject purple3OnePass;
    public GameObject orange1OnePass;
    public GameObject orange2OnePass;
    public GameObject orange3OnePass;
    public GameObject pink1TwoPass;
    public GameObject pink2TwoPass;
    public GameObject pink3TwoPass;
    public GameObject blue1TwoPass;
    public GameObject blue2TwoPass;
    public GameObject blue3TwoPass;
    public GameObject purple1TwoPass;
    public GameObject purple2TwoPass;
    public GameObject purple3TwoPass;
    public GameObject orange1TwoPass;
    public GameObject orange2TwoPass;
    public GameObject orange3TwoPass;
    public bool isSecOne;
    private GameObject passObject;
    public VoiceService _voiceService;
    private int randomInt;
    private bool randomVoice;
    private bool isHuman;
    private string path = "D:/Aiagent for autism/GPTLog/GptLog.txt";
    public GameObject leftToRightHuman;
    public Transform leftToRightHumanTransform;
    public GameObject rightToLeftHuman;
    public Transform rightToLeftHumanTransform;
    public GameObject leftBoom;
    public GameObject rightBoom;
    private bool isLTR;
    private bool isRTL;
    private string carDialog;
    
    private Vector3 leftTrans;
    private Vector3 rightTrans;
    private bool boomLR;

    // Start is called before the first frame update
    void Awake()
    {
        boomLR = false;
        isLTR =false;
        isRTL = false;
        keyboardManager = GameObject.Find("KeyBoardManager").GetComponent<KeyboardManager>();
        _voiceService = GameObject.Find("VoiceServer").GetComponent<VoiceService>();
        lastHanded = false;
        spawnCompleted = false;
        initialSpawn = true;
        spawnCounter = 0;
        Debug.Log("Deavtive everything...");
        pink1One.SetActive(false);
        pink2One.SetActive(false);
        pink3One.SetActive(false);
        pink4One.SetActive(false);
        blue1One.SetActive(false);
        blue2One.SetActive(false);
        blue3One.SetActive(false);
        blue4One.SetActive(false);
        purple1One.SetActive(false);
        purple2One.SetActive(false);
        purple3One.SetActive(false);
        purple4One.SetActive(false);
        orange1One.SetActive(false);
        orange2One.SetActive(false);
        orange3One.SetActive(false);
        orange4One.SetActive(false);
        pink1HandOne.SetActive(false);
        pink2HandOne.SetActive(false);
        pink3HandOne.SetActive(false);
        pink4HandOne.SetActive(false);
        blue1HandOne.SetActive(false);
        blue2HandOne.SetActive(false);
        blue3HandOne.SetActive(false);
        blue4HandOne.SetActive(false);
        purple1HandOne.SetActive(false);
        purple2HandOne.SetActive(false);
        purple3HandOne.SetActive(false);
        purple4HandOne.SetActive(false);
        orange1HandOne.SetActive(false);
        orange2HandOne.SetActive(false);
        orange3HandOne.SetActive(false);
        orange4HandOne.SetActive(false);
        pink1Two.SetActive(false);
        pink2Two.SetActive(false);
        pink3Two.SetActive(false);
        pink4Two.SetActive(false);
        blue1Two.SetActive(false);
        blue2Two.SetActive(false);
        blue3Two.SetActive(false);
        blue4Two.SetActive(false);
        purple1Two.SetActive(false);
        purple2Two.SetActive(false);
        purple3Two.SetActive(false);
        purple4Two.SetActive(false);
        orange1Two.SetActive(false);
        orange2Two.SetActive(false);
        orange3Two.SetActive(false);
        orange4Two.SetActive(false);
        pink1HandTwo.SetActive(false);
        pink2HandTwo.SetActive(false);
        pink3HandTwo.SetActive(false);
        pink4HandTwo.SetActive(false);
        blue1HandTwo.SetActive(false);
        blue2HandTwo.SetActive(false);
        blue3HandTwo.SetActive(false);
        blue4HandTwo.SetActive(false);
        purple1HandTwo.SetActive(false);
        purple2HandTwo.SetActive(false);
        purple3HandTwo.SetActive(false);
        purple4HandTwo.SetActive(false);
        orange1HandTwo.SetActive(false);
        orange2HandTwo.SetActive(false);
        orange3HandTwo.SetActive(false);
        orange4HandTwo.SetActive(false);
        pink1OnePass.SetActive(false);
        pink2OnePass.SetActive(false);
        pink3OnePass.SetActive(false);
        blue1OnePass.SetActive(false);
        blue2OnePass.SetActive(false);
        blue3OnePass.SetActive(false);
        purple1OnePass.SetActive(false);
        purple2OnePass.SetActive(false);
        purple3OnePass.SetActive(false);
        orange1OnePass.SetActive(false);
        orange2OnePass.SetActive(false);
        orange3OnePass.SetActive(false);
        pink1TwoPass.SetActive(false);
        pink2TwoPass.SetActive(false);
        pink3TwoPass.SetActive(false);
        blue1TwoPass.SetActive(false);
        blue2TwoPass.SetActive(false);
        blue3TwoPass.SetActive(false);
        purple1TwoPass.SetActive(false);
        purple2TwoPass.SetActive(false);
        purple3TwoPass.SetActive(false);
        orange1TwoPass.SetActive(false);
        orange2TwoPass.SetActive(false);
        orange3TwoPass.SetActive(false);

        //
        //blue1One.SetActive(true);
    }
    void Start()
    {
        isRoadOne = true;
        isSecOne = false;
        //getResponse = false;
        //changeSaying = false;
        doAction = false;
        detactAction = false;
        _soulEverythingControl = gameObject;
        _history = GameObject.Find("AIAgent").GetComponent<AIHistory>();
        SpawnCar();
        Debug.Log("Start initial call");
        // orange3Two.SetActive(true);
        // pink4One.SetActive(true);
        leftTrans = leftToRightHuman.transform.position;
        //Debug.Log("ZZZ" + leftTrans.z);
        rightTrans = rightToLeftHuman.transform.position;
        
    }

    public void SpawnCar()
    {
        output = "";
        
        string background = _history.GetBackgroundDescribeForCar();
        string soulsPrefix = _history.GetSoulPrefixDescribeForCar();
        string soulsInfo = _history.GetSoulsInfoForCar();
        string soulsSuffix = _history.GetSoulSuffixDescribeForCar();
        string playerHistory = _history.GetPlayerHistoryForCar();
        Debug.Log("当前AI History "+playerHistory);
            
        //getResponse = true;
        
        string fullPrompt = background + soulsPrefix + soulsInfo + soulsSuffix;
        Debug.Log("Sending Prompt " + fullPrompt);
        openai.ChatToGPT(fullPrompt, playerHistory);
            
        //changeSaying = false;
        doAction = false;

        
    }

    // Update is called once per frame
    
    void Update()
    {
        //SoulInformation information = _soulEverythingControl.GetComponent<SoulEverythingControl>().GetTargetSoul().GetComponent<SoulInformation>();
        
        if (openai.outputFinished)
        {
            Debug.Log("Output completed " + output);
            File.AppendAllText(path, "["+Time.time+"]"+output+"\n");
            openai.outputFinished = false;
            //getResponse = false;
            //changeSaying = true;
        }
        if (initialSpawn & spawnCounter == 1){
            spawnCounter = 0;
            Debug.Log("Send first new request...");
            SpawnCar();
        } else if (spawnCounter == 2){
            spawnCounter = 0;
            Debug.Log("Send new request...");
            SpawnCar();
        }
        if (isLTR){
            leftToRightHumanTransform.Translate(Vector3.right * Time.deltaTime * 1.2f);
        }
        if (isRTL){
            rightToLeftHumanTransform.Translate(Vector3.left * Time.deltaTime * 1.5f);
        }
    }
    public void GenerateLeftDelayed(){
        carDialog = dialogQueue.Dequeue();
        
        currentChatBox = leftChatBox;
        currentChatBox.SetActive(true);
        leftchatBoxText.text = carDialog;
        // Left road
        //Debug.Log("Send Voice data");
        randomInt = Random.Range(1, 5);
        bool isVoice = speakQueue.Dequeue();
        randomVoice = isVoice;
        if (randomInt == 1){
            _voiceService.voiceCharacterName = "派蒙";
        } else if (randomInt == 2){
            _voiceService.voiceCharacterName = "班尼特";
        } else if (randomInt == 3){
            _voiceService.voiceCharacterName = "流萤";
        } else if (randomInt == 4){
            _voiceService.voiceCharacterName = "罗刹";
        }
        if (randomVoice){
            StartCoroutine(GenerateAndPlayVoice(carDialog));
        }
        
        StartCoroutine(GenerateAndPlayVoice(carDialog));
        Invoke("GenerateLeft", 2f);
    }
    public void GenerateRightDelayed(){
        carDialog = dialogQueue.Dequeue();
        bool isVoice = speakQueue.Dequeue();
        //Debug.Log("Send Voice data");
        currentChatBox = rightChatBox;
        currentChatBox.SetActive(true);
        rightchatBoxText.text = carDialog;
        randomInt = Random.Range(1, 5);
        randomVoice = isVoice;

        if (randomInt == 1){
            _voiceService.voiceCharacterName = "派蒙";
        } else if (randomInt == 2){
            _voiceService.voiceCharacterName = "班尼特";
        } else if (randomInt == 3){
            _voiceService.voiceCharacterName = "流萤";
        } else if (randomInt == 4){
            _voiceService.voiceCharacterName = "罗刹";
        }
        if (randomVoice){
            StartCoroutine(GenerateAndPlayVoice(carDialog));
        }
        
        Invoke("GenerateRight", 2f);
    }
    public void GenerateLeft(){
        spawnCounter += 1;
        carName = nameQueue.Dequeue();
        
        bool carAction = actionQueue.Dequeue();
        isHuman = humanQueue.Dequeue();
        //isHuman = true;
        File.AppendAllText(path, "["+Time.time+"]"+"Left Car:"+carName+"|action:" + carAction +"|with speak:" +randomVoice+ "|with human:" + isHuman +"|dialog:"+carDialog+"\n");
        // if (isHuman){
        //     LeftToRightWalker();    
        // }
        if (carName == "One"){
            if (randomInt == 1){
                pink1One.SetActive(true);
                lastCar = pink1One;
                passObject = pink1OnePass;
                if (carAction){
                    lastHanded = true;
                    pink1HandOne.SetActive(true);
                    lastHand = pink1HandOne;
                }
            } else if (randomInt == 2){
                blue1One.SetActive(true);
                lastCar = blue1One;
                passObject = blue1OnePass;
                if (carAction){
                    lastHanded = true;
                    blue1HandOne.SetActive(true);
                    lastHand = blue1HandOne;
                }
            } else if (randomInt == 3){
                purple1One.SetActive(true);
                lastCar = purple1One;
                passObject = purple1OnePass;
                if (carAction){
                    lastHanded = true;
                    purple1HandOne.SetActive(true);
                    lastHand = purple1HandOne;
                }
            } else {
                orange1One.SetActive(true);
                lastCar = orange1One;
                passObject = orange1OnePass;
                if (carAction){
                    lastHanded = true;
                    orange1HandOne.SetActive(true);
                    lastHand = orange1HandOne;
                }
            }
        } else if (carName == "Two"){
            
            if (randomInt == 1){
                pink2One.SetActive(true);
                lastCar = pink2One;
                passObject = pink2OnePass;
                if (carAction){
                    lastHanded = true;
                    pink2HandOne.SetActive(true);
                    lastHand = pink2HandOne;
                }
            } else if (randomInt == 2){
                blue2One.SetActive(true);
                lastCar = blue2One;
                passObject = blue2OnePass;
                if (carAction){
                    lastHanded = true;
                    blue2HandOne.SetActive(true);
                    lastHand = blue2HandOne;
                }
            } else if (randomInt == 3){
                purple2One.SetActive(true);
                lastCar = purple2One;
                passObject = purple2OnePass;
                if (carAction){
                    lastHanded = true;
                    purple2HandOne.SetActive(true);
                    lastHand = purple2HandOne;
                }
            } else {
                orange2One.SetActive(true);
                lastCar = orange2One;
                passObject = orange2OnePass;
                if (carAction){
                    lastHanded = true;
                    orange2HandOne.SetActive(true);
                    lastHand = orange2HandOne;
                }
            }
        } else if (carName == "Three"){
            boomLR = true;
            if (randomInt == 1){
                
                pink3One.SetActive(true);
                lastCar = pink3One;
                passObject = pink3OnePass;
                Invoke("PassCar", 5f);
                if (carAction){
                    lastHanded = true;
                    pink3HandOne.SetActive(true);
                    lastHand = pink3HandOne;
                }
                Debug.Log("1s collosion check");
                Invoke("CollisionCheck", 3f);
            } else if (randomInt == 2){
                blue3One.SetActive(true);
                lastCar = blue3One;
                passObject = blue3OnePass;
                if (carAction){
                    lastHanded = true;
                    blue3HandOne.SetActive(true);
                    lastHand = blue3HandOne;
                }
                Debug.Log("1s collosion check");
                Invoke("CollisionCheck", 3f);
                Invoke("PassCar", 5f);
            } else if (randomInt == 3){
                purple3One.SetActive(true);
                lastCar = purple3One;
                passObject = purple3OnePass;
                if (carAction){
                    lastHanded = true;
                    purple3HandOne.SetActive(true);
                    lastHand = purple3HandOne;
                }
                Debug.Log("1s collosion check");
                Invoke("CollisionCheck", 3f);
                Invoke("PassCar", 5f);
            } else {
                orange3One.SetActive(true);
                lastCar = orange3One;
                passObject = orange3OnePass;
                if (carAction){
                    lastHanded = true;
                    orange3HandOne.SetActive(true);
                    lastHand = orange3HandOne;
                }
                Debug.Log("5s collosion check");
                Invoke("CollisionCheck", 5f);
                Invoke("PassCar", 5f);
            }
        } else {
            boomLR = true;
            if (randomInt == 1){
                pink4One.SetActive(true);
                lastCar = pink4One;
                if (carAction){
                    lastHanded = true;
                    pink4HandOne.SetActive(true);
                    lastHand = pink4HandOne;
                }
                Debug.Log("1s collosion check");
                Invoke("CollisionCheck", 3f);
                Invoke("fourCarPass", 5f);
                
            } else if (randomInt == 2){
                blue4One.SetActive(true);
                lastCar = blue4One;
                if (carAction){
                    lastHanded = true;
                    blue4HandOne.SetActive(true);
                    lastHand = blue4HandOne;
                }
                Debug.Log("1s collosion check");
                Invoke("CollisionCheck", 3f);
                Invoke("fourCarPass", 5f);
            } else if (randomInt == 3){
                purple4One.SetActive(true);
                lastCar = purple4One;
                if (carAction){
                    lastHanded = true;
                    purple4HandOne.SetActive(true);
                    lastHand = purple4HandOne;
                }
                Debug.Log("1s collosion check");
                Invoke("CollisionCheck", 3f);
                Invoke("fourCarPass", 5f);
            } else {
                orange4One.SetActive(true);
                lastCar = orange4One;
                if (carAction){
                    lastHanded = true;
                    orange4HandOne.SetActive(true);
                    lastHand = orange4HandOne;
                }
                Debug.Log("1s collosion check");
                Invoke("CollisionCheck", 3f);
                Invoke("fourCarPass", 5f);
            }
            
        }
        if (isHuman){
            LeftToRightWalker();
        }
        // if (carName == "Three"){
        //     Debug.Log("5s collosion check");
        //     Invoke("CollisionCheck", 5f);

        // } else if (carName == "Four"){
        //     Debug.Log("1s collision check");
        //     Invoke("CollisionCheck", 1f);
        // }
    }


    public void GenerateRight(){
        spawnCounter += 1;
        string carName = nameQueue.Dequeue();
        bool carAction = actionQueue.Dequeue();
        isHuman = humanQueue.Dequeue();
        //isHuman = true;
        
        File.AppendAllText(path, "["+Time.time+"]"+"Right Car:"+carName+"|action:" + carAction +"|with speak:" +randomVoice+ "|with human:" + isHuman +"|dialog:"+carDialog+"\n");
        
        if (carName == "One"){
            
            if (randomInt == 1){
                pink1Two.SetActive(true);
                lastCar = pink1Two;
                passObject = pink1TwoPass;
                if (carAction){
                    lastHanded = true;
                    pink1HandTwo.SetActive(true);
                    lastHand = pink1HandTwo;
                }
            } else if (randomInt == 2){
                blue1Two.SetActive(true);
                lastCar = blue1Two;
                passObject = blue1TwoPass;
                if (carAction){
                    lastHanded = true;
                    blue1HandTwo.SetActive(true);
                    lastHand = blue1HandTwo;
                }
            } else if (randomInt == 3){
                purple1Two.SetActive(true);
                lastCar = purple1Two;
                passObject = purple1TwoPass;
                if (carAction){
                    lastHanded = true;
                    purple1HandTwo.SetActive(true);
                    lastHand = purple1HandTwo;
                }
            } else {
                orange1Two.SetActive(true);
                lastCar = orange1Two;
                passObject = orange1TwoPass;
                if (carAction){
                    lastHanded = true;
                    orange1HandTwo.SetActive(true);
                    lastHand = orange1HandTwo;
                }
            }
        } else if (carName == "Two"){
            if (randomInt == 1){
                pink2Two.SetActive(true);
                lastCar = pink2Two;
                passObject = pink2TwoPass;
                if (carAction){
                    lastHanded = true;
                    pink2HandTwo.SetActive(true);
                    lastHand = pink2HandTwo;
                }
            } else if (randomInt == 2){
                
                blue2Two.SetActive(true);
                lastCar = blue2Two;
                passObject = blue2TwoPass;
                if (carAction){
                    lastHanded = true;
                    blue2HandTwo.SetActive(true);
                    lastHand = blue2HandTwo;
                }
            } else if (randomInt == 3){
                purple2Two.SetActive(true);
                lastCar = purple2Two;
                passObject = purple2TwoPass;
                if (carAction){
                    lastHanded = true;
                    purple2HandTwo.SetActive(true);
                    lastHand = purple2HandTwo;
                }
            } else {
                orange2Two.SetActive(true);
                lastCar = orange2Two;
                passObject = orange2TwoPass;
                if (carAction){
                    lastHanded = true;
                    orange2HandTwo.SetActive(true);
                    lastHand = orange2HandTwo;
                }
            }
        } else if (carName == "Three"){
            boomLR = true;
            if (randomInt == 1){
                pink3Two.SetActive(true);
                lastCar = pink3Two;
                passObject = pink3TwoPass;
                Invoke("PassCar", 5f);
                Debug.Log("5s collosion check");
                Invoke("CollisionCheck", 5f);
                if (carAction){
                    lastHanded = true;
                    pink3HandTwo.SetActive(true);
                    lastHand = pink3HandTwo;
                }
            } else if (randomInt == 2){
                blue3Two.SetActive(true);
                lastCar = blue3Two;
                passObject = blue3TwoPass;
                Debug.Log("5s collosion check");
                Invoke("CollisionCheck", 5f);
                if (carAction){
                    lastHanded = true;
                    blue3HandTwo.SetActive(true);
                    lastHand = blue3HandTwo;
                }
                Invoke("PassCar", 5f);
            } else if (randomInt == 3){
                purple3Two.SetActive(true);
                lastCar = purple3Two;
                passObject = purple3TwoPass;
                Debug.Log("5s collosion check");
                Invoke("CollisionCheck", 5f);
                if (carAction){
                    lastHanded = true;
                    purple3HandTwo.SetActive(true);
                    lastHand = purple3HandTwo;
                }
                Invoke("PassCar", 5f);
            } else {
                orange3Two.SetActive(true);
                lastCar = orange3Two;
                passObject = orange3TwoPass;
                Debug.Log("5s collosion check");
                Invoke("CollisionCheck", 5f);
                if (carAction){
                    lastHanded = true;
                    orange3HandTwo.SetActive(true);
                    lastHand = orange3HandTwo;
                }
                Invoke("PassCar", 5f);
            }
        } else {
            boomLR = true;
            if (randomInt == 1){
                pink4Two.SetActive(true);
                lastCar = pink4Two;
                Debug.Log("1s collosion check");
                Invoke("CollisionCheck", 3f);
                if (carAction){
                    lastHanded = true;
                    pink4HandTwo.SetActive(true);
                    lastHand = pink4HandTwo;
                }
                Invoke("fourCarPass", 5f);
                
            } else if (randomInt == 2){
                blue4Two.SetActive(true);
                lastCar = blue4Two;
                if (carAction){
                    lastHanded = true;
                    blue4HandTwo.SetActive(true);
                    lastHand = blue4HandTwo;
                }
                Debug.Log("1s collosion check");
                Invoke("CollisionCheck", 3f);
                Invoke("fourCarPass", 5f);
            } else if (randomInt == 3){
                purple4Two.SetActive(true);
                lastCar = purple4Two;
                if (carAction){
                    lastHanded = true;
                    purple4HandTwo.SetActive(true);
                    lastHand = purple4HandTwo;
                }
                Debug.Log("1s collosion check");
                Invoke("CollisionCheck", 3f);
                Invoke("fourCarPass", 5f);
            } else {
                orange4Two.SetActive(true);
                lastCar = orange4Two;
                if (carAction){
                    lastHanded = true;
                    orange4HandTwo.SetActive(true);
                    lastHand = orange4HandTwo;
                }
                Debug.Log("1s collosion check");
                Invoke("CollisionCheck", 3f);
                Invoke("fourCarPass", 5f);
            }
        }
        if (isHuman){
            RightToLeftWalker();   
        }
        // if (isHuman){
        //     RightToLeftWalker();
        // }
        // if (carName == "Three"){
        //     Debug.Log("5s collosion check");
        //     Invoke("CollisionCheck", 5f);

        // } else if (carName == "Four"){
        //     Debug.Log("1s collision check");
        //     Invoke("CollisionCheck", 1f);
        // }
    }

    public void CollisionCheck(){
        if (keyboardManager.playerPosisiton == "左侧斑马线" | keyboardManager.playerPosisiton == "右侧斑马线"){
            keyboardManager.isCollision = true;  
        }
        keyboardManager.SaveElapsedTime();
    }

    public void PassCar(){
        lastCar.SetActive(false);
        passObject.SetActive(true);
        if (lastHanded){
            lastHanded = false;
            lastHand.SetActive(false);
        }
    }

    public void fourCarPass(){
        lastCar.SetActive(false);
        if (lastHanded){
            lastHanded = false;
            lastHand.SetActive(false);
        }
    }

    private IEnumerator GenerateAndPlayVoice(string textToSpeak) // 文字生成语音
    {
        _voiceService.voiceText = textToSpeak; // Set the text to be spoken
        // Open ChatBox
        

        Task<string> voiceGenerationTask = _voiceService.VoiceGeneration(); // Start the voice generation task

        // Wait until the voiceGenerationTask is completed
        Debug.Log("Generating Voice");
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
                Debug.Log("Open Chat box");
                //getResponse = false;
                currentChatBox.SetActive(true);
                StartCoroutine(streamAudioFromURL.GetAudioClip(voiceUrl));
                //Debug.Log("Voice completed");
                //currentChatBox.SetActive(false);
            }
        }
    }


    void LeftToRightWalker(){
        leftToRightHuman.SetActive(true);
        isLTR = true;
        if (boomLR){
            Debug.Log("BOOM!!!");
            Invoke("LeftHumanDisappear", 3f);
        }else{
            Debug.Log("No BOOM!!!");
            Invoke("LeftHumanDisappear", 5f);
        }
        
    }
    void RightToLeftWalker(){
        isRTL = true;
        rightToLeftHuman.SetActive(true);
        if (boomLR){
            Debug.Log("BOOM!!!");
            Invoke("RightHumanDisappear", 3f);
        }else{
            Debug.Log("No BOOM!!!");
            Invoke("RightHumanDisappear", 5f);
        }
        
    }
    void LeftHumanDisappear(){
        isLTR = false;
        if (boomLR){
            leftBoom.SetActive(true);
            boomLR = false;
            leftToRightHuman.SetActive(false);
            Invoke("RemoveBoom", 2f);
        }
        else{
            leftToRightHuman.SetActive(false);
        }
        boomLR = false;
        Vector3 temp = new Vector3(leftTrans.x, leftToRightHuman.transform.position.y,1f);
        leftToRightHuman.transform.position= temp;
        Debug.Log("AFTER zzz" + temp);
        Debug.Log("Left Trans result zzz " + leftToRightHuman.transform.position.z);
        
    }
    void RightHumanDisappear(){
        isRTL = false;
        if (boomLR){
            Debug.Log("Boom");
            boomLR = false;
            rightBoom.SetActive(true);
            rightToLeftHuman.SetActive(false);
            Invoke("RemoveBoom", 2f);
        }else{
            Debug.Log("NoBoom");
            rightToLeftHuman.SetActive(false);
        }
        boomLR = false;
        Debug.Log("New Position " + rightTrans);    
        Vector3 temp = new Vector3(rightTrans.x, rightToLeftHuman.transform.position.y,1f); 
        rightToLeftHuman.transform.position= temp;
        Debug.Log("New Position " + rightToLeftHuman.transform.position.z); 
    }
    private void RemoveBoom(){
        boomLR = false;
        leftBoom.SetActive(false);
        rightBoom.SetActive(false);
    }
    
}