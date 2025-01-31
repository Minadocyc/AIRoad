using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using TMPro;

public class StarManager : MonoBehaviour
{
    public bool isTriggerClicked = false;
    public List<GameObject> hardList; 
    public List<GameObject> easyList;
    private List<GameObject> starList;
    public ProcedureManager procedureManager;
    private int currentIndex = 1;
    private int totalIndex;
    private int star_mode;

    public TMP_Text timerDisplay;
    public float countdown = 30f; // Initial countdown time
    public int score = 0; // Example additional variable
    private float startTime;

    // private List<int> oneList = new List<int>();
    // private List<int> secList = new List<int>();
    public CarGenerationControl carGenerationControl;
    public GameObject success;
    public bool successFlag = false;
    public bool victor = false;
    // Start is called before the first frame update
    void Start()
    {
        successFlag = false;
        success.SetActive(false);
        startTime = Time.time;

        isTriggerClicked = false;

        star_mode = procedureManager.selection;
        for (int i = 0; i < easyList.Count; i++)
        {
            easyList[i].SetActive(false);
        }
        for (int i = 0; i < hardList.Count; i++)
        {
            hardList[i].SetActive(false);
        }

        if (star_mode == 1)
        {
            starList = easyList;
            // oneList.AddRange(new int[] {0,3,4,7,8});
            // secList.AddRange(new int[] {8});
            // carGenerationControl.isRoadOne = true;
            // carGenerationControl.isSecOne = false;
            
        }
        else if (star_mode == 2)
        {
            starList = hardList;
            // oneList.AddRange(new int[] {0,2,4,5,7,10});
            // secList.AddRange(new int[] {0,1,2,5,6,9,10});
            // carGenerationControl.isRoadOne = true;
            // carGenerationControl.isSecOne = false;
        }
        // Debug.Log("is Road One " + carGenerationControl.isRoadOne);
        // Debug.Log("is Sec One " + carGenerationControl.isSecOne);
        

        totalIndex = starList.Count - 1;
        starList[0].SetActive(true);
        Debug.Log("current mode " + star_mode + "with total stars " + totalIndex);
        carGenerationControl = GameObject.Find("SoulEverythingControl").GetComponent<CarGenerationControl>();
        //carGenerationControl.SpawnCar();
    }

    // Update is called once per frame
    void Update()
    {
        if (!successFlag){
            float currentTime = Time.time  - startTime;
            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);
            timerDisplay.text = $"时间: {minutes:D2}:{seconds:D2}\n星星数: {currentIndex-1}-{totalIndex+1}";
        }

        // 检测按键 "l" 是否被按下
        if (Input.GetKeyDown(KeyCode.L))
        {
            isTriggerClicked = true;
        }

        //Debug.Log(isTriggerClicked);
        //    if (isTriggerClicked)
        //   {
        //   isTriggerClicked = false;
        //
        //   SetActiveObject();
        //   currentIndex += 1;
        //carGenerationControl.SpawnCar(); 
        //}
    }

 

public void SetActiveObject()
    {
        Debug.Log("Next star trigged, star index " + currentIndex);
        if (totalIndex >= currentIndex)
        {
            starList[currentIndex-1].SetActive(false);
            starList[currentIndex].SetActive(true);
            currentIndex += 1;
            // if (star_mode == 1)
            // {
            //     if (oneList.Contains(index-1))
            //     {
                    
            //         carGenerationControl.isRoadOne = true; 
            //         Debug.Log("is Road One " + carGenerationControl.isRoadOne);
            //     }
            //     if (secList.Contains(index-1))
            //     {
            //         carGenerationControl.isSecOne = true;
            //         Debug.Log("is Sec One " + carGenerationControl.isSecOne);
            //     }      
            // }
        }
        else{
            Debug.Log("Reach the end of the list");
            //TODO trigger Win!!!
            starList[currentIndex-1].SetActive(false);
            currentIndex  += 1;
            success.SetActive(true);
            successFlag = true;
            float currentTime = Time.time  - startTime;
            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);
            timerDisplay.text = $"时间: {minutes:D2}:{seconds:D2}\n星星数: {currentIndex-1}-{totalIndex+1}";
        }
    }
}
