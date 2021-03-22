using System.Collections;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    //Components
    private Rigidbody rb;                                                   // Set Rigidbody component to "rb"
    private Transform player;                                               // Set Transform component to "player"
    private Camera playerCamera;                                            // Set Camera component to "playerCamera"

    // Movement input Axis & Direction
    private float xAxisMovement;                                            // Movement on the "X" axis

    // Player Settings
    private float gravity = 9.82f;                                          // Gravity value

    [Header("Player settings", order = 1)]
    public float gravityMultiplier = 5;                                     // Gravity Multiplier

    public float movementSpeed = 10f;                                       // Movement speed
    public float vaultSpeed = 5f;                                           // Vaulting speed
    public float maxSpeed = 20;                                             // Max movement speed

    //Debug bools
    [Header("Debug bools", order = 2)]
    public bool isGrounded;                                                 // Bool to check if player is on ground
    public bool canVault;                                                   // Bool to check if player can vault
    public bool isVaulting;                                                 // Bool to check if player is vaulting


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();                                     // Set "rb" to Rigidbody of attached Gameobject
        rb.freezeRotation = true;                                           // Disable rotatoin through rigidbody
        rb.useGravity = false;                                              // Disable Gravity from Rigidbody

        player = GetComponent<Transform>();                                 // Set "player" to Transform of attached gameobject

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
        yield return new WaitForSeconds((1 / vaultSpeed) / 5);  // Wait "x" seconds before disabling vaulting (set "isVaulting" to false), time is proportinal with vault speed
        isVaulting = false;
    }


    private void CheckVault()
    {
        // Size of collision box
        Vector2 boxSize = new Vector2(0.1f, 1f);

        // Raycast for "canVault" bool (true if either raycast hits other object)
        bool vaultCheck = Physics.BoxCast(player.position + player.TransformDirection(Vector2.right * 0.39f), boxSize, player.TransformDirection(Vector2.right), Quaternion.identity, 0.1f);
        
        if (vaultCheck)
        {
            canVault = true;
        } else {
            canVault = false;
        }

        // If canVault is true and the player presses splace, set "isVaulting" to true
        if (Input.GetKeyDown(KeyCode.Space) && canVault)
        {
            isVaulting = true;
        }

        Color color = Color.yellow;
        if (canVault)
        {
            color = Color.green;
        } else {
            color = Color.yellow;
        }

        Debug.DrawRay(player.position + player.TransformDirection(0.5f, boxSize.y, 0), player.TransformDirection(Vector2.right) * (boxSize.x - 0.01f), color);        // Top
        Debug.DrawRay(player.position + player.TransformDirection(0.5f, -boxSize.y, 0), player.TransformDirection(Vector2.right) * (boxSize.x - 0.01f), color);       // Buttom
        Debug.DrawRay(player.position + player.TransformDirection(0.49f + boxSize.x, -boxSize.y, 0), Vector2.up * (boxSize.y * 2), color);                   // Center
    }


    private void CheckGrounded()
    {
        // Size of collision box
        Vector2 boxSize = new Vector2(0.5f, 0.1f);
        isGrounded = Physics.BoxCast(player.position + player.TransformDirection(Vector2.down * 0.89f), boxSize, player.TransformDirection(Vector2.down), Quaternion.identity, 0.1f);

        Color color = Color.yellow;
        if (isGrounded)
        {
            color = Color.green;
        } else {
            color = Color.yellow;
        }

        Debug.DrawRay(player.position + new Vector3(boxSize.x, -1), Vector2.down * (boxSize.y - 0.01f), color);                 // Front
        Debug.DrawRay(player.position + new Vector3(-boxSize.x, -1), Vector2.down * (boxSize.y - 0.01f), color);                // Rear
        Debug.DrawRay(player.position + new Vector3(-boxSize.x, -0.99f -boxSize.y), Vector2.right * (boxSize * 2), color);      // Center
        
    }

    private void CameraFollower()
    {
        playerCamera.transform.position = player.position + new Vector3(0, 0, -10);     // Makes the Camera follow the player
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