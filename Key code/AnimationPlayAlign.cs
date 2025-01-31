using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPlayAlign : MonoBehaviour
{
    public enum DirectionEnum { x, y, z }
    
    public GameObject leftObject;
    public GameObject rightObject;
    public GameObject playerObject;
    public AnimationControl animationControl; // Assuming AnimationControl is a class you've defined elsewhere
    public DirectionEnum direction;

    private float focusTime = 10f; // Time in seconds to stay focused
    private float notFocusTime = 5f; // Time in seconds to not focus
    private float lastChangeTime;
    private bool isFocusing = true;
    
    // Randomness parameters
    private float notFocusTarget;
    
    void Start()
    {
        lastChangeTime = Time.time;
        SetRandomNotFocusTarget();
    }

    void Update()
    {
        float left = 0;
        float right = 0;
        float eyesMove = 0;
        
        switch (direction)
        {
            case DirectionEnum.x:
                left = leftObject.transform.position.x;
                right = rightObject.transform.position.x;
                eyesMove = playerObject.transform.position.x;
                break;
            case DirectionEnum.y:
                left = leftObject.transform.position.y;
                right = rightObject.transform.position.y;
                eyesMove = playerObject.transform.position.y;
                break;
            case DirectionEnum.z:
                left = leftObject.transform.position.z;
                right = rightObject.transform.position.z;
                eyesMove = playerObject.transform.position.z;
                break;
        }

        if (isFocusing)
        {
            // If we are focusing, set the playTarget to the correct position
            animationControl.playTarget = (eyesMove - left) / (right - left);
            if (Time.time - lastChangeTime > focusTime)
            {
                // It's time to not focus
                isFocusing = false;
                lastChangeTime = Time.time;
                SetRandomNotFocusTarget();
            }
        }
        else
        {
            // If we are not focusing, set the playTarget to a random target
            animationControl.playTarget = notFocusTarget;
            if (Time.time - lastChangeTime > notFocusTime)
            {
                // It's time to focus again
                isFocusing = true;
                lastChangeTime = Time.time;
            }
        }
    }

    private void SetRandomNotFocusTarget()
    {
        // Set a random not focus target based on the current direction
        switch (direction)
        {
            case DirectionEnum.x:
                notFocusTarget = Random.Range(leftObject.transform.position.x, rightObject.transform.position.x);
                break;
            case DirectionEnum.y:
                notFocusTarget = Random.Range(leftObject.transform.position.y, rightObject.transform.position.y);
                break;
            case DirectionEnum.z:
                notFocusTarget = Random.Range(leftObject.transform.position.z, rightObject.transform.position.z);
                break;
        }
    }
}