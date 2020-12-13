using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DeterminePath : BtNode
{
    private float distanceToPlayer;
    private float distanceToInky;
    Transform _player;
    Transform _inky;

    // Constructor for holding variables
    public DeterminePath(Transform player, Transform inky)
    {
        _player = player;
        _inky = inky;       
    }

    // This method will hold the distance from Clyde to the Player and will create a vector3 direction which will add an offset to the position
    // this basically gets the position from inky and the player and adds an offset which to push the player to Inky and sets Clyde to that direction.
    public override NodeState evaluate(Blackboard blackboard)
    {
        Transform clyde = blackboard.owner.transform;
        float distFromClydeToPlayer = Vector3.Distance(_player.position, clyde.position);

        //Determine direction for where clyde should path and then set the destination to that direction on the agent
        float offsetLength = -2.5f;
        Vector3 direction = (_inky.position - _player.position).normalized * offsetLength + _player.position;
        blackboard.owner.transform.GetComponent<NavMeshAgent>().SetDestination(direction);

        
        if(distFromClydeToPlayer < 2)
        {
            blackboard.owner.transform.GetComponent<NavMeshAgent>().SetDestination(_player.position);
            return NodeState.SUCCESS;
        }
        
        return NodeState.RUNNING;
     
    }

    public override string getName()
    {
        return "DeterminePath";
    }
}
