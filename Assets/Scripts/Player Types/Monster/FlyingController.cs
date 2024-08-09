using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingController : MonoBehaviour
{
    // Serialized fields for easy adjustment in the Inspector
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float flightSpeed = 20f;
    [SerializeField] private float tiltAngle = 30f;
    [SerializeField] private float tiltSpeed = 5f;
    [SerializeField] private float takeoffDelay = 0.3f;

    // Private fields for tracking flight status and input timing
    private bool isFlying = false;
    private float lastSpacePressTime;
    private bool spacePressedOnce = false;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        // Handle double-tap space bar to toggle flight mode
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (spacePressedOnce && Time.time - lastSpacePressTime <= takeoffDelay)
            {
                isFlying = !isFlying;
                spacePressedOnce = false;
            }
            else
            {
                spacePressedOnce = true;
                lastSpacePressTime = Time.time;
            }
        }

        // Reset single space press if too much time has passed
        if (spacePressedOnce && Time.time - lastSpacePressTime > takeoffDelay)
        {
            spacePressedOnce = false;
        }

        // Handle movement and tilt
        if (isFlying)
        {
            Fly();
        }
        else
        {
            MoveOnGround();
        }

        Tilt();
    }

    private void MoveOnGround()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, 0f, vertical) * moveSpeed * Time.deltaTime;
        rb.MovePosition(transform.position + movement);
    }

    private void Fly()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, 0f, vertical) * flightSpeed * Time.deltaTime;
        rb.MovePosition(transform.position + movement);

        if (vertical > 0)
        {
            rb.velocity = new Vector3(rb.velocity.x, flightSpeed, rb.velocity.z);
        }
    }

    private void Tilt()
    {
        float tiltInput = Input.GetAxis("Horizontal");
        float targetTilt = tiltInput * tiltAngle;

        Quaternion targetRotation = Quaternion.Euler(0f, transform.eulerAngles.y, -targetTilt);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, tiltSpeed * Time.deltaTime);
    }
}


