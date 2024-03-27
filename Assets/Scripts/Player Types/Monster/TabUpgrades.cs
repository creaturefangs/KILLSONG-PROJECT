using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabUpgrades : MonoBehaviour
{
    public GameObject playerUpgrades;
    bool UpgradesPanelOpen = false;
    public AudioSource upgradesSFX;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (UpgradesPanelOpen)
            {
                UpgradesClose();
            }
            else
            {
                UpgradesOpen();
            }

        }

    }

    void UpgradesOpen()
    {
        playerUpgrades.SetActive(true);
        UpgradesPanelOpen = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        upgradesSFX.Play();
        
    }

    void UpgradesClose()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        playerUpgrades.SetActive(false);
        UpgradesPanelOpen = false;
        AudioListener.pause = false;
        upgradesSFX.Play();
    }

}
