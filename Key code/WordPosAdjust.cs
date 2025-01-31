using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordPosAdjust : MonoBehaviour
{
    public Transform realWorldLeftDown;
    public Transform realWorldLeftTop;
    public Transform realWorldRightTop;
    public Transform realWorldLeftTopHeight;


    //public Transform realWorldLeftTopDoor;

    public Transform virtualLeftDown;
    public Transform virtualLeftTop;
    public Transform virtualRightTop;
    public Transform virtualLeftTopHeight;

    //public Transform virtualLeftTopDoor;

    public GameObject trackedObject;
    public GameObject virtualObject;

    void Start()
    {

    }

    void Update()
    {
        Vector3 trackedPos = trackedObject.transform.position;

        // 计算x轴比例
        float xRatio = DistancePointToLine(trackedPos, realWorldLeftTop.position, realWorldLeftDown.position) / DistancePointToLine(realWorldRightTop.position, realWorldLeftTop.position, realWorldLeftDown.position);

        // 计算y轴比例
        //float yRatio = CalculateRatio(trackedPos, realWorldLeftDown.position, realWorldLeftTop.position);

        //float zRatio = CalculateRatio(trackedPos, realWorldLeftTop.position, realWorldLeftTopHeight.position);


        // 根据比例计算虚拟世界中的位置
        Vector3 xDirection = virtualRightTop.position - virtualLeftTop.position;
        //Vector3 yDirection = virtualLeftTop.position - virtualLeftDown.position;
        //Vector3 zDirection = virtualLeftTop.position - virtualLeftTopHeight.position;

        Vector3 virtualPos = virtualLeftTop.position + xDirection * xRatio;// + yDirection * yRatio - zDirection * zRatio;

        // 更新虚拟物体的位置
        virtualObject.transform.position = new Vector3(virtualPos.x, virtualPos.y, virtualPos.z);
    }

    // 计算点到线段比例的函数
    float CalculateRatio(Vector3 point, Vector3 lineStart, Vector3 lineEnd)
    {
        Vector3 lineDir = lineEnd - lineStart;
        Vector3 pointDir = point - lineStart;
        
        // 点到线段的投影长度
        float projectionLength = Vector3.Dot(pointDir, lineDir.normalized);

        // 线段的长度
        float lineLength = lineDir.magnitude;

        // 计算比例
        return projectionLength / lineLength;
    }
    
    public float DistancePointToLine(Vector3 point, Vector3 lineStart, Vector3 lineEnd)
    {
        // 计算直线的方向向量，也即是从起点到终点的向量
        Vector3 lineDirection = lineEnd - lineStart;

        // 计算从直线起点到点的向量
        Vector3 lineToPoint = point - lineStart;

        // 投影lineToPoint向量到lineDirection方向上，得到点在直线上的投影点
        Vector3 projection = Vector3.Project(lineToPoint, lineDirection);

        // 计算点的投影和原点之间的向量
        Vector3 pointToProjection = lineToPoint - projection;

        // 点到直线的距离就是这个向量的长度
        float distance = pointToProjection.magnitude;

        return distance;
    }
}