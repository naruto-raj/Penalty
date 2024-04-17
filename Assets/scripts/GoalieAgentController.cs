using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

using UnityEngine.SceneManagement;


public class GoalieAgentController : Agent
{
    [SerializeField] private float initialSpeed = 5f; // Base speed
    [SerializeField] private float resetHeight = -10f;
    [SerializeField] private float speed; // Current speed, adjusted based on key press duration
    [SerializeField] private float keyPressDuration = 0f; // Duration of the current key press
    [SerializeField] private GameObject striker;
    [SerializeField] private GameObject ball;
    [SerializeField] private GameObject Goal;
    [SerializeField] private Animator strikeranimator;
    [SerializeField] private Animator Goalieanimator;

    [SerializeField] private char keypress;
    [SerializeField] private Material SaveMaterial;
    [SerializeField] private Material GoalMaterial;

    void Start()
    {   
        Goalieanimator = GetComponent<Animator>();
        strikeranimator = striker.GetComponent<Animator>();
    }
    public override void OnEpisodeBegin()
    {   
        Debug.Log("Episode has begun!");
        
        // Reset the position of the striker and the ball
        ball.transform.localPosition = new Vector3(-0.28f, 0.27f, 1.32f);
        ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        //set ball public variable iskicked to false
        ball.GetComponent<BallMovement>().iskicked = false;
        // set striker getdirectiontogaol to zero
        striker.GetComponent<StrikerRaycastController>().SetDirectionToGoal();

        // Reset the position of the striker
        striker.transform.localPosition = new Vector3(-0.24f, 0, -1.0f);
        striker.transform.localRotation = Quaternion.Euler(0, 0, 0);

        // spawn goalie randomly withinx x axis
        this.transform.localPosition = new Vector3(Random.Range(-7.0f, 7.0f), 0, 10f);
        this.transform.localRotation = Quaternion.Euler(0, 180, 0);

        // set striker to kick animation
        strikeranimator.SetBool("Kick", true);
        speed = initialSpeed;

    }
    public override void CollectObservations(VectorSensor sensor)
    {
        // Add the position of the striker and the ball to the observations
        // sensor.AddObservation(striker.transform.localPosition);
        // sensor.AddObservation(ball.transform.localPosition);
        // sensor.AddObservation(this.transform.localPosition);
        // sensor.AddObservation(speed);
    }
    
    public override void OnActionReceived(ActionBuffers actions)
    {

        float moveGoalie = actions.ContinuousActions[0];
        float acceleration = actions.ContinuousActions[1];
        // create increase or decrease speed based on acceleration formula
        speed = speed + acceleration;


        // Determine direction for animation
        int direction = moveGoalie > 0 ? 1 : (moveGoalie < 0 ? -1 : 0);
        // set keypress to 'a' or 'd' based on direction
        keypress = direction > 0 ? 'd' : (direction < 0 ? 'a' : 's');
        Debug.Log($"Direction: {direction}" + " " + $"Keypress: {keypress}");

        Goalieanimator.SetInteger("Direction", direction);

        // Move the goalie
        Vector3 moveDirection = new Vector3(moveGoalie, 0, 0);
        this.transform.Translate(moveDirection * speed * Time.deltaTime);
        
        // Check if the ball has fallen below a certain height
        if (ball.transform.position.y < resetHeight)
        {
            Debug.Log("Ball fell below the reset height!");
            EndEpisode();
        }

    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {   
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");

        // For the Heuristic method, adjust keyPressDuration based on manual input to simulate training behavior
        keyPressDuration = (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) ? keyPressDuration + Time.deltaTime : 0;
        speed = initialSpeed + keyPressDuration/2; // Adjust speed similarly as in OnActionReceived
        continuousActionsOut[1] = speed;

    }
    // Adjusted OnCollisionEnter method
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the agent collided with the ball
        if (collision.gameObject.CompareTag("Ball"))
        {
            // Positive reward for interacting with the ball, implying a save attempt
            AddReward(1f);
            Debug.Log("Goalie interacted with the ball! Reward +1");
            // Goal.GetComponent<ColorChanger>().ChangeColor(Color.green);
            // change Goal material to new material
            Goal.GetComponent<Renderer>().material = SaveMaterial;
            // Optionally end the episode if you want the interaction with the ball to conclude the attempt
            EndEpisode();
        }
    }

    // Add this method to the GoalieAgentController class
    public void HandleGoalCollision()
    {
        // Negative reward for the ball colliding with the goal, indicating a missed save
        AddReward(-1f);
        Debug.Log("Ball hit the goal! Reward -1");
        // set goal object color
        // Goal.GetComponent<ColorChanger>().ChangeColor(Color.red);
        Goal.GetComponent<Renderer>().material = GoalMaterial;
        // End the episode on a goal
        EndEpisode();
    }

    
    public void RestartGame()
    {
        // Reloads the current scene
        Debug.Log("Restarting the game...");
        EndEpisode();
    }
}
