using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {
    public float hp = 100;
    public float speed = 5f;
    public float damage = 10f;
    public bool Flips = false;
    public float direction = 1;
    private Vector3 patrolA;
    public Vector3 patrolB;
    public static GameObject popupText;
    public static GameObject canvas;
    private bool moveUp = true;
    private SpriteRenderer sp;
	// Use this for initialization
	void Start () {
        sp = gameObject.GetComponent<SpriteRenderer>();
        patrolA = transform.position;
	}

    // Update is called once per frame
    void Update()
    {
        if (patrolA == transform.position)
        {
            moveUp = true;
            if (Flips)
            {
                direction = -direction;
                Flip();
            }
            
        }
        if (patrolB == transform.position)
        {
            moveUp = false;
            if (Flips)
            {
                direction = -direction;
                Flip();
            }
        }
        if(moveUp)
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

    public float getDmg()
    {
        return damage;
    }
    //controler for taking damage and updating hits, its called from the source.
    public void TakeDamage(float damage)
    {
        StartCoroutine(flashHit());
        this.hp -= damage;
        if (hp <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        Destroy(gameObject);
    }
    //flashes three times, disabling the renderer to make the sprite invisible.
    IEnumerator flashHit()
    {
        for(int i =0;i<3;i++)
        {
            sp.enabled = false;
            yield return new WaitForSeconds(0.1f);
            sp.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
