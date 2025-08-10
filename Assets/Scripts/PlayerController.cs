using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 12f;

    [Header("Jump / Gravity")]
    public float jumpForce = 5f;
    public float gravity = -20f;
    [Tooltip("Maximum downward speed (terminal velocity)")]
    public float maxFallSpeed = -40f;

    [Header("Ground Check (optional)")]
    public Transform groundCheck;         // optional child under feet
    public float groundCheckRadius = 0.25f;
    public LayerMask groundMask = ~0;

    [Header("Fall / Respawn")]
    [SerializeField] float fallY = -5f;   // if player goes below this -> respawn

    private CharacterController controller;
    private Vector3 velocity;             // used only for vertical motion
    private Transform cam;
    private bool isRespawning = false;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        cam = Camera.main ? Camera.main.transform : null;

        // small lift in case we start intersecting the ground
        controller.enabled = false;
        transform.position += Vector3.up * 0.1f;
        controller.enabled = true;
    }

    void Update()
    {
        float dt = Time.deltaTime;

        // In case camera was recreated
        if (!cam && Camera.main) cam = Camera.main.transform;

        // --- movement input ---
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // camera-relative movement
        Vector3 forward = cam ? cam.forward : Vector3.forward;
        Vector3 right = cam ? cam.right : Vector3.right;
        forward.y = 0f; right.y = 0f; forward.Normalize(); right.Normalize();

        Vector3 input = new Vector3(x, 0f, z);
        input = Vector3.ClampMagnitude(input, 1f);
        Vector3 moveDir = forward * input.z + right * input.x;

        // face movement direction
        if (moveDir.sqrMagnitude > 0.001f)
        {
            Quaternion target = Quaternion.LookRotation(moveDir, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, rotationSpeed * dt);
        }

        // horizontal move
        controller.Move(moveDir * moveSpeed * dt);

        // ground check
        bool grounded = controller.isGrounded;
        if (groundCheck)
        {
            grounded = Physics.CheckSphere(
                groundCheck.position,
                groundCheckRadius,
                groundMask,
                QueryTriggerInteraction.Ignore
            );
        }

        // jump / stick to ground
        if (grounded && velocity.y < 0f) velocity.y = -2f;
        if (grounded && Input.GetButtonDown("Jump"))
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);

        // gravity (cap fall speed)
        velocity.y += gravity * dt;
        if (velocity.y < maxFallSpeed) velocity.y = maxFallSpeed;

        // vertical move
        controller.Move(velocity * dt);

        // fall -> respawn to GameManager.startPoint
        if (!isRespawning && transform.position.y < fallY)
        {
            isRespawning = true;
            if (GameManager.Instance != null) GameManager.Instance.RespawnPlayer();
            Invoke(nameof(ClearRespawnFlag), 0.05f); // avoid retriggering every frame
        }

        // manual respawn for testing
        if (Input.GetKeyDown(KeyCode.R) && GameManager.Instance != null)
            GameManager.Instance.RespawnPlayer();
    }

    void ClearRespawnFlag() => isRespawning = false;

    // Called by GameManager after teleport to clear gravity
    public void ResetMotion()
    {
        velocity = Vector3.zero;
    }

    void OnDrawGizmosSelected()
    {
        if (!groundCheck) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
