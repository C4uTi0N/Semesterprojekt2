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
    

    public float speed = 3;

    public float minDistance = 2;
    private float distance;
    
    public bool shouldFollow = false;

    private void Awake() 
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        yarnMemory = GameObject.Find("Dialogue Runner").GetComponent<InMemoryVariableStorage>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (yarnMemory.TryGetValue<bool>("$smolFollow", out var output))
        {
            shouldFollow = output;
        }

        distance = Vector2.Distance(rb.position, target.position);

        if (shouldFollow)
        {
            if (distance > minDistance)
            {
                rb.position = Vector2.Lerp(transform.position, target.position, speed / 100);
            }
        }
    }
    /*
    public void ShouldFollow(bool value)
    {
        shouldFollow = value;
    }*/
}

