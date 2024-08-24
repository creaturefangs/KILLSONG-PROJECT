using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{

    public float dayDuration = 60f; // Duration of a full day in seconds
    public Transform sunTransform; // Reference to the directional light transform
    public float rotationSpeed = 10f; // Speed at which the sun rotates

    private void Update()
    {
        // Calculate the current time of day based on the elapsed time
        float currentTimeOfDay = Mathf.Repeat(Time.time / dayDuration, 1f);

        // Set the sun's rotation based on the current time of day
        float rotationAngle = currentTimeOfDay * 360f;
        sunTransform.rotation = Quaternion.Euler(new Vector3(rotationAngle, 0, 0));

        // Optionally, adjust the light intensity based on the time of day
        // You can modify this part to achieve different lighting effects
        float intensityMultiplier = Mathf.Clamp01(1.0f - Mathf.Abs(currentTimeOfDay - 0.5f) * 2f);
        sunTransform.GetComponent<Light>().intensity = intensityMultiplier;

        // Rotate the sun over time
        sunTransform.Rotate(Vector3.right * Time.deltaTime * rotationSpeed);
    }
}
