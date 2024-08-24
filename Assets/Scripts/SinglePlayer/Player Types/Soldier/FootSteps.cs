using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSteps : MonoBehaviour
{
    public GameObject footstep;
    private AudioSource footstepSound;
    private StaminaController staminaScript;
    private CharacterController characterController;
    private bool sprinting = false;
    private bool winded = false;
    private bool jumping = false;

    // Start is called before the first frame update
    void Start()
    {
        footstep.SetActive(false);
        footstepSound = footstep.GetComponent<AudioSource>();
        staminaScript = GetComponent<StaminaController>();
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //Changes the speed and volume of the footsteps sound based on player's movement speed.
        sprinting = staminaScript.weAreSprinting;
        winded = !staminaScript.canSprint;
        jumping = !characterController.isGrounded;

        if (sprinting) { footstepSound.pitch = 2; footstepSound.volume = 1; }
        else if (winded) { footstepSound.pitch = 0.5f; footstepSound.volume = 0.75f; }
        else { footstepSound.pitch = 1; footstepSound.volume = 0.5f; }

        if ((Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d")) && !jumping) { footstep.SetActive(true); }
        else { footstep.SetActive(false); }
    }
}
