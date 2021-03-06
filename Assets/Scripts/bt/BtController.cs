using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BtController : MonoBehaviour
{
    //All ghosts
    private float ghostSpeed;
    private BtNode m_root;
    private Blackboard m_blackboard;
    [SerializeField] GameObject player;
    

    //Blinky's references
    private MeshRenderer rend = null;
    private Color defColor;

    //Inky's references
    public bool isHiding = false;
    [SerializeField] Mesh pill_mesh = null;
    [SerializeField] Material pill_mat = null;
    private Mesh ghost_mesh = null;
    private Material ghost_mat = null;

    //Clyde's references
    [SerializeField] Transform inkyPos = null;
    [SerializeField] GameObject inky = null;

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
        ghostSpeed = 3.3f;
        float rageSpeed = 5f;
        int pillScoreToReach = 20;
        float TimeToRage = 6f;
        
        BtNode RageMode = new Sequence(new IsRage(pillScoreToReach, TimeToRage, defColor),new IsTagClose(Mathf.Infinity, "Player"), new TowardsTarget(rageSpeed));
        BtNode chasePlayer = new Sequence(new IsTagClose(10, "Player"), new TowardsTarget(ghostSpeed));
        BtNode avoidPlayer = new Sequence(new IsTagClose(10, "Player"), new IsPillActive(player), new EvadeFromPlayer(player));
        return new Selector(avoidPlayer, chasePlayer, RageMode, wonderToPill(ghostSpeed));
    }

    //Here is the behaviour tree for Pinky the behaviour I have created will allow Pinky to go invisible when it detects the player, Pinky also has a short distance to 
    //see the player. The invisiblity has a timer which will countdown when Blinky goes invisible.
    protected BtNode createTreePinky()
    {
        ghostSpeed = 3.3f;
        float invisibleTime = 3f;

        BtNode chasePlayer = new Sequence(new IsTagClose(10, "Player"), new TowardsTarget(ghostSpeed));
        BtNode goInvisible = new Sequence(chasePlayer, new Invisible(invisibleTime));
        BtNode Detection = new Selector(goInvisible, chasePlayer);
        BtNode avoidPlayer = new Sequence(new IsTagClose(10, "Player"), new IsPillActive(player), new EvadeFromPlayer(player));
        return new Selector(avoidPlayer, Detection, wonderToPill(ghostSpeed));  
    }

    //The behaviour of Inky will include a trap that will be created when Inky is near the player, Inky will camouflage into a pill and create a suprise attack when the player
    // is very close
    protected BtNode createTreeInky()
    {
        ghostSpeed = 3.3f;
        float distanceToAppear = 5.7f;
        float distanceToTrap = 10.5f;

        BtNode chasePlayer = new Sequence(new IsTagClose(12, "Player"), new TowardsTarget(ghostSpeed));
        BtNode createTrap = new Sequence(new IsTagClose(distanceToTrap, "Player"), new CreateTrap(distanceToAppear, pill_mesh, ghost_mesh, pill_mat, ghost_mat), chasePlayer);
        BtNode avoidPlayer = new Sequence(new IsTagClose(10, "Player"), new IsPillActive(player), new EvadeFromPlayer(player));
        return new Selector(avoidPlayer, createTrap, wonderToPill(ghostSpeed));
    }

    //Clyde is the only ghost that won't try to kill the player when seen, Clyde will wait to synergise with Inky and will 
    //chase the player to Inky when Inky is in the pill state
    protected BtNode createTreeClyde()
    {
        ghostSpeed = 3.5f;
        
        BtNode movementToPlayer = new Sequence(new IsTagClose(Mathf.Infinity, "Player"), new TowardsTarget(ghostSpeed));
        BtNode chaseToTrap = new Sequence(new IsInkyHiding(inky), new DeterminePath(player.transform, inkyPos));
        BtNode avoidPlayer = new Sequence(new IsTagClose(10, "Player"), new IsPillActive(player), new EvadeFromPlayer(player));
        return new Selector(avoidPlayer, chaseToTrap, wonderToPill(ghostSpeed));
    }


    // Start is called before the first frame update
    void Start()
    {
        if ( m_root == null) {

            //Here I created some if statements to check the names of the ghosts and create the individual trees
            if(this.gameObject.name == "Blinky")
            { 
                rend = GetComponent<MeshRenderer>(); 
                defColor = rend.material.color;
                m_root = createTreeBlinky();
            }
            else if(this.gameObject.name == "Inky")
            { 
                ghost_mat = GetComponent<Renderer>().material;
                ghost_mesh = GetComponent<MeshFilter>().mesh;
                m_root = createTreeInky(); 
            }
            else if(this.gameObject.name == "Pinky")
            {  
                m_root = createTreePinky();
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
