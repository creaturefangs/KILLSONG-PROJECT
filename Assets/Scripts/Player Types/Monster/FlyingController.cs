using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingController : MonoBehaviour

{
    // Serialized fields for easy adjustment in the Inspector
    [SerializeField] private float forwardFlightSpeed = 10f;
    [SerializeField] private float boostSpeed = 20f;
    [SerializeField] private float turnSpeed = 120f;
    [SerializeField] private float tiltAngle = 30f;
    [SerializeField] private float tiltSpeed = 5f;
    [SerializeField] private float takeoffDelay = 0.3f;
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float zoomSpeed = 2f;
    [SerializeField] private float verticalMovementSpeed = 15f;
    [SerializeField] private Transform cameraTransform;

    // Private fields for tracking flight status and input timing
    private bool isFlying = false;
    private float lastSpacePressTime;
    private bool spacePressedOnce = false;
    private Rigidbody rb;
    private float currentZoom = 5f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the game window
    }

    void Update()
    {
        HandleInput();
        HandleCamera();
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

        Vector3 movement = new Vector3(horizontal, 0f, vertical).normalized * forwardFlightSpeed * Time.deltaTime;
        Vector3 direction = cameraTransform.forward * vertical + cameraTransform.right * horizontal;
        direction.y = 0f; // Ignore upward movement
        direction.Normalize();

        rb.MovePosition(transform.position + direction * forwardFlightSpeed * Time.deltaTime);

        // Rotate the player to face the direction of movement
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, Time.deltaTime * tiltSpeed));
        }
    }

    private void Fly()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Always move forward when flying
        float speed = Input.GetKey(KeyCode.LeftShift) ? boostSpeed : forwardFlightSpeed;
        Vector3 forwardMovement = transform.forward * speed * Time.deltaTime;

        // Adjust left/right movement based on turning
        if (horizontal != 0)
        {
            transform.Rotate(0f, horizontal * turnSpeed * Time.deltaTime, 0f);
        }

        // Adjust upward/downward movement
        Vector3 verticalMovement = Vector3.zero;
        if (vertical > 0)
        {
            verticalMovement = transform.up * verticalMovementSpeed * Time.deltaTime;
        }
        else if (vertical < 0)
        {
            verticalMovement = transform.up * verticalMovementSpeed * vertical * Time.deltaTime;
        }

        rb.MovePosition(transform.position + forwardMovement + verticalMovement);
    }

    private void HandleCamera()
    {
        // Mouse movement to control camera view
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        Vector3 cameraRotation = cameraTransform.localEulerAngles;
        cameraRotation.x -= mouseY;
        cameraRotation.y += mouseX;
        cameraTransform.localEulerAngles = cameraRotation;

        // Zoom in/out with scroll wheel
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        currentZoom -= scroll * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, 2f, 10f);

        cameraTransform.localPosition = new Vector3(0f, 0f, -currentZoom);
    }



    private void Tilt()
    {
        float tiltInput = Input.GetAxis("Horizontal");
        float targetTilt = tiltInput * tiltAngle;

        Quaternion targetRotation = Quaternion.Euler(0f, transform.eulerAngles.y, -targetTilt);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, tiltSpeed * Time.deltaTime);
    }


}

