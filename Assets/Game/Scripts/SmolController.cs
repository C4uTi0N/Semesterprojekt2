using UnityEngine;
using Yarn.Unity;

[RequireComponent(typeof(Rigidbody))]
public class SmolController : MonoBehaviour
{
    private InMemoryVariableStorage yarnMemory;
    private Rigidbody rb;
    private Animator girlAnimations;
    public Transform target;
    private PlayerController playerController;

    private float distance;

    public float speed = 3;
    public float minDistance = 2;

    public bool shouldFollow = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        girlAnimations = GetComponent<Animator>();
        playerController = GameObject.Find("Player Character").GetComponent<PlayerController>();

        yarnMemory = GameObject.Find("Dialogue Runner").GetComponent<InMemoryVariableStorage>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (shouldFollow && playerController.isVaulting)
        {
            rb.position = Vector3.Lerp(rb.position, playerController.transform.position + new Vector3(-0.1f, 0.5f, 1), 0.1f);
        }
    }

    private void FixedUpdate()
    {
        shouldFollow = yarnMemory.TryGetValue<bool>("$smolFollow", out var smolFollow);

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
                girlAnimations.SetBool("playAnimation", true);      //Starts animation if she's moving
                rb.position = Vector2.Lerp(transform.position, target.position, speed / 100);
            }
            else
            {
                girlAnimations.SetBool("playAnimation", false);   //Stops animation if she's not moving
            }
        }
    }
}