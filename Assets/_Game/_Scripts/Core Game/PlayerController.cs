using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Joystick joystick;
    public Rigidbody2D rb;
    public BoxCollider2D boundsCollider;
    private Vector2 minBounds, maxBounds;
    public Vector2 boundOffset;
    public Transform body, bodyParent, lockBody;
    private Vector2 direction;
    public Fish fish;
    private Quaternion initialRotation;
    
    public bool movingLeft = false;

    private void Start()
    {
        // Calculate the min and max bounds based on the Box Collider.
        CalculateBounds();
        initialRotation = lockBody.rotation;
    }

    private void LateUpdate()
    {
        lockBody.rotation = initialRotation;
    }

    void Update()
    {
        if (joystick != null)
        {
            direction = joystick.Direction;

            rb.velocity = transform.right * fish.speed;

            if (direction != Vector2.zero)
            {
                // Calculate the angle between the current forward direction and the movement direction.
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                // Calculate the Y-axis rotation based on the movement direction.
                float yRotation = (direction.x < 0) ? 180f : 0f; // 0 degrees for right, 180 degrees for left.

                //float anglee = Vector2.SignedAngle(Vector2.right, direction);
                transform.eulerAngles = new Vector3(0, 0, angle);
                body.localEulerAngles = new Vector3(yRotation, 0, 0);

                if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                {
                    if (direction.x > 0)
                    {
                        //Debug.Log("Moving Right");
                        movingLeft = false;
                    }

                    else
                    {
                        //Debug.Log("Moving Left");
                        movingLeft = true;
                    }

                }
                else
                {
                    if (direction.y > 0)
                    {

                        //Debug.Log("Moving Up");

                    }
                    else
                    {
                        //Debug.Log("Moving Down");

                    }
                }
            }

            // Ensure the player stays within bounds.
            ClampToBounds();
        }
    }

    private void CalculateBounds()
    {
        if (boundsCollider != null)
        {
            // Calculate the bounds based on the Box Collider.
            minBounds = boundsCollider.bounds.min;
            maxBounds = boundsCollider.bounds.max;
        }
    }

    private void ClampToBounds()
    {
        // Get the current position.
        Vector2 currentPosition = transform.position;

        // Clamp the position to stay within bounds.
        currentPosition.x = Mathf.Clamp(currentPosition.x, minBounds.x + boundOffset.x, maxBounds.x + -boundOffset.x);
        currentPosition.y = Mathf.Clamp(currentPosition.y, minBounds.y + boundOffset.y, maxBounds.y + -boundOffset.y);

        // Set the clamped position.
        transform.position = currentPosition;
    }
}