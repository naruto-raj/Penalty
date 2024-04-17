using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

public class StrikerAnimationController : Agent
{
    private Animator animator;
    public bool kick = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }


}
