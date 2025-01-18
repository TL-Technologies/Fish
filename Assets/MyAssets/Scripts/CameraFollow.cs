using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // The object the camera should follow
    public float smoothSpeed = 0.125f; // The smoothness factor
    public BoxCollider2D cameraBounds; // Reference to the Box Collider 2D
    private float cameraHalfWidth; // Half of the camera's orthographic width
    private float cameraHalfHeight; // Half of the camera's orthographic height

    private Vector3 offset; // The initial offset between the camera and the target

    void Start()
    {
        // Calculate the initial offset
        offset = transform.position - target.position;

        // Calculate half of the camera's orthographic size in world units
        cameraHalfHeight = Camera.main.orthographicSize;
        cameraHalfWidth = cameraHalfHeight * Camera.main.aspect;
    }

    void LateUpdate()
    {
        if (target != null)
        {
            // Calculate the desired position for the camera
            Vector3 desiredPosition = target.position + offset;

            // Get the bounds of the Box Collider
            Vector2 minBounds = cameraBounds.bounds.min;
            Vector2 maxBounds = cameraBounds.bounds.max;

            // Calculate the camera's position based on its half width and half height
            float clampedX = Mathf.Clamp(desiredPosition.x, minBounds.x + cameraHalfWidth, maxBounds.x - cameraHalfWidth);
            float clampedY = Mathf.Clamp(desiredPosition.y, minBounds.y + cameraHalfHeight, maxBounds.y - cameraHalfHeight);

            // Set the camera's position to the clamped position
            transform.position = new Vector3(clampedX, clampedY, transform.position.z);
        }
    }
}
