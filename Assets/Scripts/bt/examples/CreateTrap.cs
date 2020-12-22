using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreateTrap : BtNode
{
    private float time_ToHide;
    private float distance_ToAppear;
    private Mesh pill_Mesh;
    private Mesh def_mesh;
    private Material ghost_material;
    private Material pill_material;
    private NavMeshAgent m_agent;

    
    // Constructor for the node which holds the references needed
    public CreateTrap(float distanceToAppear, Mesh pill, Mesh defaultMesh, Material pillMaterial, Material defMat)
    {
        distance_ToAppear = distanceToAppear;
        pill_Mesh = pill;
        def_mesh = defaultMesh;
        pill_material = pillMaterial;   
        ghost_material = defMat;
    }

    public override NodeState evaluate(Blackboard blackboard)
    {
        if (m_agent == null) {
            m_agent = blackboard.owner.GetComponent<NavMeshAgent>();
        }
        MeshFilter ghost_mesh = blackboard.owner.GetComponent<MeshFilter>();
        MeshRenderer rend = blackboard.owner.GetComponent<MeshRenderer>();

        //Do nothing if there isn't a target
        if(blackboard.target == null) { return NodeState.FAILURE; }

        //Start hiding
        blackboard.owner.gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        rend.material = pill_material;
        ghost_mesh.mesh = pill_Mesh;
        m_agent.isStopped = true;
        blackboard.owner.GetComponent<BtController>().isHiding = true;

        float distance = (blackboard.owner.transform.position - blackboard.target.transform.position).magnitude;
        if(distance <= distance_ToAppear)
        {
            //Appear
            blackboard.owner.gameObject.transform.localScale = new Vector3(1, 1, 1);
            rend.material = ghost_material;
            ghost_mesh.mesh = def_mesh;
            m_agent.isStopped = false;
            blackboard.owner.GetComponent<BtController>().isHiding = false;
            return NodeState.SUCCESS;
        }
           
        return NodeState.RUNNING;
        
    }

    public override string getName()
    {
        return "CreateTrap";
    }
}
