using UnityEngine;
using GME;
using System.IO;


//腾讯GME语音识别
public class VoiceToTextService : MonoBehaviour
{
    [Header("APISetting")]
    public string sdkAppID = "1400800896";
    public string openID = "1";
    public string key = "ca1a3ab0f6784feb";

    [Header("VoiceSetting")]
    public string textResult;
    
    private string recordPath;
    void Start()
    {
        //注册腾讯API信息
        int ret = ITMGContext.GetInstance().Init(sdkAppID, openID);
        var authBuffer = QAVAuthBuffer.GenAuthBuffer(1, "", sdkAppID, key);
        ITMGPTT.GetInstance().ApplyPTTAuthbuffer(authBuffer);


        if (ret != QAVError.OK)
        {
            Debug.Log("GME SDK初始化失败:" + ret);
            return;
        }
        else
        {
            Debug.Log("GME SDK初始化成功:" + ret);

            ITMGContext.GetInstance().GetPttCtrl().OnStreamingSpeechComplete += OnStreamingSpeechComplete;
            ITMGContext.GetInstance().GetPttCtrl().OnStreamingSpeechisRunning += OnStreamingRecisRunning;
        }
    }

    // Update is called once per frame
    void Update()
    {
        ITMGContext.GetInstance().Poll();
    }

    public void StartToRecord()
    {
        textResult = "";
        // 指定录音文件的保存路径
        string directoryPath = Path.Combine(Application.persistentDataPath, "voiceFiles");

        // 检查路径是否存在
        if (!Directory.Exists(directoryPath))
        {
            // 创建路径
            Directory.CreateDirectory(directoryPath);
        }

        // 生成文件名并添加到路径
        string recordPath = Path.Combine(directoryPath, string.Format("{0}.silk", System.Guid.NewGuid().ToString()));
        // 开始录音并启用流式语音识别
        int ret = ITMGContext.GetInstance().GetPttCtrl().StartRecordingWithStreamingRecognition(recordPath, "cmn-Hans-CN", "cmn-Hans-CN");
        if (ret != QAVError.OK)
        {
            Debug.Log("GME 启动流式语音识别失败:" + ret);
        }
        else
        {
            Debug.Log("GME 启动流式语音识别成功:" + ret);

        }
    }

    public void StopRecordAndPrint()
    {
        // 停止录制
        ITMGContext.GetInstance().GetPttCtrl().StopRecording();
        // 打印录音文件路径
        Debug.Log("录音已停止。文件保存在：" + recordPath);
    }

    // 流式语音识别完成的回调
    void OnStreamingSpeechComplete(int code, string fileid, string filepath, string result)
    {
        Debug.Log(code + fileid + filepath + result);
        if (code == QAVError.OK)
        {
            Debug.Log("流式语音识别完成，文本：" + result);
        }
        else
        {
            Debug.Log("流式语音识别失败，错误码：" + code);
        }
    }

    // 流式语音识别运行时的回调
    void OnStreamingRecisRunning(int code, string fileid, string filePath, string result)
    {
        Debug.Log(code + fileid + filePath + result);

        if (code == QAVError.OK)
        {
            Debug.Log("流式语音识别运行中，实时文本：" + result);
            textResult = result;

        }
        else
        {
            Debug.Log("流式语音识别运行时出错，错误码：" + code);
        }
    }
}