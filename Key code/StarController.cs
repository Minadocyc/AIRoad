using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarController : MonoBehaviour
{
    private float minY = 0f; // 最小 y 值
    private float maxY = 50f; // 最大 y 值
    private float speed = 20f; // 移动速度

    private RectTransform rectTransform; 

    private float initialY;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        initialY = rectTransform.anchoredPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        float newY = Mathf.PingPong(Time.time * speed, maxY - minY) + minY;
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, initialY + newY);
    }
}
