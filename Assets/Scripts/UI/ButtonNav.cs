using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonNav : MonoBehaviour
{
   
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
