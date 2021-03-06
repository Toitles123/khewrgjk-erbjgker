using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform playerCamera = null;
    [SerializeField] float mouseSensitivity = 1.0f;
    [SerializeField] float walkSpeed = 5.0f;
    [SerializeField] float gravity = -20.0f;
    [SerializeField] [Range(0.0f, 1f)] float moveSmoothTime = 0.3f;
    [SerializeField] [Range(0.0f, 1f)] float mouseSmoothTime = 0.03f;
    [SerializeField] [Range(30.0f, 150f)] float walkFOV = 60f;
    [SerializeField] [Range(60.0f, 180f)] float runFOV = 90f;
    [SerializeField] [Range(0.0f, 1f)] float lerpSpeed = 0.5f;
    [SerializeField] float initialJumpSpeed = 9.0f;
    [SerializeField] float runSpeed = 10.0f;
    public bool lockCursor = true;

    float cameraPitch = 0.0f;
    float velocityY = 0.0f;
    CharacterController controller = null;

    Vector2 currentDir = Vector2.zero;
    Vector2 currentDirVelocity = Vector2.zero;

    Vector2 currentMouseDelta = Vector2.zero;
    Vector2 currentMouseDeltaVelocity = Vector2.zero;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }


    void FixedUpdate()
    {
        if (lockCursor && ProceduralLevel.mapGenerated)
        {
            UpdateMouseLook();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        if (ProceduralLevel.mapGenerated) UpdateMovement();
    }

    void UpdateMouseLook()
    {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        foreach (Camera cam in FindObjectsOfType<Camera>())
        {
            float currentFOV = cam.fieldOfView;
            if (Input.GetKey(KeyCode.LeftShift)) cam.fieldOfView = Mathf.Lerp(currentFOV, runFOV, lerpSpeed);
            else cam.fieldOfView = Mathf.Lerp(currentFOV, walkFOV, lerpSpeed);
        }


        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);

        cameraPitch -= currentMouseDelta.y * mouseSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 90.0f);

        playerCamera.localEulerAngles = Vector3.right * cameraPitch;
        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);
    }

    void UpdateMovement()
    {
        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();

        Vector3 velocity;

        if (controller.isGrounded)
        {
            currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);
            velocity = (transform.forward * currentDir.y + transform.right * currentDir.x);
            if (Input.GetKey(KeyCode.LeftShift)) velocity = velocity * runSpeed + Vector3.up * velocityY;
            else velocity = velocity * walkSpeed + Vector3.up * velocityY;
            velocityY = 0.0f;
            if (Input.GetButton("Jump"))
            {
                velocityY = initialJumpSpeed;
            }
        }
        else
        {
            currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, 0.6f);
            velocityY += gravity * Time.deltaTime;
            velocity = (transform.forward * currentDir.y + transform.right * currentDir.x);
            if (Input.GetKey(KeyCode.LeftShift)) velocity = velocity * runSpeed + Vector3.up * velocityY;
            else velocity = velocity * walkSpeed + Vector3.up * velocityY;
        }

        controller.Move(velocity * Time.deltaTime);
    }
}