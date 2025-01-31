using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

//从URL音频转到AudioSource Gameobject
public class StreamAudioFromURL : MonoBehaviour
{
    public AudioSource audioSource;
    public bool voiceClipCompleted;
    // public GameObject leftChatBox;
    // public GameObject rightChatBox;

    void Start()
    {voiceClipCompleted = false;

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }
    void Update(){
        if (voiceClipCompleted){
            Debug.Log("Voice Clip Completed");
        }

    }

    public IEnumerator GetAudioClip(string url)
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.WAV))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                
                Debug.Log(www.error);
            }
            else
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                audioSource.clip = clip;
                audioSource.Play();
                StartCoroutine(WaitForClipCompletion());
            }
        }
    }

    private IEnumerator WaitForClipCompletion()
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        Debug.Log("Audio clip completed!");
        // leftChatBox.SetActive(false);
        // rightChatBox.SetActive(false);
        // Perform any other actions you need
    }
}