using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtController : MonoBehaviour
{
    private BtNode m_root;
    private Blackboard m_blackboard;

    //Public method for the original wonder to pill 
    public BtNode wonderToPill()
    {
        BtNode isTargetSelected = new Sequence(new IsTargeting("pill"), new Inverter(new IsClose(1))); 
        BtNode stickyTarget = new Selector(isTargetSelected, new TargetRandom("pill")); 
        return new Sequence(stickyTarget, new TowardsTarget());
    }

    // method to create the tree, sorry - no GUI for this we need to build it by hand
    protected BtNode createTreeInky()
    {
        Debug.Log("Inky");
        //BtNode isPlayerInDistance = new Sequence(new TargetPlayer("Player"), new IsClose(4));
        BtNode chasePlayer = new Sequence(new IsTagClose(10, "Player"), new TowardsTarget());
        return new Selector(chasePlayer, wonderToPill());
    }
    protected BtNode createTreePinky()
    {
        Debug.Log("Pinky");
         BtNode chasePlayer = new Sequence(new IsTagClose(10, "Player"), new TowardsTarget());
        return new Selector(chasePlayer, wonderToPill());
    }

    // Blinky's behaviour consists of constantly chasing the player with an unlimited
    protected BtNode createTreeBlinky()
    {
        Debug.Log("Blinky");
        BtNode RageMode = new Sequence(new IsRage(2),new IsTagClose(30, "Player"), new TowardsTarget());
        BtNode chasePlayer = new Sequence(new IsTagClose(10, "Player"), new TowardsTarget());
        return new Selector(chasePlayer, RageMode, wonderToPill());
    }
    protected BtNode createTreeClyde()
    {
        Debug.Log("Clyde");
        BtNode chasePlayer = new Sequence(new IsTagClose(3, "Player"), new TowardsTarget());
        return new Selector(chasePlayer, wonderToPill());
    }


    // Start is called before the first frame update
    void Start() {
        if ( m_root == null) {

            if(this.gameObject.name == "Inky")  { m_root = createTreeInky(); }
            else if(this.gameObject.name == "Pinky")  {  m_root = createTreePinky();  }
            else if(this.gameObject.name == "Blinky")  { m_root = createTreeBlinky(); }
            else if(this.gameObject.name == "Clyde") { m_root = createTreeClyde(); }
           
            m_blackboard = new Blackboard();
            m_blackboard.owner = gameObject;
        }
    }

    // Update is called once per frame
    void Update() {
        NodeState result = m_root.evaluate(m_blackboard);
        if ( result != NodeState.RUNNING ) {
            m_root.reset();
        }
    }
}
