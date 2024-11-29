using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JunkochanCamControl : MonoBehaviour
{
    private GameObject JKC; // Junkochan

    [Header("Target Settings")]
    public Transform target; // The character to follow

    [Header("Offset Settings")]
    public Vector3 offset = new Vector3(0, 5, -10); // Default camera offset
    public float minZoomDistance = 2f; // Minimum zoom distance
    public float maxZoomDistance = 20f; // Maximum zoom distance
    public float zoomSpeed = 5f; // Speed of zooming with the scroll wheel

    [Header("Smooth Settings")]
    public float followSpeed = 5f; // Speed of following

    [Header("Mouse Control Settings")]
    public float mouseSensitivity = 2f; // Sensitivity of mouse movement
    public float verticalRotationLimit = 80f; // Limit for up/down rotation

    private float currentYaw = 0f; // Current horizontal rotation
    private float currentPitch = 0f; // Current vertical rotation

    private float currentZoom = 10f; // Current zoom level (distance from target)

    [Header("View Target Settings")]
    public Transform lookAtPoint; // The specific point to focus on (e.g., character's torso)

    private void Start()
    {
        JKC = GameObject.Find("JunkoChan"); // Find the character object

        if (lookAtPoint == null && target != null)
        {
            lookAtPoint = target;
        }

        // Initialize camera rotation and zoom based on current offset
        Vector3 initialDirection = (transform.position - target.position).normalized;
        currentYaw = Mathf.Atan2(initialDirection.x, initialDirection.z) * Mathf.Rad2Deg;
        currentPitch = Mathf.Asin(initialDirection.y) * Mathf.Rad2Deg;

        currentZoom = offset.magnitude; // Use the initial offset magnitude as the starting zoom level
    }

    private void LateUpdate()
    {
        if (target == null || lookAtPoint == null)
        {
            Debug.LogWarning("Target or LookAtPoint is not assigned in JunkochanCamControl.");
            return;
        }

        HandleMouseInput();
        HandleScrollInput();

        // Calculate the new camera position based on rotation and zoom
        Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0f);
        Vector3 rotatedOffset = rotation * Vector3.back * currentZoom;

        Vector3 desiredPosition = target.position + rotatedOffset;

        // Smoothly move the camera to the desired position
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        // Make the camera look at the specified point
        transform.LookAt(lookAtPoint.position);
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButton(1))
        { // Right mouse button is held
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            // Update yaw and pitch based on mouse movement
            currentYaw += mouseX * mouseSensitivity;
            currentPitch -= mouseY * mouseSensitivity;

            // Clamp the pitch to prevent flipping
            currentPitch = Mathf.Clamp(currentPitch, -verticalRotationLimit, verticalRotationLimit);
        }
    }

    private void HandleScrollInput()
    {
        // Get scroll wheel input
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        // Adjust the zoom level based on scroll input
        currentZoom -= scroll * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoomDistance, maxZoomDistance);
    }
}
