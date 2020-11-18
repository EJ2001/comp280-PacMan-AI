using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsRage : BtNode
{
    public int pills_collected = 0;
    private int rage_limit = 10;

    public IsRage(int pillsCollected){
        rage_limit = pillsCollected;
    }

    public override NodeState evaluate(Blackboard blackboard)
    {
        GameController game = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        if ( blackboard.target == null) {
            return NodeState.FAILURE;
        }
        //float distance = (blackboard.owner.transform.position - blackboard.target.transform.position).magnitude;
        if (rage_limit < game.score) {
            return NodeState.SUCCESS;
        } else {
            return NodeState.FAILURE;
        }
    }

    public override string getName()
    {
        return "isRage";
    }

}
