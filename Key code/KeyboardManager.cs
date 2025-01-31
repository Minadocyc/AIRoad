using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography.X509Certificates;
using GME;
using Unity.PlasticSCM.Editor.WebApi;

public class KeyboardManager : MonoBehaviour
{
    private bool spacePressed;
    public bool spaceTriggered;
    private bool leftArrowPressed;
    public bool leftArrowTrigged; 
    private bool rightArrowPressed; 
    public bool rightArrowTrigged; 
    private bool downArrowPressed;
    public bool downArrowTrigged;
    public string playerPosisiton;
    public bool isCollision;
    private bool leftBoom;
    private bool rightBoom;
    private bool zPressecd;
    private bool xPressed;
    public GameObject leftBoomAni;
    public GameObject rightBoomAni;
    public GameObject leftHumanAni;
    public bool aPressed;

    private StarManager starManager;
    public CarGenerationControl carGenerationControl;

    private float startTime;
    public int passCount;
    public int failCount;
    

    // Start is called before the first frame update
    void Start()
    {
        spacePressed = false;
        spaceTriggered = false;
        leftArrowPressed = false;
        leftArrowTrigged = false; 
        rightArrowPressed = false; 
        rightArrowTrigged = false; 
        downArrowPressed = false;
        downArrowTrigged = false;
        zPressecd = false;
        xPressed = false;
        leftBoom = false;
        rightBoom = false;
        leftBoomAni.SetActive(false);
        rightBoomAni.SetActive(false);
        starManager = GameObject.Find("SoulEverythingControl").GetComponent<StarManager>();
        string path = "D:/Aiagent for autism/TimeLog" + "/event_time.txt";
        File.AppendAllText(path, "[Experiment Start]["+Time.time+"]\n");
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)){
            spacePressed = true;
            Debug.Log("SPACE pressed " + spacePressed);
        }
        if (Input.GetKeyUp(KeyCode.Space) & spacePressed){
            spacePressed = false;
            //spaceTriggered = true;
            starManager.SetActiveObject();
            //Debug.Log("SPACE trigged " + spaceTriggered);
        }
        
        if (Input.GetKeyDown(KeyCode.LeftArrow)){
            leftArrowPressed = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow) & leftArrowPressed){
            leftArrowPressed = false;
            leftArrowTrigged = true;
            carGenerationControl.GenerateLeftDelayed();
            startTime = Time.time;
            Debug.Log("LEFT Arrow trigged " + leftArrowTrigged);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow)){
            rightArrowPressed = true;
        }
        if (Input.GetKeyUp(KeyCode.RightArrow) & rightArrowPressed){
            rightArrowPressed = false;
            rightArrowTrigged = true;
            startTime = Time.time;
            carGenerationControl.GenerateRightDelayed();
            Debug.Log("RIGHT Arrow trigged " + rightArrowTrigged);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow)){
            downArrowPressed = true;
        }
        if (Input.GetKeyUp(KeyCode.DownArrow) & downArrowPressed){
            downArrowPressed = false;
            downArrowTrigged = true;
            float elapsedTime = Time.time - startTime; // Calculate elapsed time
            Debug.Log($"Elapsed Time: {elapsedTime} seconds");
            if (playerPosisiton == "左侧斑马线" | playerPosisiton == "右侧斑马线"){
                isCollision = true;
            }
            SaveElapsedTime();
            carGenerationControl.PassCar();
            
            Debug.Log("DOWN Arrow trigged " + downArrowTrigged);
        }
        if (Input.GetKeyDown(KeyCode.Z)){
            zPressecd = true;
        }
        if (Input.GetKeyUp(KeyCode.Z) & zPressecd){
            zPressecd = false;
            leftBoomAni.SetActive(true);
            Debug.Log("Left Pressed");
            string path = "D:/Aiagent for autism/TimeLog" + "/event_time.txt";
            File.AppendAllText(path, "小朋友在左侧斑马线过马路失败，车辆类型"+carGenerationControl.carName+"\n");
            RecordLeftBoom();
            Invoke("CancelLeftBoom", 2f);
        }
        if (Input.GetKeyDown(KeyCode.X)){
            xPressed = true;
        }
        if (Input.GetKeyUp(KeyCode.X) & xPressed){
            xPressed = false;
            Debug.Log("Right Pressed");
            string path = "D:/Aiagent for autism/TimeLog" + "/event_time.txt";
            File.AppendAllText(path, "小朋友在右侧斑马线过马路失败，车辆类型"+carGenerationControl.carName+"\n");
            RecordRightBoom();
            rightBoomAni.SetActive(true);
            Invoke("CancelRightBoom", 2f);
        }
        if (Input.GetKeyDown(KeyCode.A)){
            aPressed = true;
        }
        if (Input.GetKeyUp(KeyCode.A) & aPressed){
            aPressed = false;
            Debug.Log("Left Human Pressed");
            leftHumanAni.SetActive(true);
            Invoke("CancelLeftHuman", 2f);
        }

    }
    private void CancelLeftBoom(){
        leftBoomAni.SetActive(false);
    }
    private void CancelRightBoom(){
        rightBoomAni.SetActive(false);
    }

    private void CancelLeftHuman(){
        leftHumanAni.SetActive(false);
    }

    public void SaveElapsedTime()
    {
        string path = "D:/Aiagent for autism/TimeLog" + "/elapsed_time.txt";
        string eventPath = "D:/Aiagent for autism/TimeLog" + "/event_time.txt";
        //Debug.Log("Saving Path " + path);
        float elapsedTime = Time.time - startTime - 2;
        string elapsedTimeString = elapsedTime.ToString("F2"); // Format to 2 decimal places
        //File.AppendAllText(path, elapsedTimeString);
        string currentTime = Time.time.ToString();
        File.AppendAllText(path, "["+currentTime+"]");
        if (isCollision){
            isCollision = false;
            File.AppendAllText(path,  "[小朋友在" + playerPosisiton + "通过失败,车辆" + carGenerationControl.carName+ ",使用时间"+elapsedTimeString+"s]");
            File.AppendAllText(eventPath,  "小朋友在" + playerPosisiton + "通过失败,车辆" + carGenerationControl.carName+ ",使用时间"+elapsedTimeString+"s|\n");
        } else {
            
            File.AppendAllText(path,  "[小朋友在" + playerPosisiton + "通过成功,车辆" + carGenerationControl.carName+ ",使用时间"+elapsedTimeString+"s]");
            File.AppendAllText(eventPath,  "小朋友在" + playerPosisiton + "通过成功,车辆" + carGenerationControl.carName+ ",使用时间"+elapsedTimeString+"s|\n");
        }
        
        //Debug.Log($"Elapsed Time saved to {path}");
    }
    private void RecordLeftBoom(){
        string eventPath = "D:/Aiagent for autism/TimeLog" + "/event_time.txt";
        string path = "D:/Aiagent for autism/TimeLog" + "/elapsed_time.txt";
        string currentTime = Time.time.ToString();
        float elapsedTime = Time.time - startTime - 2;
        string elapsedTimeString = elapsedTime.ToString("F2");
        File.AppendAllText(eventPath, "["+currentTime+"]LeftBoom\n");
        File.AppendAllText(path,  "[小朋友在" + playerPosisiton + "通过失败,车辆" + carGenerationControl.carName+ ",使用时间"+elapsedTimeString+"s]");
    }
    private void RecordRightBoom(){
        string eventPath = "D:/Aiagent for autism/TimeLog" + "/event_time.txt";
        string path = "D:/Aiagent for autism/TimeLog" + "/elapsed_time.txt";
        string currentTime = Time.time.ToString();
        float elapsedTime = Time.time - startTime - 2;
        string elapsedTimeString = elapsedTime.ToString("F2");
        File.AppendAllText(eventPath, "["+currentTime+"]RightBoom\n");
        File.AppendAllText(path,  "[小朋友在" + playerPosisiton + "通过失败,车辆" + carGenerationControl.carName+ ",使用时间"+elapsedTimeString+"s]");
    }
}
