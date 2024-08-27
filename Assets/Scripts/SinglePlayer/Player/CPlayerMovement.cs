using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPlayerMovement : MonoBehaviour
{
    public enum MovementTypes
    {
        Slow,
        Normal
    }
    public MovementTypes movementType;
    public float speed = 5f; // Movement speed
   
    [SerializeField] private float sprintSpeed = 10f; // Sprinting speed
    [SerializeField] private float jumpForce = 5f; // Jump force
    [SerializeField] private float mouseSensitivity = 2f; // Mouse sensitivity
    [SerializeField] private float zoomSensitivity = 15f; // Zoom sensitivity
    [SerializeField] private float minZoom = 20f; // Minimum camera zoom (FOV)
    [SerializeField] private float maxZoom = 60f; // Maximum camera zoom (FOV)
    [SerializeField] private int maxJumps = 2; // Maximum number of jumps (e.g., 2 for double jump)
    [SerializeField] private LayerMask groundLayer; // Layer mask to identify ground

    private Rigidbody rb;
    private Camera playerCamera;
    private Vector3 movement;
    private float mouseX, mouseY;
    private bool isSprinting = false;
    private int jumpCount = 0;
    private float distanceToGround;

    private bool _canRotate = true;
    private bool _canMove = true;

    #region Properties
    public bool CanRotate
    {
        get { return _canRotate; }
        set { _canRotate = value; }
    }

    public bool CanMove
    {
        get { return _canMove; }
        set { _canMove = value; }
    }
    #endregion
    
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCamera = GetComponentInChildren<Camera>(); // Assuming the camera is a child of the player
        Cursor.lockState = CursorLockMode.Locked; // Lock cursor to center of screen

        distanceToGround = GetComponent<Collider>().bounds.extents.y; // Calculate distance from center to the bottom of the collider
    }

    void Update()
    {
        #region Movement
        // Player movement input
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (_canMove)
        {
            // Check if sprint key is pressed
            isSprinting = Input.GetKey(KeyCode.LeftShift);

            // Check if there's any movement input
            if (horizontalInput != 0 || verticalInput != 0)
            {
                float currentSpeed = isSprinting ? sprintSpeed : speed;
                movement = new Vector3(horizontalInput, 0f, verticalInput).normalized * currentSpeed;
            }
            else
            {
                movement = Vector3.zero; // No movement input, reset movement vector
            }
        }
        #endregion
        #region Mouse Rotation
        if (_canRotate)
        {
            // Mouse look input for camera rotation around the player
            mouseX += Input.GetAxis("Mouse X") * mouseSensitivity;
            mouseY -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            mouseY = Mathf.Clamp(mouseY, -30f, 60f); // Limit vertical rotation to prevent the camera from flipping

            // Rotate the camera around the player
            playerCamera.transform.localRotation = Quaternion.Euler(mouseY, 0f, 0f);
            transform.rotation = Quaternion.Euler(0f, mouseX, 0f);
        }
        #endregion
        #region Jumping
        // Jump input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        #endregion
        #region Scrolling
        // Mouse zoom input
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0f)
        {
            ZoomCamera(scrollInput);
        }
        #endregion
    }

    void FixedUpdate()
    {
        if (_canMove)
        {
            // Move the player
            PerformMovement(movement);
        }
    }

    void PerformMovement(Vector3 move)
    {
        rb.MovePosition(rb.position + transform.TransformDirection(move) * Time.fixedDeltaTime);
    }

    void Jump()
    {
        if (IsGrounded() || jumpCount < maxJumps)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpCount++;
        }
    }

    bool IsGrounded()
    {
        // Check if the player is on the ground using a sphere cast
        return Physics.CheckSphere(transform.position + Vector3.down * distanceToGround, 0.3f, groundLayer);
    }

    void ZoomCamera(float scrollInput)
    {
        float newFOV = playerCamera.fieldOfView - scrollInput * zoomSensitivity;
        newFOV = Mathf.Clamp(newFOV, minZoom, maxZoom);
        //playerCamera.fieldOfView = Mathf.Clamp(newFOV, minZoom, maxZoom);

        //for smoother zooming - Alden
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, newFOV, Time.deltaTime * zoomSensitivity);
    }

    public void ToggleMovement()
    {
        _canMove = !_canMove;
    }

    public void ToggleRotation()
    {
        _canRotate = !_canRotate;
    }
}