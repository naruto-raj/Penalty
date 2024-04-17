using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

using TMPro;

public class StrikerRaycastController : MonoBehaviour
{
    [SerializeField] private Transform ballTransform; // Assign in the inspector

    [SerializeField] private GameObject goal; // Assign in the inspector
    [SerializeField] private Vector3 directionToGoal = Vector3.zero; // Direction from the ball to the goal

    [SerializeField] private TMP_Text  overlayText; // Assign in the inspector

    [SerializeField] private Animator strikeranimator;
    [SerializeField] private GameObject striker;
    public Vector3 GetDirectionToGoal()
    {
        return directionToGoal;
    }
    public Vector3 SetDirectionToGoal()
    {
        return directionToGoal = Vector3.zero;
    }
    void Start()
    {
        strikeranimator = GetComponent<Animator>();
        
    }
    void Update()
    {
        // Check if the raycast has not been performed and direction to goal is not zero
        if (directionToGoal == Vector3.zero)
        {
            PerformRaycastsToFindGoal();
        }
        
    }
    

    void PerformRaycastsToFindGoal()
    {
        Vector3 randomDirection = Random.onUnitSphere; // Generate a random direction
        RaycastHit hit;

        if (Physics.Raycast(transform.position, randomDirection, out hit))
        {
            // get goal objects collider
            Collider goalCollider = goal.GetComponent<Collider>();


            // check if the raycast hit the goal object
            if (hit.collider == goalCollider)
            {
                // Calculate and store the direction from the ball to the goal
                directionToGoal = hit.point - ballTransform.position;
                strikeranimator.SetBool("Kick", true);
                // Calculate the angle
                float angle = Vector3.Angle(directionToGoal, goal.transform.forward);
                // Cross product to determine if the angle is to the left or right of forward
                Vector3 cross = Vector3.Cross(goal.transform.forward, directionToGoal);
                bool isLeft = cross.y < 0; // Negative Y in the cross product indicates right

                if (angle < 20f)
                {
                    Debug.Log("Ball is in the center");
                    overlayText.text = "Center";
                    // striker.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    striker.transform.localPosition = new Vector3(-0.74f, 0, -1.32f);
                    strikeranimator.SetInteger("Direction", 0);
                }
                else if (angle > 20f && angle <= 60f)
                {
                    if (isLeft)
                    {
                        Debug.Log("Ball is to the Left");
                        overlayText.text = "Left";
                        
                    }
                    else
                    {
                        Debug.Log("Ball is to the Right");
                        overlayText.text = "Right";
                    }
                    // set striker animation to far left or far right
                    strikeranimator.SetInteger("Direction", isLeft ? -1 : 1);
                }
                else
                {
                    // For angles greater than 60 degrees, you might consider them as "far left/right" or similar
                    overlayText.text = isLeft ? "Far Left" : "Far Right";
                    // set striker animation to far left or far right
                    strikeranimator.SetInteger("Direction", isLeft ? -1 : 1);
                }
                Debug.DrawRay(transform.position, randomDirection * hit.distance, Color.green, 1.0f); // Optional: for visualization
            }
            // draw directionToGoal ray
            Debug.DrawRay(ballTransform.position, directionToGoal, Color.blue, 1.0f); // Optional: for visualization

            

        }
    }
}

