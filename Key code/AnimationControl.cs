using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControl : MonoBehaviour
{
    public Animator animator;
    public string animationName; // 替换为您的动画剪辑名称
    public float playTarget;
    public float transitionSpeed = 1f;
    

    void Update()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float currentNormalizedTime = stateInfo.normalizedTime % 1;
        float targetNormalizedTime = Mathf.Clamp01(playTarget);
        
        // 使用插值平滑过渡到目标归一化时间
        float newNormalizedTime = Mathf.Lerp(currentNormalizedTime, targetNormalizedTime, transitionSpeed * Time.deltaTime);
        animator.Play(animationName, 0, newNormalizedTime);
    }
    
}
