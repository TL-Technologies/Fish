using UnityEngine;
using Random = UnityEngine.Random;

public class AiPlayerController : MonoBehaviour
{
    public Animator anim;
    public Rigidbody2D rb;
    public Transform body, bodyParent;
    private AIPlayerTargetManager AIPlayerTargetManager;
    [HideInInspector] public Transform target;
    private Vector2 targetPosition;
    public GameObject bonePrefab;
    public Fish fish;

    private void Start()
    {
        AIPlayerTargetManager = FindObjectOfType<AIPlayerTargetManager>();
        FindTarget();
    }

    public void FindTarget()
    {
        int randomIndex = Random.Range(0, AIPlayerTargetManager.targets.Count);

        Transform randonTarget = AIPlayerTargetManager.targets[randomIndex];
        GotoTarget(randonTarget);

        AIPlayerTargetManager.targets.RemoveAt(randomIndex);

        if (AIPlayerTargetManager.targets.Count == 0)
            AIPlayerTargetManager.ResetTargets();
    }

    private void GotoTarget(Transform randonTarget)
    {
        target = randonTarget;
    }

    void Update()
    {
        Vector2 direction;

        if (target != null)
        {
            targetPosition = new Vector2(target.position.x, target.position.y);
            Vector2 currentPosition = rb.position;
            direction = (targetPosition - currentPosition).normalized;
            rb.velocity = direction * fish.speed;

            CheckTargetReached();

            // Calculate the angle between the current forward direction and the movement direction.
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Calculate the Y-axis rotation based on the movement direction.
            float yRotation = (direction.x < 0) ? 180f : 0f; // 0 degrees for right, 180 degrees for left.

            //float anglee = Vector2.SignedAngle(Vector2.right, direction);
            bodyParent.eulerAngles = new Vector3(0, 0, angle);
            body.localEulerAngles = new Vector3(yRotation, 0, 0);
        }
    }

    private void CheckTargetReached()
    {
        // Check if the player is close enough to the target to stop.
        float distanceToTarget = Vector2.Distance(transform.position, targetPosition);
        if (distanceToTarget <= .5f)
        {
            FindTarget(); 
        }
    }
}
