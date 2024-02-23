using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingScreen : MonoBehaviour
{
    public TMP_Text loadingText;
    public TMP_Text completionText;
    public GameObject loadingScreen;

    private const int maxPeriods = 3;

    private void Start()
    {
        // Optionally, you can attach this script to a button click event.
    }

    // Method to be called when the button is clicked
    public void StartLoadingScreen()
    {
        StartCoroutine(ShowLoadingScreen());
    }

    private IEnumerator ShowLoadingScreen()
    {
        loadingScreen.SetActive(true);

        // Initialize loading text
        loadingText.text = "Loading";

        // Simulate a time-consuming task
        for (int i = 0; i < 100; i++)
        {
            // Do some work
            yield return new WaitForSeconds(0.05f); // Simulate work by waiting for 0.05 seconds

            // Update loading text
            loadingText.text = "Loading" + new string('.', (i / 20) % (maxPeriods + 1));

            // Reset loading text after displaying three periods
            if (loadingText.text.Length > maxPeriods + 7) // "Loading" length + maxPeriods
            {
                loadingText.text = "Loading";
            }

            // Display UPLINK COMPLETION and percentage
            if (i == 99)
            {
                completionText.text = "UPLINK COMPLETION: " + i + "%";
                
            }
           
        }

        // Hide the loading screen after the task is complete
        loadingScreen.SetActive(false);
    }
}






