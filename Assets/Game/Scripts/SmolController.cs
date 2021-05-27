using System.Collections;
using UnityEngine;
using Yarn.Unity;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]
public class SmolController : MonoBehaviour
{
    private Rigidbody rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private PlayerController playerController;
    private InMemoryVariableStorage yarnMemory;
    private AudioSource audioSrc;
    public Transform target;
    public GameObject swingUsable;
    public GameObject smolOnSwing;

    private float distance;

    public float speed = 3;
    public float minDistance = 2;

    public bool shouldFollow = false;
    public bool shouldVault = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerController = GameObject.Find("Player Character").GetComponent<PlayerController>();
        yarnMemory = GameObject.Find("Dialogue Runner").GetComponent<InMemoryVariableStorage>();

        audioSrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void Update()
    {
        yarnMemory.TryGetValue<bool>("$smolFollow", out var smolFollow);
        shouldFollow = smolFollow;

        if (shouldVault && playerController.isVaulting)
        {
            rb.position = Vector3.Lerp(rb.position, playerController.transform.position + new Vector3(-0.1f, 0.5f, 1), 0.1f);
        }
    }

    private void FixedUpdate()
    {

        if (rb.position.x < target.position.x)
        {
            rb.rotation = Quaternion.Euler(0, 0, 0);     // Set player rotation on Y axis to 90 deg
        }
        if (rb.position.x > target.position.x)
        {
            rb.rotation = Quaternion.Euler(0, 180, 0);    // Set player rotation on Y axis to -90 deg
        }

        distance = Vector2.Distance(rb.position, target.position);

        if (shouldFollow)
        {
            if (distance > minDistance)
            {
                animator.SetBool("playAnimation", true);      //Starts animation if she's moving
                rb.position = Vector2.Lerp(transform.position, target.position, speed / 100);
                if (!audioSrc.isPlaying)
                {
                    audioSrc.PlayOneShot(audioSrc.clip, 1);
                }
            }
            else
            {
                animator.SetBool("playAnimation", false);   //Stops animation if she's not moving;
                audioSrc.Stop();
            }
        }

        if (yarnMemory.TryGetValue<bool>("$cutsceneRunning", out bool cutsceneRunning))
        {
            if (smolOnSwing.activeSelf)
            {
                if (!cutsceneRunning)
                {
                    smolOnSwing.SetActive(false);
                    spriteRenderer.enabled = true;
                    swingUsable.SetActive(true);
                }
            }
        }
    }

    public bool ShouldVault
    {
        get { return shouldVault; }
        set
        {
            if (value == shouldVault)
            {
                return;
            }
            shouldVault = value;
        }
    }

    public void SmolOnSwing()
    {
        smolOnSwing.SetActive(true);
        spriteRenderer.enabled = false;
        swingUsable.SetActive(false);
    }
}