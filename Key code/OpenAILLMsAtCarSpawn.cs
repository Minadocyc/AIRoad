using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using SimpleJSON;
using System.Text.RegularExpressions;
using Unity.VisualScripting;


public class OpenAILLMsAtCarSpawn : MonoBehaviour
{ 
    public string openaiURL;
    public string apiKey;
    private OpenAIStreamHandler streamHandler;
    private StringBuilder resultBuilder = new StringBuilder();
    public string output;
    public string modelName;
    public bool outputFinished;
    public string dialog1;
    public string carName1;
    public bool action1;
    public bool voice1;
    public bool Human1;
    public string dialog2;
    public string carName2;
    public bool action2;
    public bool voice2;
    public bool Human2;
    public bool sayingFinished;
    private bool processStart;
    private CarGenerationControl carGenerationControl;
    
    void Awake()
    {
        processStart = false;
    }
    
    public void ChatToGPT(string system, string prompt)
    {
        output = "";
        carName1 = "";
        carName2 = "";
        outputFinished = false;
        sayingFinished = false;
        resultBuilder.Clear();
        output = "";
        string requestBody = "{\"model\":\"" + modelName + "\",\"messages\":[{\"role\":\"system\",\"content\":\"" + system + "\"},{\"role\":\"user\",\"content\":\"" + prompt + "\"}],\"stream\":true}";
        Debug.Log(requestBody);
        carGenerationControl = GameObject.Find("SoulEverythingControl").GetComponent<CarGenerationControl>();
        StartCoroutine(SendRequestToOpenAI(requestBody));
    }

