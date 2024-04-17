using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationStateControllerGoalie : MonoBehaviour
{
    private Animator animator;
    private float initialSpeed = 3f; // Base speed
    public float speed; // Current speed, adjusted based on key press duration
    private float keyPressDuration = 0f; // Duration of the current key press

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Reset speed to base speed initially
        speed = initialSpeed;

        // Increase keyPressDuration if "A" is being pressed
        if (Input.GetKey(KeyCode.D))
        {
            keyPressDuration += Time.deltaTime; // Increment the duration
            animator.SetInteger("Direction", -1);
            
        }
        // Increase keyPressDuration if "D" is being pressed
        else if (Input.GetKey(KeyCode.A))
        {
            keyPressDuration += Time.deltaTime; // Increment the duration
            animator.SetInteger("Direction", 1);
        }
        else
        {
            animator.SetInteger("Direction", 0);
            keyPressDuration = 0f; // Reset the duration if no relevant keys are pressed
        }

        // Calculate the speed based on keyPressDuration, if a key is pressed
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            // Example of adjusting speed based on key press duration
            // Here, speed increases linearly with time. Adjust the formula as needed for your game's feel
            speed += keyPressDuration;

            // Apply movement
            Vector3 direction = Input.GetKey(KeyCode.A) ? Vector3.left : Vector3.right;
            transform.Translate(direction * speed * Time.deltaTime);
        }
    }
}
