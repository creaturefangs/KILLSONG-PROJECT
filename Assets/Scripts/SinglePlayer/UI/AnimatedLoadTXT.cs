using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnimatedLoadTXT : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI textMeshPro;
    public GameObject panel;
    public Button startButton;
    public AudioClip startupSFX;

    public string customText = "Hello, World!";
    public float typingSpeed = 0.5f;
    public float sliderFillSpeed = 0.1f;
    public float delayBeforeTextAnimation = 1f;
    public float delayBeforePanelActivation = 1f;

    private AudioSource audioSource;

    private bool isAnimating = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Attach the button click event to the method that starts the animation
        if (startButton != null)
        {
            startButton.onClick.AddListener(StartAnimation);
        }
    }

    void StartAnimation()
    {
        if (!isAnimating)
        {
            // Play the sound effect
            if (audioSource != null && startupSFX!= null)
            {
                audioSource.PlayOneShot(startupSFX);
            }

            StartCoroutine(AnimateUI());
        }
    }

    IEnumerator AnimateUI()
    {
        isAnimating = true;

        // Animate the slider
        if (slider != null)
        {
            slider.gameObject.SetActive(true);
            yield return StartCoroutine(AnimateSliderFill(slider, sliderFillSpeed));
            slider.gameObject.SetActive(false);
        }

        // Wait for a delay before animating the text
        yield return new WaitForSeconds(delayBeforeTextAnimation);

        // Animate the text with typing effect
        if (textMeshPro != null)
        {
            textMeshPro.gameObject.SetActive(true);
            yield return StartCoroutine(TypeText(customText, textMeshPro, typingSpeed));
        }

        // Wait for a delay before activating the panel
        yield return new WaitForSeconds(delayBeforePanelActivation);

        // Activate the panel
        if (panel != null)
        {
            panel.SetActive(true);
        }

        isAnimating = false;
    }

    IEnumerator AnimateSliderFill(Slider slider, float fillSpeed)
    {
        float timer = 0f;

        while (timer < 1f)
        {
            timer += Time.deltaTime * fillSpeed;
            slider.value = Mathf.Lerp(0f, 1f, timer);
            yield return null;
        }
    }

    IEnumerator TypeText(string message, TextMeshProUGUI textComponent, float speed)
    {
        textComponent.text = "";
        foreach (char letter in message)
        {
            textComponent.text += letter;
            yield return new WaitForSeconds(speed);
        }
    }
}
