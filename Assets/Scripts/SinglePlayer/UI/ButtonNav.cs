using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonNav : MonoBehaviour
{

    public string sceneName; // The name of the scene to load
    public Button loadSceneButton; // Reference to the button that triggers the scene load (optional)

    private void Start()
    {
        // If a button is assigned, add the onClick listener
        if (loadSceneButton != null)
        {
            loadSceneButton.onClick.AddListener(() => LoadSceneWithDelay(sceneName));
        }
    }

    // Public method to be called to start the scene loading process
    public void LoadSceneWithDelay(string sceneName)
    {
        StartCoroutine(LoadSceneAfterDelay(sceneName));
    }

    // Coroutine to handle the delay
    private IEnumerator LoadSceneAfterDelay(string sceneName)
    {
        // Wait for 3 seconds
        yield return new WaitForSeconds(3f);

        // Load the scene
        SceneManager.LoadScene(sceneName);
    }

    public void OnQuitButtonClick()
    {
        Application.Quit();
    }

    public void OnLevelOneClick()
    {
        SceneManager.LoadScene("LEVELONE");
    }

    public void OnCreditButtonClick()
    {
        SceneManager.LoadScene("CREDITS");
    }

    public void OnMainMenuButtonClick()
    {
        SceneManager.LoadScene("MAINMENU");
    }

    public void OnClickPlaytest()
    {
        SceneManager.LoadScene("TESTLEVEL");
    }

    public void OnClickLoadScreen()
    {
        SceneManager.LoadScene("LOADSCENE");
    }

    public void OpenURL(string URLname)
    {
        Application.OpenURL(URLname);
    }

 
}