    IEnumerator SendRequestToOpenAI(string requestBody)
    {
        Debug.Log("开始发送请求给OpenAI in openai");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(requestBody);
        streamHandler = new OpenAIStreamHandler();

        using (UnityWebRequest webRequest = new UnityWebRequest(openaiURL, "POST"))
        {
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = streamHandler;
            
            webRequest.SetRequestHeader("Content-Type", "application/json");
            apiKey = "sk-fF4yhBsJAyFww0lb3a8d39F76f41432eAbC03eAb0dCbBb80";
            webRequest.SetRequestHeader("Authorization", "Bearer " + apiKey);
            //Debug.Log("WebRquest Header: ", webRequest.GetRequestHeader);

            yield return webRequest.SendWebRequest();
            
            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(webRequest.error);
                outputFinished = true;
                Debug.Log("请求错误");
            }
            else{
                Debug.Log("webRequest.result " + webRequest.result);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (streamHandler != null)
        {
            string content = streamHandler.GetContent();
            if (!string.IsNullOrEmpty(content))
            {
                // Process the content received in this frame
                ProcessContent(content);
            }
            output = resultBuilder.ToString();
            if (!outputFinished)
            {
                // 尝试解析JSON
                if (output.EndsWith("]}") & !processStart)
                {
                    Debug.Log("Output completed " + output);
                    processStart = true;
                }
                try
                {
                    Debug.Log("Getting output... "+ output);
                    
                    //VehicleData data = JsonUtility.FromJson<VehicleData>(output);
                    
                    if (processStart)
                    {
                        Debug.Log("Processing");
                        string data = output.TrimStart('{').TrimEnd('}');
                        string[] cars = data.Split(new[] { "][" }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string car in cars)
                        {
                            // Split each pair by '|' to separate key and value
                            string[] pairs = car.Split('|');
                            foreach (string pair in pairs)
                            {
                                string[] keyValue = pair.Split(':');
                                // Extract the key and value
                                string key = keyValue[0];
                                string value = keyValue[1];
                                // Extract specific values based on keys
                                if (key.Contains("CarName1"))
                                {
                                    carName1 = Regex.Replace(value, @"[\[\]{}<>]", "");
                                    //Debug.Log($"CarName1: {carName1}");
                                }
                                else if (key.Contains("SpeakContent1"))
                                {
                                    dialog1 = Regex.Replace(value, @"[\[\]{}<>]", "");
                                    //Debug.Log($"SpeakContent: {dialog1}");
                                }
                                else if (key.Contains("Action1"))
                                {
                                    string actionString1 = Regex.Replace(value, @"[\[\]{}<>]", "");
                                    if (actionString1 == "是")
                                    {
                                        action1 = true;
                                    }
                                    else
                                    {
                                        action1 = false;
                                    }
                                    //Debug.Log($"Action: {action1}");
                                }
                                else if (key.Contains("Voice1"))
                                {
                                    string voiceString1 = Regex.Replace(value, @"[\[\]{}<>]", "");
                                    if (voiceString1 == "是")
                                    {
                                        voice1 = true;
                                    }
                                    else
                                    {
                                        voice1 = false;
                                    }
                                }
                                else if (key.Contains("Human1"))
                                {
                                    string humanString1 = Regex.Replace(value, @"[\[\]{}<>]", "");
                                    if (humanString1 == "是")
                                    {
                                        Human1 = true;
                                    }
                                    else
                                    {
                                        Human1 = false;
                                    }
                                }
                                else if (key.Contains("CarName2"))
                                {
                                    carName2 = Regex.Replace(value, @"[\[\]{}<>]", "");
                                    //Debug.Log($"CarName2: {carName2}");
                                }
                                else if (key.Contains("SpeakContent2"))
                                {
                                    dialog2 = Regex.Replace(value, @"[\[\]{}<>]", "");
                                    //Debug.Log($"SpeakContent2: {dialog2}");
                                }
                                else if (key.Contains("Action2"))
                                {
                                    string actionString2 = Regex.Replace(value, @"[\[\]{}<>]", "");
                                    if (actionString2 == "是")
                                    {
                                        action2 = true;
                                    }
                                    else
                                    {
                                        action2 = false;
                                    }
                                }
                                else if (key.Contains("Voice2"))
                                {
                                    string voiceString2 = Regex.Replace(value, @"[\[\]{}<>]", "");
                                    if (voiceString2 == "是")
                                    {
                                        voice2 = true;
                                    }
                                    else
                                    {
                                        voice2 = false;
                                    }
                                }
                                else if (key.Contains("Human2"))
                                {
                                    string humanString2 = Regex.Replace(value, @"[\[\]{}<>]", "");
                                    if (humanString2 == "是")
                                    {
                                        Human2 = true;
                                    }
                                    else
                                    {
                                        Human2 = false;
                                    }
                                }
                            }
                            
                        }
                        processStart = false;
                        output = "";
                        resultBuilder.Clear();
                        // 抽取车辆名字、说话的内容和是否执行行为
                        // Debug.Log("1车辆名字: " + carName1);
                        // Debug.Log("1说话的内容: " + dialog1);
                        // Debug.Log("1是否执行行为: " + action1);
                        // Debug.Log("2车辆名字: " + carName2);
                        // Debug.Log("2说话的内容: " + dialog2);
                        // Debug.Log("2是否执行行为: " + action2);
                        carGenerationControl.nameQueue.Enqueue(carName1);
                        carGenerationControl.dialogQueue.Enqueue(dialog1);
                        carGenerationControl.actionQueue.Enqueue(action1);
                        carGenerationControl.speakQueue.Enqueue(voice1);
                        carGenerationControl.humanQueue.Enqueue(Human1);
                        carGenerationControl.nameQueue.Enqueue(carName2);
                        carGenerationControl.dialogQueue.Enqueue(dialog2);
                        carGenerationControl.actionQueue.Enqueue(action2);
                        carGenerationControl.speakQueue.Enqueue(voice2);
                        carGenerationControl.humanQueue.Enqueue(Human2);

                        outputFinished = true;
                    }
                }
                catch (Exception ex)
                {
                    // 输出错误信息
                    Debug.LogError("解析JSON时出错: " + ex.Message);
                }
            }
            
        }

        
    }

    private void ProcessContent(string content)
    {
        //Debug.Log("content for process: " + content);

        // content == [DONE] 时 完成输出， 进行处理
        // Parse the content and extract information as needed
        var dataLines = content.Split(new string[] { "\n" }, System.StringSplitOptions.None);
        foreach (var line in dataLines)
        {
            if (line.StartsWith("data: "))
            {
                string jsonLine = line.Substring("data: ".Length);
                JSONNode json = JSON.Parse(jsonLine);
                if (json["choices"].IsArray)
                {
                    foreach (JSONNode choice in json["choices"].AsArray)
                    {
                        if (choice["finish_reason"] == "stop")
                        {
                            outputFinished = true;
                        }
                        else if (choice["delta"] != null && choice["delta"]["content"] != null)
                        {
                            string messageContent = choice["delta"]["content"];
                            // Do something with messageContent
                            //Debug.Log(messageContent);
                            // Append the content to the result builder
                            resultBuilder.Append(messageContent);
                        }
                        
                    }
                }
            }
        }
    }
}