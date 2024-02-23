using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Text.RegularExpressions;

public class NoteSystem : MonoBehaviour
{
    private InteractSystem interactorscr;

    private GameObject noteUI; // The DISPLAYED note not the whole UI group.
    public GameObject noteObj;
    public AudioSource audioSource;
    public AudioClip noteSFX;
    private TextAsset noteDoc;
    private string[] notes;

    private Color darkOverlay = Color.black;

    [HideInInspector] public bool exitingNote = false;

    void Start()
    {
        interactorscr = GameObject.Find("Camera").GetComponent<InteractSystem>();
        noteDoc = Resources.Load<TextAsset>("Misc/LoreNotes");
        noteUI = gameObject.transform.GetChild(0).gameObject;
        GetNoteList();
        darkOverlay.a = 0.25f;
    }

    private void Update()
    {
        if (noteUI.activeSelf && Input.GetKeyDown(KeyCode.E)) { ExitNote(); }
    }

    public void PickUpNote()
    {
        gameObject.GetComponent<Image>().color = darkOverlay;
        noteObj = interactorscr.interactObj; // Gets the object for the physical note by having the Interactor script call this on interat.
        noteUI.SetActive(true);

        // Sets note text to first note in the array.
        noteUI.transform.GetChild(0).GetComponent<TMP_Text>().text = notes[noteObj.GetComponent<InteractEvent>().noteNum - 1];

        // Deactivate the note object
        noteObj.SetActive(false);

        // Set the cursor to visible
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ExitNote()
    {
        gameObject.GetComponent<Image>().color = Color.clear;
        exitingNote = true;

        // Hide the note display panel
        noteUI.SetActive(false);

        // hide the cursor during gameplay
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Activate the note object
        noteObj.SetActive(true);

        // play sound effect
        audioSource.PlayOneShot(noteSFX);

        exitingNote = false;
    }

    void GetNoteList()
    {
        var text = noteDoc.text;
        var contents = Regex.Split(text, @"\[\d+]"); // Splits the string into an array of strings, splitting on [!] (the "new note" indicator).
        contents = contents.Skip(1).ToArray(); // Removes the random empty object that appears at the start of the array.
        for (int i = 0; i < (contents.Length); i++) { contents[i] = contents[i].Substring(2); } // Gets rid of the leftover newline at the start of notes.
        notes = contents;
    }
}
