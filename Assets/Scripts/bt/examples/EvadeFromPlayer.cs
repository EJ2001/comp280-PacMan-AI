using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EvadeFromPlayer : BtNode
{
    private Transform playerObj;
    private NavMeshAgent agent;

    // Constructor which holds the player object
    public EvadeFromPlayer(GameObject player)
    {
        playerObj = player.transform;
    }

    // This method creates the destination for the ghost to evade to by adding a distance between the ghost to the player onto the ghost
    public override NodeState evaluate(Blackboard blackboard)
    {
        agent = blackboard.owner.GetComponent<NavMeshAgent>();
        Vector3 runDestination = blackboard.owner.transform.position + ((blackboard.owner.transform.position - playerObj.position));
        agent.SetDestination(runDestination);
        return NodeState.RUNNING;      
    }

    public override string getName()
    {
        return "EvadeFromPlayer";
    }

}
