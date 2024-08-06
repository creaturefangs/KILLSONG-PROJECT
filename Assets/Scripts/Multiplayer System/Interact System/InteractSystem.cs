using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractSystem : MonoBehaviour
{
    public LayerMask interactLayer;
    InteractEvent interactable;
    UnityEvent onInteract;
    public GameObject eInteractUI;
    public GameObject interactObj;
    private string interactType;
    private AudioSource interactSFX;
    public bool pickupCrowbar;

    private NoteSystem noteInteraction;
    private PlayerHealth health;
    private Objectives objectives;
  

    // Start is called before the first frame update
    void Start()
    {
        noteInteraction = GameObject.Find("NotesUI").GetComponent<NoteSystem>();
        health = GameObject.Find("PlayerController").GetComponent<PlayerHealth>();
        objectives = GameObject.Find("TaskUI").GetComponent<Objectives>();
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 7, interactLayer))
        {
            onInteract = hit.collider.GetComponent<InteractEvent>().onInteract;
            interactType = hit.collider.GetComponent<InteractEvent>().interactType;
            eInteractUI.SetActive(true);

            // Add debug logging to check the hit object
            Debug.Log("Hit object: " + hit.collider.gameObject.name);

          

            if (Input.GetKeyDown(KeyCode.E) && !noteInteraction.exitingNote)
            {
                interactObj = hit.collider.gameObject;
                if (interactType == "note")
                {
                    interactSFX = GameObject.Find("NoteSFX").GetComponent<AudioSource>();
                    noteInteraction.PickUpNote();
                }
                else if (interactType == "lantern") { interactSFX = GameObject.Find("LanternSFX").GetComponent<AudioSource>(); }
                else if (interactType == "crowbar")
                {
                    interactSFX = GameObject.Find("interactSFX").GetComponent<AudioSource>();
                    pickupCrowbar = true;
                }
                else if (interactType == "knife") { interactSFX = GameObject.Find("KnifeSFX").GetComponent<AudioSource>(); }
                else if (interactType == "door") { interactSFX = GameObject.Find("DoorSFX").GetComponent<AudioSource>(); }
                else if (interactType == "pills")
                {
                    interactSFX = GameObject.Find("pillsSFX").GetComponent<AudioSource>();
                    health.RestoreHealth(50);

                }
                else if (interactType == "firstaid")
                {
                    interactSFX = GameObject.Find("pillsSFX").GetComponent<AudioSource>();
                    health.RestoreHealth(100);

                }
                else if (interactType == "key")
                {
                    interactSFX = GameObject.Find("keySFX").GetComponent<AudioSource>();
                }
                else if (interactType == "radio")
                {
                    interactSFX = GameObject.Find("radioSFX").GetComponent<AudioSource>();
                }
                else if (interactType == "powerbox")
                {
                    //interactSFX = GameObject.Find("powerboxSFx").GetComponent<AudioSource>();

                }
                else if (interactType == "powerswitch")
                {
                    //interactSFX = GameObject.Find("powerswitchSFX").GetComponent<AudioSource>();
                    //turns power on in level one 
                    //powerOn.powerOn = true;
                }
                else if (interactType == "lightgenerator")
                {
                    interactSFX = GameObject.Find("lightgeneratorSFX").GetComponent<AudioSource>();
                }
                else if (interactType == "fence")
                {
                    if (pickupCrowbar == true)
                    {

                        Destroy(hit.collider.gameObject);

                        interactSFX = GameObject.Find("interactSFX").GetComponent<AudioSource>();
                    }
                }
                if (interactType != "radio" && interactType != "note" && interactType != "powerbox" && interactType != "powerswitch" && interactType != "lightgenerator" && interactType != "door" && interactType != "fence") { Destroy(hit.collider.gameObject); }
                if (interactSFX != null) { interactSFX.Play(); }

                if (interactObj.tag == "Objective") { objectives.UpdateObjective(interactObj.name); }
                else if (interactObj.tag == "SpecialObjective")
                {
                    if (interactObj.name == "Key Variant") { objectives.UpdateObjective("key to the"); }
                    else if (interactObj.name == "Main Knob") { objectives.UpdateObjective("radio station"); }
                    else if (interactObj.name == "Radio") { objectives.UpdateObjective("radio broadcast"); }
                    else if (interactObj.name == "Crowbar") { objectives.UpdateObjective("break the fence"); }
                }

                onInteract.Invoke();
                Debug.Log("Player interacted with: " + interactObj.name);
            }
        }
        else
        {
            eInteractUI.SetActive(false);

            if (interactObj != null)
            {
                
            }
        }
    }

    void EquipItem(GameObject item)
    {
        // Figure out how to add to the items array here???
    }
}
