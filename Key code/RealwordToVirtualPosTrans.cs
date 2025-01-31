using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealwordToVirtualPosTrans : MonoBehaviour
{
    public bool isVisible;
    public GameObject alignTigs;
    public Transform virtualTransformLeft;
    public Transform virtualTransformRight;
    public Transform realwordTransformLeft;

    public Transform realwordTransformRight;

    public Transform scenes;

    public bool useX;

    public float offsetX;

    public bool useY;
    public float offsetY;

    public bool useZ;
    public float offsetZ;

    public float scale = 1;

    public float rotation = 0;

    private List<Transform> realworldTransList = new List<Transform>();
    private List<Transform> virtualTransList = new List<Transform>();


    // Start is called before the first frame update
    void Start()
    {
        realworldTransList.Add(realwordTransformLeft);
        realworldTransList.Add(realwordTransformRight);
        virtualTransList.Add(virtualTransformLeft);
        virtualTransList.Add(virtualTransformRight);
        if(isVisible){
            virtualTransformLeft.gameObject.SetActive(true);
            virtualTransformRight.gameObject.SetActive(true);
            //realwordTransformLeft.gameObject.SetActive(true);
            //realwordTransformRight.gameObject.SetActive(true);
            alignTigs.SetActive(true);
        }else{
            virtualTransformLeft.gameObject.SetActive(false);
            virtualTransformRight.gameObject.SetActive(false);
            //realwordTransformLeft.gameObject.SetActive(false);
            //realwordTransformRight.gameObject.SetActive(false);
            alignTigs.SetActive(false);

        }


    }

    // Update is called once per frame
    void Update()
    {

        for (int i = 0; i < realworldTransList.Count; i++)
        {
            
            
            virtualTransList[i].position = new Vector3
            (useX ? realworldTransList[i].position.x * scale + offsetX : virtualTransList[i].position.x, 
            useY ? realworldTransList[i].position.y * scale + offsetY : virtualTransList[i].position.y, 
            useZ ? realworldTransList[i].position.z * scale + offsetZ : virtualTransList[i].position.z);

            //virtualTransList[i].rotation = Quaternion.Euler(0f, rotation, 0f);
        }

        scenes.transform.rotation = Quaternion.Euler(0f, rotation, 0f);
        virtualTransformLeft.rotation = Quaternion.Euler(0f, rotation, 0f);
        virtualTransformRight.rotation = Quaternion.Euler(0f, rotation, 0f);

    }

    public void ConvertPosition(Transform realworldTrans, Transform virtualTrans){
        realworldTransList.Add(realworldTrans);
        virtualTransList.Add(virtualTrans);
    }
}
