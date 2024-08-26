using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct STRInteractables 
{
    public string interactableName;
    public Image uiImage;
    public Vector2 uiImageSize;
    
    public enum EInteractionType
    {
        Trigger,
        TriggerWithExit,
        BasicInteraction
    }
}
