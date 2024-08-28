using UnityEngine;
using UnityEngine.UI;
public class CBIO_Note : CollisionBasedInteractableObject
{
    [SerializeField] private SO_Note note;
    [SerializeField] private Image noteImage;
    [SerializeField] private GameObject noteCanvas;
    [SerializeField] private GameObject fileOpenButton;
    [SerializeField] private KeyCode escapeNoteKey;
    [SerializeField] private AudioClip noteSFX;
    
    private CPlayerMovement _playerMovement;
    private void Start()
    {
        InitNote();
        _playerMovement = FindObjectOfType<CPlayerMovement>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (noteCanvas.activeSelf)
            {
                ToggleNoteUI(false);
                FindObjectOfType<CPlayerMovement>().ToggleRotation();
            }
        }

        if (Input.GetKeyDown(escapeNoteKey))
        {
            if(noteCanvas.activeSelf)
                noteCanvas.SetActive(false);
        }
    }

    public void HandleNoteActivationButton()
    {
        noteImage.gameObject.SetActive(true);
        note.locked = false;
        Destroy(fileOpenButton);
    }

    private void InitNote()
    {
        noteImage.sprite = note.noteDisplayImage;
    }
    
    public void ToggleNoteUI(bool enable)
    {
        noteCanvas.SetActive(enable);
        
        if (enable)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            _playerMovement.CanRotate = false;

            EnvironmentalSoundController.Instance.PlaySound2D(noteSFX, 1.0f);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _playerMovement.CanRotate = true;
        }
    }
}
