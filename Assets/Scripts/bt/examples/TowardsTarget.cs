using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AI;


public class TowardsTarget : BtNode
{
    private NavMeshAgent m_agent;
    private float _speed;
    
    //Created method to hold the speed variable for the ghosts
    public TowardsTarget(float speed)
    {          
        _speed = speed;  
    }
    
    public override NodeState evaluate(Blackboard blackboard) {
        if (m_agent == null) {
            m_agent = blackboard.owner.GetComponent<NavMeshAgent>();
            m_agent.speed = _speed;
        }

        // if target is null, we can't move towards it!
        if (blackboard.target == null) {
            return NodeState.FAILURE;
        }

        m_agent.SetDestination(blackboard.target.transform.position);
        //Debug.Log("Agent: " + blackboard.owner.name + ", Target: " + blackboard.target.name);
        if ( Vector3.Distance(blackboard.owner.transform.position, blackboard.target.transform.position) < 0.5 )
        {
            return NodeState.RUNNING;
        }

        return NodeState.SUCCESS;
    }

    public override string getName()
    {
        return "TowardsTarget";
    }

}
