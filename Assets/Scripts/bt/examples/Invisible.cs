using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invisible : BtNode
{
    private float time_Invis;

    // Method to hold the float time amount given
    public Invisible(float time)
    {
        time_Invis = time;
    }

    // the state which will return failure if the target is null or turn the mesh renderer off and apply a particle effect for the ghost if
    // the time amount has reached 0
   public override NodeState evaluate(Blackboard blackboard)
    {
        MeshRenderer rend = blackboard.owner.GetComponent<MeshRenderer>();
        ParticleSystem particle = blackboard.owner.GetComponent<ParticleSystem>();

        if (blackboard.target == null) {
            return NodeState.FAILURE;
        }
        else if(time_Invis <= 0)
        {
            particle.Stop();
            rend.enabled = true;
            return NodeState.FAILURE;
        }

        if ( time_Invis > 0)
        {
            time_Invis -= Time.deltaTime;
            particle.Play();
            rend.enabled = false;
            return NodeState.RUNNING;
        }

        return NodeState.SUCCESS;
    }

    public override string getName()
    {
        return "Invisible";
    }

}
