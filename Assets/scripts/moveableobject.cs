using System.Collections;
using System.Collections.Generic;


using UnityEngine;
using UnityEngine.SceneManagement;

public class BallMovement : MonoBehaviour
{
    public float forceAmount = 10f; // Customize the force amount
    public float resetHeight = -10f; // Height at which the game will reset

    private Rigidbody rb; // Rigidbody of the ball

    public bool iskicked = false;
    [SerializeField] public StrikerRaycastController ragdollRaycastController; // Reference to the StrikerRaycastController
    [SerializeField] private GoalieAgentController goalieAgentController; // Reference to the GoalieAgent

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // check if the ball position is behind striker or ball velocity is zero and iskicked is true
        if ((transform.position.z < -1.0f || rb.velocity.magnitude < 0.1f ) && iskicked)
        {
            // call goalie agent to reset the game
            if (goalieAgentController != null)
            {
                Debug.Log("Ball is behind the striker or velocity is zero");
                goalieAgentController.RestartGame();
            }
            else
            {
                Debug.LogError("GoalieAgentController reference not set in the GoalCollisionDetector script.");
            }
        }

    }
    void OnCollisionEnter(Collision collision)
    {
        // Check if the collider belongs to the Striker
        if (collision.gameObject.CompareTag("Striker"))
        {
            Vector3 directionToGoal = ragdollRaycastController.GetDirectionToGoal();
            // Apply a force to the ball in the direction determined by the Striker's raycasts
            if (directionToGoal != Vector3.zero)
            {
                rb.AddForce(directionToGoal.normalized * forceAmount);
                // Access the striker's Animator and set it to idle
                Animator strikerAnimator = collision.gameObject.GetComponent<Animator>();
                if (strikerAnimator != null)
                {
                    strikerAnimator.SetBool("Kick", false);
                    strikerAnimator.SetBool("Idle", true);
                    Debug.Log("Striker has kicked the ball!");
                    iskicked = true;
                }
            }
        }

    }

}
