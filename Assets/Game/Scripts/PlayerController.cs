using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Components
    private Rigidbody rb;                                                   // Set Rigidbody component to "rb"

    private Transform player;                                               // Set Transform component to "player"
    private Camera playerCamera;

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
        rb.freezeRotation = true;                                           // Disable rotatoin trhough rigidbody
        rb.useGravity = false;                                              // Disable Rigidbody gravity

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
            rb.velocity = new Vector3(xAxisMovement, rb.velocity.y, rb.velocity.z);     // Apply movement speed and direction to player velocity

            if (rb.velocity.magnitude > maxSpeed)                                       // Clamp player movement speed to max speed
            {
                rb.velocity = rb.velocity.normalized * maxSpeed;
            }

            // Rotate player in movement direction
            if (rb.velocity.x > 0)
            {
                player.rotation = Quaternion.Euler(0, 90, 0);     // Set player rotation on Y axis to 90 deg
            }
            if (rb.velocity.x < 0)
            {
                player.rotation = Quaternion.Euler(0, -90, 0);    // Set player rotation on Y axis to -90 deg
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
                rb.velocity = player.forward * vaultSpeed;
                StartCoroutine(VaultTimer());
            }
        }
    }

    private IEnumerator VaultTimer()    // Timer for when to disable vaulting (set "isVaulting" to false)
    {
        yield return new WaitForSeconds((1 / vaultSpeed) / 5);  // Wait "x" seconds before disabling vaulting (set "isVaulting" to false), time is proportinal with vault speed
        isVaulting = false;
    }


    private void CheckVault()
    {
        // Raycast for "canVault" bool (true if either raycast hits other object)
        if (Physics.Raycast(player.position + Vector3.up, player.TransformDirection(Vector3.forward), 0.6f) || Physics.Raycast(player.position, player.TransformDirection(Vector3.forward), 0.6f) || Physics.Raycast(player.position + Vector3.down, player.TransformDirection(Vector3.forward), 0.6f))
        {
            canVault = true;
        }
        else
        {
            canVault = false;
        }

        // If canVault is true and the player presses splace, set "isVaulting" to true
        if (Input.GetKeyDown(KeyCode.Space) && canVault)
        {
            isVaulting = true;
        }

        // Raycast visual for "canVault" bool (true if raycast hits other object)
        Debug.DrawRay(player.position + Vector3.up, player.TransformDirection(Vector3.forward) * 0.6f, Color.cyan);               // Upper
        Debug.DrawRay(player.position, player.TransformDirection(Vector3.forward) * 0.6f, Color.cyan);                                          // Middle
        Debug.DrawRay(player.position + Vector3.down , player.TransformDirection(Vector3.forward) * 0.6f, Color.cyan);    // Lower
    }


    private void CheckGrounded()
    {
        // Raycast for "GroundCheck" bool (true if  either raycast hits other object)
        if (Physics.Raycast(player.position + new Vector3(0.5f, -1, 0), player.TransformDirection(Vector3.down), 0.05f) || Physics.Raycast(player.position + Vector3.down, player.TransformDirection(Vector3.down), 0.05f) || Physics.Raycast(player.position + new Vector3(-0.5f, -1, 0), player.TransformDirection(Vector3.down), 0.05f))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;                     // set "isGrounded" to false
        }

        // Raycast visual for "GroundCheck" bools (true if raycast hits other object)
        Debug.DrawRay(player.position + new Vector3(0.5f, -1, 0), player.TransformDirection(Vector3.down) * 0.05f, Color.yellow);   // Front
        Debug.DrawRay(player.position + new Vector3(-0.5f, -1, 0), player.TransformDirection(Vector3.down) * 0.05f, Color.yellow);  // Back
        Debug.DrawRay(player.position + Vector3.down, player.TransformDirection(Vector3.down) * 0.05f, Color.yellow);               // Middle
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
            rb.AddForce(Vector3.down * gravity);
        }
    }
}