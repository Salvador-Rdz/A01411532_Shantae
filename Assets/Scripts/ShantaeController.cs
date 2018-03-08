using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShantaeController : MonoBehaviour {
    public float jumpForce;
    public Vector3 currentSpeed;
    public float hp = 250;
    public HealthBar hpBar;
    private Rigidbody2D rb;
    public Animator animator;
    private BoxCollider2D bCol;
    public AudioManager adm;
    private int direction = 1;
    private float speed = 10f;
    public bool isHit = false;
    private SpriteRenderer sp;
    private LevelManager lm;
    // Use this for initialization
    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        bCol = GetComponent<BoxCollider2D>();
        sp = gameObject.GetComponent<SpriteRenderer>();
        lm = GameObject.FindObjectOfType<LevelManager>();
        if (!adm) adm = GameObject.FindObjectOfType<AudioManager>();
        hpBar.setTotalHP(hp);
    }
    private void Update()
    {
        if (isHit==false)
        {
            currentSpeed = rb.velocity;
            if (!Input.anyKey)
            {
                animator.SetBool("isAttacking", false);
                animator.SetBool("isCrouching", false);
                animator.SetBool("isMoving", false);
            }
            else
            {
                if (Input.GetKey(KeyCode.Z))
                {
                    animator.SetBool("isAttacking", true);
                    if (checkIfAttacking())
                    {
                        adm.playOnce("p_attack");
                    }
                }
                else
                {
                    animator.SetBool("isAttacking", false);
                }
                if (Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow) && animator.GetBool("isGrounded"))
                {
                    animator.SetBool("isGrounded", false);
                    adm.playOnce("s_land");
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                }
                if ((!Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow)) || (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow)))
                {
                    if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.RightArrow) && !checkIfAttacking())
                    {
                        if (animator.GetBool("isGrounded")) animator.SetBool("isAttacking", false);
                        if (!Input.GetKey(KeyCode.Z) && !animator.GetBool("isAttacking"))
                        {
                            animator.SetBool("isMoving", true);
                            animator.SetBool("isAttacking", false);
                            direction = -1;
                            Flip();
                            transform.position += Vector3.left * speed * Time.deltaTime;
                        }
                        else
                        {
                            animator.SetBool("isMoving", false);
                            animator.SetBool("isAttacking", true);
                        }
                    }
                    if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.LeftArrow) && !checkIfAttacking())
                    {
                        if (animator.GetBool("isGrounded")) animator.SetBool("isAttacking", false);
                        if (!Input.GetKey(KeyCode.Z) && !animator.GetBool("isAttacking"))
                        {
                            animator.SetBool("isMoving", true);
                            animator.SetBool("isAttacking", false);
                            direction = 1;
                            Flip();
                            transform.position += Vector3.right * speed * Time.deltaTime;
                        }
                        else
                        {
                            animator.SetBool("isMoving", false);
                            animator.SetBool("isAttacking", true);
                        }

                    }
                }
                else
                {
                    animator.SetBool("isMoving", false);
                }
                if (Input.GetKeyUp(KeyCode.DownArrow))
                {
                    animator.SetBool("isCrouching", false);
                }
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    animator.SetBool("isCrouching", true);
                    animator.SetBool("isAttacking", false);
                    if (Input.GetKey(KeyCode.Z))
                    {
                        animator.SetBool("isAttacking", true);
                    }
                    else if (Input.GetKey(KeyCode.LeftArrow) && !checkIfAttacking())
                    {
                        animator.SetBool("isMoving", true);
                        animator.SetBool("isAttacking", false);
                        direction = -1;
                        Flip();
                        transform.position += Vector3.left * speed * 0.2f * Time.deltaTime;
                    }
                    else if (Input.GetKey(KeyCode.RightArrow) && !checkIfAttacking())
                    {
                        animator.SetBool("isMoving", true);
                        animator.SetBool("isAttacking", false);
                        direction = 1;
                        Flip();
                        transform.position += Vector3.right * speed * 0.2f * Time.deltaTime;
                    }
                    else
                    {
                        animator.SetBool("isAttacking", false);
                        animator.SetBool("isMoving", false);
                    }
                }
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor" && isHit==false)
        {
            animator.SetBool("isGrounded", true);
        }
        if (collision.gameObject.tag == "Enemy")
        {
            takeDamage(collision.gameObject.GetComponent<Enemy>().getDmg());
        }
    }
    //Flips the sprite, just used to change direction.
    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x = direction;
        transform.localScale = scale;

    }
    //Useful to manage the smoothness inbetween the attacks completing and the character moving.
    bool checkIfAttacking()
    {
        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("shantaeIdleAttack"))
        {
            return true;
        }
        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("shantaeDuckAttack"))
        {
            return true;
        }
        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("shantaeJumpAttack"))
        {
            return true;
        }
        return false;
    } 
    //controler for taking damage and updating hits, its called from the source.
    public void takeDamage(float damage)
    {
        isHit = true;
        StartCoroutine(flashHit());
        StartCoroutine(Invulnerability());
        if(direction == 1) rb.velocity = new Vector2(-5f, 10f);
        if(direction == -1) rb.velocity = new Vector2(5f, 10f);
        this.hp -= damage;
        hpBar.TakeDamage(damage);
    }
    IEnumerator Invulnerability()
    {
        adm.playOnce("s_pain");
        animator.Play("shantaeIdleHit");      
        bCol.enabled = false;
        yield return new WaitForSeconds(1f);
        if(hp<=0)
        {
            lm.LoadLevel("GameOver");
        }
        isHit = false;
        bCol.enabled = true;
        animator.enabled = true;
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
}
