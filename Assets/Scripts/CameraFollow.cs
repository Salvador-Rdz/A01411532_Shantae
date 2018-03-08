using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public Transform player;
    public Vector3 offset;
    public ShantaeController shantae;
    public float minX;
    public float maxX;
    private bool atLimits;
    // Use this for initialization
    void Start() {

    }

    //Follows the player, limiting the position of the camera and its boundaries
    void Update() {
        Vector3 newPos = new Vector3(player.position.x + offset.x, offset.y, offset.z);
        if (newPos.x > minX && newPos.x < maxX)
        {
            transform.position = newPos;
        }
        if (newPos.x >= maxX)
        {
            shantae.animator.Play("shantaeVictory");    
            shantae.isHit = true;
            StartCoroutine(wait(2f));
            /*shantae.animator.Play("shantaeRun");
            shantae.transform.position = Vector2.MoveTowards(shantae.transform.position, new Vector2(maxX, shantae.transform.position.y), 0.2f);
            */StartCoroutine(finishLevel());
        }
    }
    IEnumerator wait(float time)
    {
        yield return new WaitForSeconds(4f);
    }
    IEnumerator finishLevel()
    {
        yield return new WaitForSeconds(4f);
        LevelManager lm = GameObject.FindObjectOfType<LevelManager>();
        lm.LoadStart();
    }
}
