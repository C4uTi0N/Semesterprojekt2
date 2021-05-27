using System.Collections;
using UnityEngine;
using Yarn.Unity;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Transform))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
    // Components
    private Rigidbody rb;                                                   // Set Rigidbody component to "rb"
    private Transform playerTrans;                                          // Set Transform component to "player"
    private Camera playerCamera;                                            // Set Camera component to "playerCamera"
    private Animator boyAnimations;                                         // Set the Animator to "boyAnimations"
    private GameObject smol;                                                // Set GameObject component to "smol"
    private InMemoryVariableStorage yarnMemmory;                            // Set InMemoryVariableStorage to "yarnMemory"
    private AudioSource audioSrc;                                           // Set AudioSource to "audioSrc"
    public CapsuleCollider capsCollider;                                    // Set the Capsule Collider of the player


    // Movement input Axis & Direction
    private float xAxisMovement;                                            // Movement on the "X" axis

    // Player Settings
    private float gravity = 9.82f;                                          // Gravity value
    private float playerPositionOffset;                                     // Offset fo player position for camera
    private float cameraOffsetY;                                            // Offset for camera on Y axis

    [Header("Player settings", order = 1)]
    public float gravityMultiplier = 5;                                     // Gravity Multiplier
    public float movementSpeed = 10f;                                       // Movement speed
    public float vaultSpeed = 5f;                                           // Vaulting speed
    public float maxSpeed = 20;                                             // Max movement speed
    public bool cameraPanOut = false;                                       // If true, camera will pan out when moving
    public bool cameraFollow = true;                                        // If true, camera will follow when moving

    //Debug bools

    [Header("Debug bools", order = 2)]
    public bool isGrounded;                                                 // Bool to check if player is on ground
    public bool canVault;                                                   // Bool to check if player can vault
    public bool isVaulting;                                                 // Bool to check if player is vaulting
    public bool isInteracting;                                              // Bool to check if player is interacting
    public bool cutsceneRunning;                                            // Bool to check if cutscene is running

    public UnityEngine.Events.UnityEvent onEscape;                        // Unity Event for pressing Escape

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();                                     // Set "rb" to Rigidbody of attached Gameobject
        rb.freezeRotation = true;                                           // Disable rotation through rigidbody
        rb.useGravity = false;                                              // Disable Gravity from Rigidbody

        playerTrans = GetComponent<Transform>();                            // Set "player" to Transform of attached gameobject
        playerCamera = FindObjectOfType<Camera>();                          // Find the only camera in the scene and set it "camera"
        boyAnimations = GetComponent<Animator>();

        smol = GameObject.Find("Smol");
        yarnMemmory = GameObject.Find("Dialogue Runner").GetComponent<InMemoryVariableStorage>();
        capsCollider = GetComponent<CapsuleCollider>();

        audioSrc = GetComponent<AudioSource>();

        gravity *= gravityMultiplier;                                       // Apply Gravity Multiplier to player gravity
    }

    private void Update()
    {
        CheckGrounded();
        CheckVault();
        CameraFollower();
        BoyAnimator();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            onEscape.Invoke();
        }

    }

    private void FixedUpdate()
    {
        MovementInput();
        Vault();
        Gravity();
    }

    private void MovementInput()
    {
        yarnMemmory.TryGetValue<bool>("$cutsceneRunning", out bool cutsceneRunning);    // Get "$cutsceneRunning" bool from Yarn Program

        if (isGrounded && !isVaulting && !cutsceneRunning)
        {
            xAxisMovement = Input.GetAxis("Horizontal") * movementSpeed;                // Get keyboard input for "A", "D" and "Right/Left Arrow", and apply movement speed;
            rb.velocity = new Vector2(xAxisMovement, rb.velocity.y);                    // Apply movement speed and direction to player velocity

            if (xAxisMovement != 0)
            {
                if (!audioSrc.isPlaying)
                {
                    audioSrc.PlayOneShot(audioSrc.clip, 1.5f);
                }
            }
            else
            {
                audioSrc.Stop();
            }

            if (rb.velocity.magnitude > maxSpeed)                                       // Clamp player movement speed to max speed
            {
                rb.velocity = rb.velocity.normalized * maxSpeed;
            }

            if (!isInteracting)
            {
                // Rotate player in movement direction
                if (rb.velocity.x > 0)
                {
                    playerTrans.rotation = Quaternion.Euler(0, 0, 0);     // Set player rotation on Y axis to 90 deg
                }
                if (rb.velocity.x < 0)
                {
                    playerTrans.rotation = Quaternion.Euler(0, 180, 0);    // Set player rotation on Y axis to -90 deg
                }
            }
        }
    }

    private void BoyAnimator()
    {
        if (rb.velocity.x > 0)
        {
            boyAnimations.SetFloat("boySpeed", 1);
            boyAnimations.SetBool("boyRight", true);
            boyAnimations.SetBool("boyIdle", false);
        }
        if (rb.velocity.x < 0)
        {
            boyAnimations.SetFloat("boySpeed", -1);
            boyAnimations.SetBool("boyRight", false);
            boyAnimations.SetBool("boyIdle", false);
        }
        if (Input.GetAxis("Horizontal") == 0)
        {
            boyAnimations.SetFloat("boySpeed", 0);
            boyAnimations.SetBool("boyIdle", true);
        }
    }

    private void Vault()
    {
        if (isVaulting)
        {
            if (canVault)
            {
                rb.velocity = playerTrans.up * vaultSpeed;   // Move player up when vaulting
            }

            if (!canVault)
            {
                StartCoroutine(VaultTimer());
            }
        }
    }

    private IEnumerator VaultTimer()    // Timer for when to disable vaulting (set "isVaulting" to false)
    {
        rb.velocity = playerTrans.right * vaultSpeed;

        if (smol.GetComponent<SmolController>().shouldVault)
            smol.GetComponent<Rigidbody>().velocity = playerTrans.right * (vaultSpeed / 2);

        yield return new WaitForSeconds(0.125f);
        isVaulting = false;
    }

    private void CheckVault()
    {
        // Size of collision box
        Vector2 boxSize = new Vector2(capsCollider.radius / 2, capsCollider.height / 2);

        // Raycast for "canVault" bool (true if either raycast hits other object)
        RaycastHit hitInfo;
        bool vaultCheck = Physics.BoxCast(playerTrans.position + new Vector3(0, boxSize.y, 0), boxSize, playerTrans.TransformDirection(Vector2.right), out hitInfo, Quaternion.identity, boxSize.x * 1.5f);

        if (vaultCheck)
        {
            if (hitInfo.collider.GetComponent<Vaultable>() != null)
            {
                vaultCheck = true;
            }
            else
            {
                vaultCheck = false;
            }
        }

        if (vaultCheck && !isInteracting)
        {
            canVault = true;
        }
        else
        {
            canVault = false;
        }

        Color color = Color.yellow;
        if (canVault)
        {
            color = Color.green;
        }
        else
        {
            color = Color.yellow;
        }

        Debug.DrawRay(playerTrans.position + playerTrans.TransformDirection(boxSize.x * 2.5f, boxSize.y * 2, 0), Vector2.down * boxSize.y * 2, color);    // Front
        Debug.DrawRay(playerTrans.position + playerTrans.TransformDirection(boxSize.x, boxSize.y * 2, 0), Vector2.down * boxSize.y * 2, color);           // Rear
        Debug.DrawRay(playerTrans.position + playerTrans.TransformDirection(boxSize.x, boxSize.y * 2, 0), playerTrans.right * boxSize.x * 1.5f, color);            // Top
        Debug.DrawRay(playerTrans.position + playerTrans.TransformDirection(boxSize.x, 0, 0), playerTrans.right * boxSize.x * 1.5f, color);                       // Buttom

        // If canVault is true and the player presses space, set "isVaulting" to true
        if (Input.GetKey(KeyCode.Space) && canVault)
        {
            isVaulting = true;
        }
    }

    private void CheckGrounded()
    {
        // Size of collision box
        Vector2 boxSize = new Vector2(capsCollider.radius, capsCollider.height / 10);
        isGrounded = Physics.BoxCast(playerTrans.position + new Vector3(0, boxSize.y * 2, 0), boxSize, Vector2.down, Quaternion.identity, boxSize.y + (boxSize.y / 2));

        Color color = Color.yellow;
        if (isGrounded)
        {
            color = Color.green;
        }
        else
        {
            color = Color.yellow;
        }

        Debug.DrawRay(playerTrans.position + new Vector3(boxSize.x, boxSize.y / 2), new Vector2(0, -boxSize.y), color);              // Front
        Debug.DrawRay(playerTrans.position + new Vector3(-boxSize.x, boxSize.y / 2), new Vector2(0, -boxSize.y), color);             // Rear
        Debug.DrawRay(playerTrans.position + new Vector3(-boxSize.x, boxSize.y / 2), Vector2.right * (boxSize.x * 2), color);        // Top
        Debug.DrawRay(playerTrans.position + new Vector3(-boxSize.x, -boxSize.y / 2), Vector2.right * (boxSize.x * 2), color);       // Buttom
    }

    public bool CameraFollow
    {
        get { return cameraFollow; }
        set
        {
            if (value == cameraFollow)
            {
                return;
            }
            cameraFollow = value;
        }
    }

    public bool CameraPanOut
    {
        get { return cameraPanOut; }
        set
        {
            if (value == cameraPanOut)
            {
                return;
            }
            cameraPanOut = value;
            if (cameraPanOut)
                playerPositionOffset = playerTrans.position.x;
        }
    }

    private void CameraFollower()
    {
        float originCameraOrthSize = 3.6f;      // Original size of Player Camera in orthographic view

        if (cameraFollow)
        {
            if (cameraPanOut && playerCamera.orthographicSize < 7)      // Pan out
            {
                cameraOffsetY = playerTrans.position.x / 8;
                playerCamera.orthographicSize = originCameraOrthSize + (playerTrans.position.x - playerPositionOffset) / 3;
                playerCamera.transform.position = playerTrans.position + new Vector3(0, cameraOffsetY, -2);
            }

            if (cameraPanOut)
            {
                playerCamera.transform.position = playerTrans.position + new Vector3(0, cameraOffsetY, -2);
            }

            if (!cameraPanOut)
            {
                playerCamera.transform.position = playerTrans.position + new Vector3(0, 2, -2);
            }
        }
        else
        {
            playerCamera.transform.position = Vector3.Lerp(playerCamera.transform.position, GameObject.Find("Camera Lock").transform.position, Time.deltaTime);
        }
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