using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsPillActive : BtNode
{
    private GameObject playerObj;

    // Contrusctor for holding player object
    public IsPillActive(GameObject player)
    {
        playerObj = player.gameObject;
    }

    // Method to check if the pill is powered 
    public override NodeState evaluate(Blackboard blackboard) {
        if ( blackboard.target == null) {
            return NodeState.FAILURE;
        }

        bool isActive = playerObj.transform.GetComponent<PacmanController>().isPowered();
        if (isActive) {
            return NodeState.SUCCESS;
        } else {
            return NodeState.FAILURE;
        }
    }

    public override string getName()
    {
        return "EscapePlayer";
    }

}
