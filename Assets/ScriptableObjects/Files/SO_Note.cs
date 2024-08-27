using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Note", order = 2)]
public class SO_Note : ScriptableObject
{
    [Tooltip("What note/document do you want to show in this note?")]
    public Sprite noteDisplayImage;
    public bool locked = true;
}
