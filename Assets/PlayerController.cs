
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float acceleration = 15f;
    public float deceleration = 20f;
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;

    [Header("Mouse Look")]
    public float mouseSensitivity = 2f;
    public float lookSmoothness = 12f;
    public Transform playerCamera;

    private CharacterController controller;
    private float verticalVelocity;
    private float cameraRotationX;

    private Vector3 currentHorizontalVelocity;
    private float targetCameraRotationX;
    private float currentMouseX;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (playerCamera != null)
        {
            cameraRotationX = playerCamera.localEulerAngles.x;

            if (cameraRotationX > 180f)
                cameraRotationX -= 360f;

            targetCameraRotationX = cameraRotationX;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (controller == null || playerCamera == null)
            return;

        MovePlayer();
        LookAround();
    }

    void MovePlayer()
{
    float x = Input.GetAxisRaw("Horizontal");
    float z = Input.GetAxisRaw("Vertical");

    Vector3 input = new Vector3(x, 0f, z).normalized;
    Vector3 targetVelocity =
        (transform.right * input.x + transform.forward * input.z) * moveSpeed;

    float smooth = (input.magnitude > 0) ? acceleration : deceleration;

    currentHorizontalVelocity = Vector3.Lerp(
        currentHorizontalVelocity,
        targetVelocity,
        smooth * Time.deltaTime
    );

    // Ground stick
    if (controller.isGrounded)
    {
        if (verticalVelocity < 0)
            verticalVelocity = -3f;

        if (Input.GetButtonDown("Jump"))
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    verticalVelocity += gravity * Time.deltaTime;

    Vector3 move = currentHorizontalVelocity;
    move.y = verticalVelocity;

    controller.Move(move * Time.deltaTime);
}

    void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        targetCameraRotationX -= mouseY;
        targetCameraRotationX = Mathf.Clamp(targetCameraRotationX, -90f, 90f);

        cameraRotationX = Mathf.Lerp(
            cameraRotationX,
            targetCameraRotationX,
            1f - Mathf.Exp(-lookSmoothness * Time.deltaTime)
        );

        playerCamera.localRotation =
            Quaternion.Euler(cameraRotationX, 0f, 0f);
    }
}
