using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

[RequireComponent(typeof(Rigidbody))]
public class SmolController : MonoBehaviour
{
    private Rigidbody rb;
    public Transform target;
    public YarnProgram storyScript;

    public float speed = 3;

    public float minDistance = 2;
    private float distance;
    
    public bool shouldFollow = false;

    private void Awake() 
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        distance = Vector2.Distance(rb.position, target.position);

        if (shouldFollow)
        {
            if (distance > minDistance)
            {
                rb.position = Vector2.Lerp(transform.position, target.position, speed / 100);
            }
        }
        
    }
    public void ShouldFollow(bool value)
    {
        shouldFollow = value;
    }
}

