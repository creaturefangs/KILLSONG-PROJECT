using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class PauseManager : MonoBehaviour
{
    public bool gameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject playerUI;
    public AudioSource pauseSFX;
    private CPlayerMovement _playerMovement;
    private List<(Canvas, bool)> _otherCanvases;

    void Start()
    {
        _playerMovement = FindObjectOfType<CPlayerMovement>();
        _otherCanvases = new List<(Canvas, bool)>();
        FindAllUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        //TODO: Handle states of all other UI when resuming
        //Restore the active state of canvases other than pause menu when resuming
        //SetOtherCanvasesToOriginalStates();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
        playerUI.SetActive(true);
        gameIsPaused = false;
        AudioListener.pause = false;
        pauseSFX.Play();

        if (_playerMovement.CanMove != true)
        {
            _playerMovement.CanMove = true;
        }

        if (_playerMovement.CanRotate != true)
        {
            _playerMovement.CanRotate = true;
        }
    }

    public void Pause()
    {
        
        //TODO: Handle states of all other UI when pausing
        //Store the active state of canvases other than pause menu when pausing
        //StoreCurrentCanvasStates();
        // Hide all other canvases when pausing
        //SetOtherCanvasesActive(false);

        Time.timeScale = 0f;
        pauseMenuUI.SetActive(true);
        playerUI.SetActive(false);
        gameIsPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pauseSFX.Play();
        AudioListener.pause = true;
        
        if (_playerMovement.CanMove != true)
        {
            _playerMovement.CanMove = false;
        }

        if (_playerMovement.CanRotate != true)
        {
            _playerMovement.CanRotate = false;
        }
    }

    private void FindAllUI()
    {
        Canvas[] allCanvases = FindObjectsOfType<Canvas>();

        foreach (Canvas canvas in allCanvases)
        {
            //exclude the pause menu canvas
            if (canvas.gameObject != pauseMenuUI)
            {
                //store each canvas with its initial active state
                _otherCanvases.Add((canvas, canvas.gameObject.activeSelf));
            }
        }
    }

    private void StoreCurrentCanvasStates()
    {
        // Update the list to store the current active states
        for (int i = 0; i < _otherCanvases.Count; i++)
        {
            Canvas canvas = _otherCanvases[i].Item1;
            _otherCanvases[i] = (canvas, canvas.gameObject.activeSelf);
        }
    }

    private void SetOtherCanvasesActive(bool isActive)
    {
        //enable/disable all canvases except the pause menu
        foreach (var (canvas, _) in _otherCanvases)
        {
            canvas.gameObject.SetActive(isActive);
        }
    }

    private void SetOtherCanvasesToOriginalStates()
    {
        //restore the original active state of all canvases
        foreach (var (canvas, wasActive) in _otherCanvases)
        {
            canvas.gameObject.SetActive(wasActive);
        }
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        Debug.Log("Loading Menu...");
        SceneManager.LoadScene("MAINMENU");
        AudioListener.pause = false;
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}