using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCombatController : MonoBehaviour
{
    public GameObject combatCollider; // Reference to the combat collider GameObject
    private bool colliderActive = false; // Flag to track if the collider is active

    public GameObject ventGrate;

    void Update()
    {
        // Check if the left mouse button is pressed
        if (Input.GetMouseButtonDown(0))
        {
            // Toggle collider activation
            colliderActive = !colliderActive;

            // Activate or deactivate the collider accordingly
            combatCollider.SetActive(colliderActive);

            if (colliderActive)
            {
                CheckCollision();
            }
        }
    }

    void CheckCollision()
    {
        // Check for collision with another collider
        Collider[] colliders = Physics.OverlapSphere(combatCollider.transform.position, combatCollider.GetComponent<SphereCollider>().radius);
        foreach (Collider collider in colliders)
        {
            // Check if the collided object meets certain conditions
            if (collider.CompareTag("Enemy"))
            {
                // Trigger event for hitting an enemy
                // For example:
                collider.GetComponent<Enemy>().TakeDamage(10); // Assuming there's a method in the Enemy script to take damage
            }
            else if (collider.CompareTag("VentGrate"))
            {
                // Trigger event for hitting an enemy
                // For example:
                //collider.GetComponent<Enemy>().TakeDamage(10); // Assuming there's a method in the Enemy script to take damage
                ventGrate.SetActive(false);
            }
        }
    }
}
