using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Valve.VR;
using System.IO;

public class TrackAdjust : MonoBehaviour
{
    public bool needIt;
    public SteamVR_Input_Sources handType; 
    public SteamVR_Action_Boolean triggerAction;

    public GameObject righthandObject;
    public TMP_Text text;
    public int step;

    public GameObject CAVEleftdown;
    public GameObject CAVEleftup;
    public GameObject CAVErightup;

    public GameObject CAVEleftUpHeight;

    //public GameObject CAVEleftupheight;

    
    private void Start()
    {
        LoadPositions();
        if (!needIt)
        {
            gameObject.SetActive(false);
        }
        step = -1;
        NextStep();
    }

    void Update()
    {
        if (triggerAction.GetStateDown(handType))
        {
            NextStep();
            Debug.Log("Trigger is pressed down");
        }

        // 检查触发器是否被持续按住
        if (triggerAction.GetState(handType))
        {
            Debug.Log("Trigger is held down");
        }

        // 检查触发器是否被释放
        if (triggerAction.GetStateUp(handType))
        {
            Debug.Log("Trigger is released");
        }
    }

    private string[] hints = { 
        "准备调整定位后，按下右手扳机键开始校正程序",
        "1. 环境配准 地板左下：请将有麦克风的手柄放到地板左下角，并且按下扳机键",  // 0
        "2. 环境配准 地板左上：请将有麦克风的手柄放到地板左上角，并且按下扳机键", // 1
        "3. 环境配准 地板右上：请将有麦克风的手柄放到地板右上角，并且按下扳机键",// 2
        "4. 环境配准 左上门框边：请将有麦克风的手柄放到地板右上角，并且按下扳机键", // 3
    };

    void NextStep()
    {
        step++;
        if(step > 0){
            GameObject[] cavePoints = { CAVEleftdown, CAVEleftup, CAVErightup, CAVEleftUpHeight};
            SavePosition(step - 1, righthandObject.transform.position);

            if (step - 1 < hints.Length)
            {
                // 更新文本指示并保存位置
                
                cavePoints[step - 1].transform.position = righthandObject.transform.position;
            }
            else
            {

                text.text = "校正完成！按扳机键退出";
            }
        }
        text.text = hints[step];


    }

    void SavePosition(int step, Vector3 position)
    {
        string fileName = "Position_Step" + step + ".txt";
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        Debug.Log(filePath);
        
        string content = position.x + "," + position.y + "," + position.z;
        File.WriteAllText(filePath, content);
    }

    void LoadPositions()
    {
        GameObject[] cavePoints = { CAVEleftdown, CAVEleftup, CAVErightup, CAVEleftUpHeight};

        for (int i = 0; i < cavePoints.Length; i++)
        {
            string fileName = "Position_Step" + i + ".txt";
            string filePath = Path.Combine(Application.persistentDataPath, fileName);

            if (File.Exists(filePath))
            {
                string[] positionStr = File.ReadAllText(filePath).Split(',');
                Vector3 position = new Vector3(
                    float.Parse(positionStr[0]),
                    float.Parse(positionStr[1]),
                    float.Parse(positionStr[2]));

                cavePoints[i].transform.position = position;
            }
        }
    }
    

    
    
}
