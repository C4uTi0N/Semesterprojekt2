using System.Collections;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    //Components
    private Rigidbody rb;                                                   // Set Rigidbody component to "rb"
    private Transform player;                                               // Set Transform component to "player"
    private Camera playerCamera;                                            // Set Camera component to "playerCamera"
    private CapsuleCollider capsCollider;

    // Movement input Axis & Direction
    private float xAxisMovement;                                            // Movement on the "X" axis

    // Player Settings
    private float gravity = 9.82f;                                          // Gravity value

    [Header("Player settings", order = 1)]
    public float gravityMultiplier = 5;                                     // Gravity Multiplier
    public float movementSpeed = 10f;                                       // Movement speed
    public float vaultSpeed = 5f;                                           // Vaulting speed
    public float maxSpeed = 20;                                             // Max movement speed
    public bool cameraPanOut = false;

    //Debug bools
    [Header("Debug bools", order = 2)]
    public bool isGrounded;                                                 // Bool to check if player is on ground
    public bool canVault;                                                   // Bool to check if player can vault
    public bool isVaulting;                                                 // Bool to check if player is vaulting
    public bool isInteracting;                                              // Bool to check if player is interacting

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();                                     // Set "rb" to Rigidbody of attached Gameobject
        rb.freezeRotation = true;                                           // Disable rotation through rigidbody
        rb.useGravity = false;                                              // Disable Gravity from Rigidbody

        player = GetComponent<Transform>();                                 // Set "player" to Transform of attached gameobject
        capsCollider = GetComponent<CapsuleCollider>();

        playerCamera = FindObjectOfType<Camera>();                          // Find the only camera in the scene and set it "camera"

        gravity *= gravityMultiplier;                                       // Apply Gravity Multiplier to player gravity
    }


    private void Update()
    {
        CheckGrounded();
        CheckVault();
        CameraFollower();
    }

    private void FixedUpdate()
    {
        MovementInput();
        Gravity();
        Vault();
    }


    private void MovementInput()
    {
        if (isGrounded && !isVaulting)
        {
            xAxisMovement = Input.GetAxis("Horizontal") * movementSpeed;                // Get keyboard input for "A", "D" and "Right/Left Arrow", and apply movement speed;
            rb.velocity = new Vector2(xAxisMovement, rb.velocity.y);                    // Apply movement speed and direction to player velocity

            if (rb.velocity.magnitude > maxSpeed)                                       // Clamp player movement speed to max speed
            {
                rb.velocity = rb.velocity.normalized * maxSpeed;
            }

            if (!isInteracting)
            {
                // Rotate player in movement direction
                if (rb.velocity.x > 0)
                {
                    player.rotation = Quaternion.Euler(0, 0, 0);     // Set player rotation on Y axis to 90 deg
                }
                if (rb.velocity.x < 0)
                {
                    player.rotation = Quaternion.Euler(0, 180, 0);    // Set player rotation on Y axis to -90 deg
                }
            }
        }
    }

    private void Vault()
    {
        if (isVaulting)
        {
            if (canVault)
            {
                rb.velocity = player.up * vaultSpeed;   // Move player up when vaulting
            }

            if (!canVault)
            {
                StartCoroutine(VaultTimer());
            }
        }
    }

    private IEnumerator VaultTimer()    // Timer for when to disable vaulting (set "isVaulting" to false)
    {
        rb.velocity = player.right * vaultSpeed;
        yield return new WaitForSeconds(0.125f);
        isVaulting = false;
    }


    private void CheckVault()
    {
        // Size of collision box
        Vector2 boxSize = new Vector2(capsCollider.radius, capsCollider.height / 2);

        // Raycast for "canVault" bool (true if either raycast hits other object)
        bool vaultCheck = Physics.BoxCast(player.position, boxSize, player.TransformDirection(Vector2.right), Quaternion.identity, boxSize.x / 2);
        
        if (vaultCheck && !isInteracting)
        {
            canVault = true;
        } else {
            canVault = false;
        }

        Color color = Color.yellow;
        if (canVault)
        {
            color = Color.green;
        } else {
            color = Color.yellow;
        }

        Debug.DrawRay(player.position + player.TransformDirection(boxSize.x / 2, boxSize.y, 0), player.right * boxSize.x, color);           // Top
        Debug.DrawRay(player.position + player.TransformDirection(boxSize.x / 2, -boxSize.y, 0), player.right * boxSize.x, color);          // Buttom
        Debug.DrawRay(player.position + player.TransformDirection(boxSize.x * 1.5f, boxSize.y, 0), Vector2.down * boxSize.y * 2, color);    // Front
        Debug.DrawRay(player.position + player.TransformDirection(boxSize.x / 2, boxSize.y, 0), Vector2.down * boxSize.y * 2, color);       // Rear
        
        // If canVault is true and the player presses space, set "isVaulting" to true
        if (Input.GetKey(KeyCode.Space) && canVault)
        {
            isVaulting = true;
        }
    }


    private void CheckGrounded()
    {
        // Size of collision box
        Vector2 boxSize = new Vector2(capsCollider.radius, capsCollider.height / 20);
        isGrounded = Physics.BoxCast(player.position, boxSize, Vector2.down, Quaternion.identity, 1.25f);

        Color color = Color.yellow;
        if (isGrounded)
        {
            color = Color.green;
        } else {
            color = Color.yellow;
        }

        Debug.DrawRay(player.position + new Vector3(boxSize.x, -(capsCollider.height / 2)), new Vector2(0, -boxSize.y), color);                     // Front
        Debug.DrawRay(player.position + new Vector3(-boxSize.x, -(capsCollider.height / 2)), new Vector2(0, -boxSize.y), color);                    // Rear
        Debug.DrawRay(player.position + new Vector3(-boxSize.x, -(capsCollider.height / 2)), Vector2.right * (boxSize.x * 2), color);               // Top
        Debug.DrawRay(player.position + new Vector3(-boxSize.x, -(capsCollider.height / 2) -boxSize.y), Vector2.right * (boxSize.x * 2), color);    // Center
        
    }

    private void CameraFollower()
    {
        if (cameraPanOut)
        {
            if ((playerCamera.transform.position.z + 10) > player.position.x)
            {
                playerCamera.transform.position = player.position + new Vector3(0, 0, -10 + player.position.x);

            }
        }

        playerCamera.transform.position = player.position + new Vector3(0, 0, -10);
    }   

    private void Gravity()
    {
        // if the player is not vaulting, apply gravity/downforce
        if (!isVaulting)
        {
            rb.AddForce(Vector2.down * gravity);
        }
    }


}