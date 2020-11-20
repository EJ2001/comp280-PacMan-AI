using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BtController : MonoBehaviour
{
    private BtNode m_root;
    private Blackboard m_blackboard;

    //Blinky's references
    private MeshRenderer rend = null;
    private Color defColor;

    //Public method for the original wonder to pill method
    public BtNode wonderToPill(float speed)
    {
        BtNode isTargetSelected = new Sequence(new IsTargeting("pill"), new Inverter(new IsClose(1))); 
        BtNode stickyTarget = new Selector(isTargetSelected, new TargetRandom("pill")); 
        return new Sequence(stickyTarget, new TowardsTarget(speed));
    }

    // *Behaviour Trees for each ghost*

    // This is Blinky's unique behaviour that will go into rage mode and target the player after a certain score
    // is reached also with a rage time and a renderer which will hold the default color.
    protected BtNode createTreeBlinky()
    {
        BtNode RageMode = new Sequence(new IsRage(20, 6f, defColor),new IsTagClose(Mathf.Infinity, "Player"), new TowardsTarget(7f));
        BtNode chasePlayer = new Sequence(new IsTagClose(10, "Player"), new TowardsTarget(3.5f));
        return new Selector(chasePlayer, RageMode, wonderToPill(3.5f));
    }

    //Here is the behaviour tree for Pinky the behaviour I have created will allow Pinky to go invisible when it detects the player, Pinky also has a short distance to 
    //see the player. The invisiblity has a timer which will countdown when Blinky goes invisible.
    protected BtNode createTreePinky()
    {
        BtNode chasePlayer = new Sequence(new IsTagClose(5, "Player"), new TowardsTarget(3.5f));
        BtNode goInvisible = new Sequence(chasePlayer, new Invisible(3f));
        BtNode Detection = new Selector(goInvisible, chasePlayer);
        return new Selector(Detection, wonderToPill(3.5f));  
    }

    //Not finished behaviour yet
    protected BtNode createTreeInky()
    {
        //BtNode isPlayerInDistance = new Sequence(new TargetPlayer("Player"), new IsClose(4));
        BtNode chasePlayer = new Sequence(new IsTagClose(10, "Player"), new TowardsTarget(3.5f));
        return new Selector(chasePlayer, wonderToPill(3.5f));
    }

    //Not finished behaviour yet
    protected BtNode createTreeClyde()
    {
        BtNode chasePlayer = new Sequence(new IsTagClose(3, "Player"), new TowardsTarget(3.5f));
        return new Selector(chasePlayer, wonderToPill(3.5f));
    }


    // Start is called before the first frame update
    void Start()
    {
        if ( m_root == null) {
            //Here I created some if statements to check the names of the ghosts and create the individual trees
            if(this.gameObject.name == "Inky")
            { 
                m_root = createTreeInky(); 
            }
            else if(this.gameObject.name == "Pinky")
            {  
                m_root = createTreePinky();
            }
            else if(this.gameObject.name == "Blinky")
            { 
                rend = GetComponent<MeshRenderer>(); 
                defColor = rend.material.color;
                m_root = createTreeBlinky();
            }
            else if(this.gameObject.name == "Clyde")
            { 
                m_root = createTreeClyde(); 
            }
           
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
