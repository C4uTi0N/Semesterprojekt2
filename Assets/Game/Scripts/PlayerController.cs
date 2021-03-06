using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
        //Components
    private Rigidbody rb;                                                   // Set Rigidbody component to "rb"
    private Transform player;                                               // Set Transform component to "player"

        // Movement input Axis & Direction
    private float xAxisMovement;                                            // Movement on the "X" axis

        // Player Settings
    private float gravity = 9.82f;                                          // Gravity value
    [Header("Player settings", order = 1)]
    public float gravityMultiplier = 3;                                     // Gravity Multiplier
    public float movementSpeed = 10f;                                       // Movement speed
    public float vaultSpeed = 5f;                                           // Vaulting speed
    public float maxSpeed = 20;                                             // Max movement speed

        //Debug bools
    [Header("Debug bools", order = 2)]
    public bool canVault;                                                   // Bool to check if player can vault
    public bool isVaulting;                                                 // Bool to check if player is vaulting
    public bool canVaultUpwards;                                            // Bool to check if player should vault upwards


    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();                                     // Set "rb" to Rigidbody of attached Gameobject
        rb.freezeRotation = true;                                           // Disable rotatoin trhough rigidbody
        rb.useGravity = false;                                              // Disable Rigidbody gravity

        player = GetComponent<Transform>();                                 // Set "player" to Transform of attached gameobject

        gravity *= gravityMultiplier;                                       // Apply Gravity Multiplier to player gravity
    }

    // Update is called once per frame
    void Update()
    {
        CheckVault();
    }

    void FixedUpdate()
    {
        MovementInput();
        Vault();
        Gravity();
    }

    private void MovementInput()
    {
        if (!isVaulting)
        {
                // Get keyboard input for "A", "D" and "Right/Left Arrow", and apply movement speed;
            xAxisMovement = Input.GetAxis("Horizontal") * movementSpeed;

                // Apply movement speed and direction to player velocity
            rb.velocity = new Vector3(xAxisMovement, rb.velocity.y, rb.velocity.z);

                // Clamp player movement speed to max speed
            if (rb.velocity.magnitude > maxSpeed)
            {
                rb.velocity = rb.velocity.normalized * maxSpeed;
            }

                // Rotate player in movement direction
            if (rb.velocity.x > 0)
            {
                player.transform.rotation = Quaternion.Euler(0, 90, 0);     // Set player rotation on Y axis to 90 deg
            }
            if (rb.velocity.x < 0)
            {
                player.transform.rotation = Quaternion.Euler(0, -90, 0);    // Set player rotation on Y axis to -90 deg
            }
        }
    }

    private void Vault()
    {
        if (isVaulting)
        {
            if (canVaultUpwards)
            {
                rb.velocity = Vector3.up * vaultSpeed;
            }

            if (!canVaultUpwards)
            {
                rb.velocity = Vector3.right * vaultSpeed;
                StartCoroutine(VaultTimer());
            }
        }
    }

    private void CheckVault()
    {
            // Raycast for "canVault" bool (true if raycast hits other object)
        canVault = Physics.Raycast(player.transform.position, player.transform.TransformDirection(Vector3.forward), 0.6f);
        Debug.DrawRay(player.transform.position, player.transform.TransformDirection(Vector3.forward) * 0.6f, Color.cyan);

            // Raycast for "canVaultUpwards" bool (true if raycast hits other object)
        canVaultUpwards = Physics.Raycast(player.transform.position - new Vector3(0, 1, 0), player.transform.TransformDirection(new Vector3(0, 0, 1)), 0.6f);
        Debug.DrawRay(player.transform.position - new Vector3(0, 1, 0), player.transform.TransformDirection(new Vector3(0, 0, 1)) * 0.6f, Color.red);

            // If canVault is true and the player presses splace, set "isVaulting" to true
        if (Input.GetKeyDown(KeyCode.Space) && canVault)
        {
            isVaulting = true;
        }
    }

    private void Gravity()
    {
            // if the player is not vaulting, apply gravity/downforce
        if (!isVaulting)
        {
            rb.AddForce(Vector3.down * gravity);
        }
    }

        // Timer for when to disable vaulting (set "isVaulting" to false)
    IEnumerator VaultTimer()
    {
            // Wait 1/vaultSpeed seconds before disable vaulting (set "isVaulting" to false
        yield return new WaitForSeconds(1 / vaultSpeed);
        isVaulting = false;
    }
}
