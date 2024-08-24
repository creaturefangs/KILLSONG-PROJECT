using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractableController : MonoBehaviour
{
    [SerializeField] private float interactionDelay;
    public CollisionBasedInteractableObject _currentInteractableObject;
}
