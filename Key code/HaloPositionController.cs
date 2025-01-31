using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaloPositionController : MonoBehaviour
{
    public GameObject halo;
    public GameObject player;

    public void SetTransformFromGameObject(GameObject targetObject)
    {
        // 获取目标物体的 Transform
        Transform targetTransform = targetObject.transform;

        // 将目标物体的 Transform 赋值给当前物体
        transform.position = targetTransform.position;
        transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
        transform.localScale = targetTransform.localScale;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HaloPositionController haloPositionController = GetComponent<HaloPositionController>();
        haloPositionController.SetTransformFromGameObject(player);
    }
}
