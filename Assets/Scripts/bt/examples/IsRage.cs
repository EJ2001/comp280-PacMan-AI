using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsRage : BtNode
{
    public int pills_collected = 0;
    private int rage_limit = 0;
    private float time_ToRage = 0;
    private Color default_color;

    //Holds the variables for the pills collected, time for raging and the colour
    public IsRage(int pillsCollected, float timeToRage, Color colorDef){
        rage_limit = pillsCollected;
        time_ToRage = timeToRage;
        default_color = colorDef;
    }

    public override NodeState evaluate(Blackboard blackboard)
    {
        //References for the game controller script and the mesh renderer
        GameController game = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        MeshRenderer renderer = blackboard.owner.GetComponent<MeshRenderer>();
        
        // starts an if statement to evaluate if the blackboard target is null and if it is it will return failure
        if ( blackboard.target == null)
        {
            renderer.material.color = default_color;
            return NodeState.FAILURE;
        }

        // if statement which will check if the game score is greater than the rage limit and checks if the timer is
        // still going
        if (rage_limit < game.score && time_ToRage > 0)
        {
            // creates the countdown on the rage time
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
        return "IsRage";
    }


}
