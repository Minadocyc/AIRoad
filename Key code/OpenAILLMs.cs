using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using SimpleJSON;
using Unity.VisualScripting;

public class OpenAIStreamHandler : DownloadHandlerScript
{
    private StringBuilder stringBuilder = new StringBuilder();

    public OpenAIStreamHandler() : base(new byte[1024]) {}

    // Called when new data is received
    protected override bool ReceiveData(byte[] data, int dataLength)
    {
        if (dataLength > 0)
        {
            string text = Encoding.UTF8.GetString(data, 0, dataLength);
            stringBuilder.Append(text);
            return true;
        }

        return false;
    }

    public string GetContent()
    {
        string content = stringBuilder.ToString();
        stringBuilder.Clear();
        return content;
    }
}

public class OpenAILLMs : MonoBehaviour
{
    public string openaiURL;
    public string apiKey;
    private OpenAIStreamHandler streamHandler;
    private StringBuilder resultBuilder = new StringBuilder();
    public string output;
    public string modelName;
    public bool outputFinished;
    public string saying;
    public string sayingName;
    public bool sayingFinished;
    
    public void ChatToGPT(string system, string prompt)
    {
        output = "";
        sayingName = "";
        outputFinished = false;
        sayingFinished = false;
        resultBuilder.Clear();
        string requestBody = "{\"model\":\"" + modelName + "\",\"messages\":[{\"role\":\"system\",\"content\":\"" + system + "\"},{\"role\":\"user\",\"content\":\"" + prompt + "\"}],\"stream\":true}";
        Debug.Log(requestBody);
        StartCoroutine(SendRequestToOpenAI(requestBody));
    }

    IEnumerator SendRequestToOpenAI(string requestBody)
    {
        Debug.Log("开始发送请求给OpenAI");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(requestBody);
        streamHandler = new OpenAIStreamHandler();

        using (UnityWebRequest webRequest = new UnityWebRequest(openaiURL, "POST"))
        {
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = streamHandler;
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Authorization", "Bearer " + apiKey);

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(webRequest.error);
                outputFinished = true;
                Debug.Log("请求错误");

                
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
                if (output.Contains("<"))
                {
                
                    string splited = output.Split("<")[1];
                    sayingName = output.Split("<")[0];
                    if (splited.Contains(">"))
                    {
                        splited = splited.Split(">")[0];
                        outputFinished = true;
                    }

                    saying = splited;
                }
            }
            
        }

        
    }

    private void ProcessContent(string content)
    {
        // Debug.Log(content);
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