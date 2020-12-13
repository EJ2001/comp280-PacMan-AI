using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsInkyHiding : BtNode
{
    GameObject inkyObj;

    // Contructor for holding Inky's reference object
    public IsInkyHiding(GameObject inky)
    {
        inkyObj = inky;
    }

    // Quick check if Inky is hiding which will return success if so
    public override NodeState evaluate(Blackboard blackboard)
    {
        bool hidingCheck = inkyObj.transform.GetComponent<BtController>().isHiding;

        if(hidingCheck)
        {
            return NodeState.SUCCESS;
        }     
        else
        {
            return NodeState.FAILURE;
        }   
    }

    public override string getName()
    {
        return "IsInkyHiding";
    }
}
