using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacmanController : MonoBehaviour
{
    public const float POWER_PILL_TIMER = 10;
    public const int PILL_SCORE = 10;
    public const int GHOST_SCORE = 100;
    public const float SPEED_FACTOR = 2.5F;

    private Rigidbody body;
    private GameController game;
    public float invulnTimer;

    [SerializeField] Transform startPos;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        game = GameObject.Find("GameController").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (invulnTimer > 0)
        {
            invulnTimer -= Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        float xSpeed = Input.GetAxis("Horizontal");
        float ySpeed = Input.GetAxis("Vertical");
        Vector3 vel = new Vector3(xSpeed, 0, ySpeed) * SPEED_FACTOR;

        body.AddForce(vel);
    }

    public bool isPowered()
    {
        return invulnTimer > 0;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "ghost") {
            if (isPowered())
            {
                game.score += GHOST_SCORE;
                Destroy(col.gameObject);
            } else if(game.lives > 0){
                Debug.Log("test");
                game.lives--;
                this.gameObject.transform.position = startPos.position;
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                //Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "pill")
        {
            game.score += PILL_SCORE;
            Destroy(col.gameObject);
        }
        else if (col.gameObject.tag == "powerpill")
        {
            invulnTimer = POWER_PILL_TIMER;
            Destroy(col.gameObject);
        }
    }
}
