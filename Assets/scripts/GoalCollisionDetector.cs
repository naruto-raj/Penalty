using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalCollisionDetector : MonoBehaviour
{
    [SerializeField] private GoalieAgentController goalieAgentController; // Reference to the GoalieAgent

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the ball collides with the goal
        if (collision.gameObject.CompareTag("Ball"))
        {
            // Check for null reference to avoid NullReferenceException
            if (goalieAgentController != null)
            {
                goalieAgentController.HandleGoalCollision();
            }
            else
            {
                Debug.LogError("GoalieAgentController reference not set in the GoalCollisionDetector script.");
            }
        }
    }
}
