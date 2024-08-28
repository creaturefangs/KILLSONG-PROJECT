using UnityEngine;
using UnityEngine.Serialization;

public class CPlayerMovement : MonoBehaviour
{
    public float walkSpeed = 5f; // Movement speed
    public float sprintSpeed = 10f; // Sprinting speed
    [SerializeField] private float jumpForce = 5f; // Jump force
    [SerializeField] private float mouseSensitivity = 2f; // Mouse sensitivity
    [SerializeField] private float zoomSensitivity = 15f; // Zoom sensitivity
    [SerializeField] private float minZoom = 20f; // Minimum camera zoom (FOV)
    [SerializeField] private float maxZoom = 60f; // Maximum camera zoom (FOV)
    [SerializeField] private int maxJumps = 2; // Maximum number of jumps (e.g., 2 for double jump)
    [SerializeField] private LayerMask groundLayer; // Layer mask to identify ground

    //for 360 degree cam rotation

    private Rigidbody _rb;
    private Camera _playerCamera;
    private Vector3 _movement;
    private float _mouseX, _mouseY;
    public bool isSprinting = false;
    private int _jumpCount = 0;
    private float _distanceToGround;

    private bool _canRotate = true;
    private bool _canMove = true;

    #region Properties
    public bool CanRotate
    {
        get => _canRotate;
        set => _canRotate = value;
    }

    public bool CanMove
    {
        get => _canMove;
        set => _canMove = value;
    }
    #endregion
    
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _playerCamera = GetComponentInChildren<Camera>(); // Assuming the camera is a child of the player
        Cursor.lockState = CursorLockMode.Locked; // Lock cursor to center of screen

        _distanceToGround = GetComponent<Collider>().bounds.extents.y; // Calculate distance from center to the bottom of the collider
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
                float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;
                _movement = new Vector3(horizontalInput, 0f, verticalInput).normalized * currentSpeed;
            }
            else
            {
                _movement = Vector3.zero; // No movement input, reset movement vector
            }
        }
        #endregion
        #region Mouse Rotation
        if (_canRotate)
        {
            // Mouse look input for camera rotation around the player
            _mouseX += Input.GetAxis("Mouse X") * mouseSensitivity;
            _mouseY -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            _mouseY = Mathf.Clamp(_mouseY, -30f, 60f); // Limit vertical rotation to prevent the camera from flipping
            
            // Rotate the camera around the player
            _playerCamera.transform.localRotation = Quaternion.Euler(_mouseY, 0f, 0f);
            transform.rotation = Quaternion.Euler(0f, _mouseX, 0f);
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
            PerformMovement(_movement);
        }
    }

    void PerformMovement(Vector3 move)
    {
        _rb.MovePosition(_rb.position + transform.TransformDirection(move) * Time.fixedDeltaTime);
    }

    void Jump()
    {
        if (IsGrounded() || _jumpCount < maxJumps)
        {
            _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            _jumpCount++;
        }
    }

    bool IsGrounded()
    {
        // Check if the player is on the ground using a sphere cast
        return Physics.CheckSphere(transform.position + Vector3.down * _distanceToGround, 0.3f, groundLayer);
    }

    void ZoomCamera(float scrollInput)
    {
        float newFOV = _playerCamera.fieldOfView - scrollInput * zoomSensitivity;
        newFOV = Mathf.Clamp(newFOV, minZoom, maxZoom);
        //playerCamera.fieldOfView = Mathf.Clamp(newFOV, minZoom, maxZoom);

        //for smoother zooming - Alden
        _playerCamera.fieldOfView = Mathf.Lerp(_playerCamera.fieldOfView, newFOV, Time.deltaTime * zoomSensitivity);
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