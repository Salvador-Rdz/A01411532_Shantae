using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryLine : MonoBehaviour {
    public ShantaeController shantae;
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Shantae")
        {
            shantae.animator.Play("shantaeVictory");
            shantae.animator.enabled = false;
            shantae.enabled = false;
            wait(2f);
            LevelManager lm = new LevelManager();
            lm.LoadStart();
        }
    }
    IEnumerator wait(float time)
    {
        yield return new WaitForSeconds(time);
    }
}
