using UnityEngine;

[CreateAssetMenu(menuName = "Note", order = 2)]
public class SO_Note : ScriptableObject
{
    [Tooltip("What note/document do you want to show in this note?")]
    public Sprite noteDisplayImage;
    [Tooltip("Is this specific note locked? Will be unlocked on interaction.")]
    public bool locked = true;
}
