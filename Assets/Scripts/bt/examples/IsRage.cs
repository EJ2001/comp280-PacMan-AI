using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsRage : BtNode
{
    public int pills_collected = 0;
    private int rage_limit = 0;
    private float time_ToRage = 0;
    private Color default_color;

    public IsRage(int pillsCollected, float timeToRage, Color colorDef){
        rage_limit = pillsCollected;
        time_ToRage = timeToRage;
        default_color = colorDef;
    }

    public override NodeState evaluate(Blackboard blackboard)
    {
        GameController game = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        MeshRenderer renderer = blackboard.owner.GetComponent<MeshRenderer>();
           
        if ( blackboard.target == null) {
            return NodeState.FAILURE;
        }

        if (rage_limit < game.score && time_ToRage > 0) {
            time_ToRage -= Time.deltaTime;      
            renderer.material.SetColor("_Color", Color.red);
            return NodeState.SUCCESS;
        } else {
            renderer.material.color = default_color;
            return NodeState.FAILURE;
        }
    }

    public override string getName()
    {
        return "isRage";
    }


}
