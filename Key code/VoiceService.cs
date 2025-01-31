using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

// 语音生成服务
public class VoiceService : MonoBehaviour
{
    private readonly HttpClient _client;
    public const string BaseUrl = "https://bv2.firefly.matce.cn/run/predict";


    public string voiceText;
    public string voiceCharacterName;

    public VoiceService()
    {
        HttpClientHandler handler = new HttpClientHandler
        {
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
        };

        _client = new HttpClient(handler);
        InitializeHeaders(_client);
    }

    private void InitializeHeaders(HttpClient client)
    {

        client.DefaultRequestHeaders.Add("authority", "bv2.firefly.matce.cn");
        client.DefaultRequestHeaders.Add("accept", "*/*");
        client.DefaultRequestHeaders.Add("accept-language", "zh-CN,zh;q=0.9,en-US;q=0.8,en-GB;q=0.7,en;q=0.6");
        client.DefaultRequestHeaders.Add("cookie", "_gid=GA1.2.306252802.1711001208; _gat_gtag_UA_156449732_1=1; _ga_R1FN4KJKJH=GS1.1.1711001207.1.1.1711002236.0.0.0; _ga=GA1.1.1672963870.1711001208");
        client.DefaultRequestHeaders.Add("origin", "https://bv2.firefly.matce.cn");
        client.DefaultRequestHeaders.Add("referer", "https://bv2.firefly.matce.cn");
        client.DefaultRequestHeaders.Add("sec-ch-ua", "\"Chromium\";v=\"122\", \"Not(A:Brand\";v=\"24\", \"Microsoft Edge\";v=\"122\"");
        client.DefaultRequestHeaders.Add("sec-ch-ua-mobile", "?0");
        client.DefaultRequestHeaders.Add("sec-ch-ua-platform", "\"Windows\"");
        client.DefaultRequestHeaders.Add("sec-fetch-dest", "empty");
        client.DefaultRequestHeaders.Add("sec-fetch-mode", "cors");
        client.DefaultRequestHeaders.Add("sec-fetch-site", "same-origin");
        client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/122.0.0.0 Safari/537.36 Edg/122.0.0.0");

    }

    public async Task<string> SendVoiceDataAsync(string jsonPayload)
    {
        using var requestContent = new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json");
        using var request = new HttpRequestMessage(HttpMethod.Post, BaseUrl) { Content = requestContent };
        using var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> VoiceGeneration()
    {
        string jsonPayload = "{\"data\":[\"" + voiceText + "\",\""+voiceCharacterName+"_ZH\",0.5,0.6,0.9,1.1,\"ZH\",true,1,0.2,null,\"Happy\",\"\",0.7],\"event_data\":null,\"fn_index\":0,\"session_hash\":\"r1jl6bg90i\"}";

        Debug.Log(jsonPayload);
        try
        {
            string response = await SendVoiceDataAsync(jsonPayload);

            // Parse the response to extract the 'path' value
            string path = response.Split("\"name\":\"")[1].Split("\",\"data\"")[0];
            return ("https://bv2.firefly.matce.cn/file=" + path);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error sending voice data: {e.Message}");
            return "null";
        }
    }

    // Removed the GetResponse method and the callback mechanism.
}