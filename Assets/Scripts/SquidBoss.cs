using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SquidBoss : MonoBehaviour {
    //Control variables for stats and States has similar workings to Enemy
    public int hp = 100;
    public float speed = 0.001f;
    public float jumpSpeed = 1f;
    public int damage = 10;
    public float direction = 1;
    public bool aiIsOn = true;
    public bool Flips = false;
    public bool isDead = false;
    private bool moveUp = true;
    //Enumerator used to manage boss AI
    public enum BossActions
    {
        Idle,
        Patrolling,
        Jumping,
        Falling
    }
    private BossActions eCurState = BossActions.Idle;
    //Used for positioning and moving.
    private Vector3 patrolA;
    public Vector3 patrolB;
    public float jumpHeight;
    //References to objects, used for visual controlling.
    public Transform player;
    public GameObject smoke;
    public static GameObject popupText;
    public static GameObject canvas;
    private SpriteRenderer sp;
    private Animator anm;
    // Use this for initialization
    void Start()
    {
        sp = gameObject.GetComponent<SpriteRenderer>();
        anm = gameObject.GetComponent<Animator>();
        patrolA = transform.position;
        eCurState = BossActions.Patrolling;
    }

    // [Update used to manage what the enemy is doing, uses the enumarator to parse the current state.
    void Update()
    {
        if (!isDead)
        {
            switch (eCurState)
            {
                case BossActions.Idle:
                    anm.SetBool("isGrounded", true);
                    anm.SetBool("isMoving", false);
                    Idle();
                    break;
                case BossActions.Patrolling:
                    anm.SetBool("isGrounded", true);
                    anm.SetBool("isMoving", true);
                    Patrol();
                    break;
                case BossActions.Jumping:
                    anm.SetBool("isGrounded", false);
                    Jump();
                    break;
                case BossActions.Falling:
                    anm.SetBool("isGrounded", false);
                    Fall();

                    break;
            }
        }
    }
    //If it has finished whatever it was doing, it rerolls and does something else.
    void Idle()
    {
        eCurState = (BossActions)Random.Range(0, 3);
    }
    //Jumps up then triggers MoveAround
    void Jump()
    {
        anm.Play("Jump");
        if(transform.position.y <= jumpHeight)
        {
            Vector3 jumpPos = new Vector3(transform.position.x, transform.position.y + jumpHeight);
            transform.position = Vector2.MoveTowards(transform.position, jumpPos, jumpSpeed);
        }
        else
        {
            MoveAround();
        }
    }
    //Gets close to the x value of the player
    void MoveAround()
    {
        transform.position = new Vector2(player.position.x + Random.Range(0,2f),transform.position.y);
        eCurState = BossActions.Falling;
    }
    //And drops on them, dealing damage.
    void Fall()
    {
        anm.Play("Fall");
       Vector3 fallPos = new Vector3(transform.position.x, -2);
       transform.position = Vector2.MoveTowards(transform.position, fallPos, jumpSpeed*0.5f);
        if(transform.position.y <=-2)
        {
            anm.Play("FallRecover");
            eCurState = BossActions.Idle;
            
        }
    }
    //Moves from left to right, can do whatever when it reaches a point of its patrol.
    void Patrol()
    {
        anm.SetBool("isMoving", true);
        if (patrolA.x == transform.position.x)
        {
            moveUp = true;
            if (Flips)
            {
                direction = -direction;
                Flip();
                eCurState = BossActions.Idle;
            }

        }
        if (patrolB.x == transform.position.x)
        {
            moveUp = false;
            if (Flips)
            {
                direction = -direction;
                Flip();
                eCurState = BossActions.Idle;
            }
        }
        if (moveUp)
        {
            transform.position = Vector2.MoveTowards(transform.position, patrolB, speed);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, patrolA, speed);
        }
    }
    //Flips the sprite, just used to change direction.
    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x = direction;
        transform.localScale = scale;

    }

    public int getDmg()
    {
        return damage;
    }
    //controler for taking damage and updating hits, its called from the source.
    public void TakeDamage(int damage)
    {
        StartCoroutine(flashHit());
        this.hp -= damage;
        if (hp <= 0)
        {
            Die();
        }
    }
    //Manages the victory state, smoke and audio.
    public void Die()
    {
        Smoke();
        isDead = true;
        player.GetComponent<ShantaeController>().isHit = true;
        player.GetComponent<Animator>().Play("shantaeVictory");
        StartCoroutine(finishLevel());
    }
    void Smoke()
    {
        GameObject smokePuff = Instantiate(smoke, transform.position, Quaternion.identity);
        ParticleSystem ps = smokePuff.GetComponent<ParticleSystem>();
        var main = ps.main;
    }
    IEnumerator finishLevel()
    {
        gameObject.GetComponent<AudioSource>().enabled = false;
        GameObject.FindObjectOfType<AudioManager>().Play("shantae_victory");
        yield return new WaitForSeconds(4f);
        LevelManager lm = GameObject.FindObjectOfType<LevelManager>();
        lm.LoadStart();
    }
    //flashes three times, disabling the renderer to make the sprite invisible.
    IEnumerator flashHit()
    {
        for (int i = 0; i < 3; i++)
        {
            sp.enabled = false;
            yield return new WaitForSeconds(0.1f);
            sp.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
    }
    IEnumerator wait(float secs)
    {
        yield return new WaitForSeconds(secs);
        eCurState = BossActions.Idle;
    }
}
