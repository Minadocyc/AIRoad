using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class TextOverflowController : MonoBehaviour
{
    
    private TMP_Text textComponent;
    public int maxVisibleCharacters = 32; // 设置为你的文本框允许的最大字符数

    void Awake()
    {
        textComponent = GetComponent<TMP_Text>();
    }

    void Update()
    {
        // 确保textInfo已经更新
        textComponent.ForceMeshUpdate();

        if (textComponent.textInfo.characterCount > maxVisibleCharacters)
        {
            // 超过最大字符数时，从文本开头剔除多余字符
            int removeCount = textComponent.textInfo.characterCount - maxVisibleCharacters;
            textComponent.text = textComponent.text.Substring(removeCount);
            textComponent.ForceMeshUpdate(); // 更新Mesh，这一步很重要
        }
    }
}