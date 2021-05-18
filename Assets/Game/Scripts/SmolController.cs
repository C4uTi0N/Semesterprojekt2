using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

[RequireComponent(typeof(Rigidbody))]
public class SmolController : MonoBehaviour
{
    private InMemoryVariableStorage yarnMemory;
    private Rigidbody rb;
    public Transform target;
    
    private float distance;

    public float speed = 3;
    public float minDistance = 2;

    public bool shouldFollow = false;

    public Animator girlAnimations; //Animator


    private void Awake() 
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        yarnMemory = GameObject.Find("Dialogue Runner").GetComponent<InMemoryVariableStorage>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (rb.position.x < target.position.x)
        {
            rb.rotation = Quaternion.Euler(0, 0, 0);     // Set player rotation on Y axis to 90 deg
        }
        if (rb.position.x > target.position.x)
        {
            rb.rotation = Quaternion.Euler(0, 180, 0);    // Set player rotation on Y axis to -90 deg
        }

        if (yarnMemory.TryGetValue<bool>("$smolFollow", out var output))
        {
            shouldFollow = output;
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

