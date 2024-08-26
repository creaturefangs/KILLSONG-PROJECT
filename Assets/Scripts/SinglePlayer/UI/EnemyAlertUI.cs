using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UI;

public class EnemyAlertUI : MonoBehaviour
{
    public enum AlertState
    {
        Neutral,
        Suspicious,
        Alerted
    }

    public AlertState currentState = AlertState.Neutral;
    public Image alertImage;

    private Color neutralColor = new Color(0.9f, 0.9f, 0.9f);  // Light white-grey
    private Color suspiciousColor = Color.yellow;
    private Color alertedColor = Color.red;

    void Start()
    {
        UpdateAlertColor();
    }

    void OnValidate()
    {
        UpdateAlertColor();
    }

    public void ChangeState(AlertState newState)
    {
        currentState = newState;
        UpdateAlertColor();
    }

    void UpdateAlertColor()
    {
        if (alertImage == null) return;

        switch (currentState)
        {
            case AlertState.Neutral:
                alertImage.color = neutralColor;
                break;
            case AlertState.Suspicious:
                alertImage.color = suspiciousColor;
                break;
            case AlertState.Alerted:
                alertImage.color = alertedColor;
                break;
        }
    }
}



