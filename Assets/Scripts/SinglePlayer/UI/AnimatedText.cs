using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AnimatedText : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    
    public float animationDuration = 2f;

    void Start()
    {
        StartCoroutine(AnimateTextAndActivatePanel());
    }

    IEnumerator AnimateTextAndActivatePanel()
    {
        // Store the original text
        string originalText = textMeshPro.text;
        textMeshPro.text = "";

        // Animate the text
        float timer = 0f;
        while (timer < animationDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / animationDuration;
            int endIndex = Mathf.RoundToInt(Mathf.Lerp(0, originalText.Length, progress));
            textMeshPro.text = originalText.Substring(0, endIndex);
            yield return null;
        }

    }
}
