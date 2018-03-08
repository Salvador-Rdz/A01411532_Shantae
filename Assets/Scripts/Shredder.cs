using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shredder : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D collider)
    {
        print("something entered");
        if(collider.gameObject.tag == "Shantae" || collider.gameObject.tag == "Attack")
        {
            collider.GetComponent<ShantaeController>().takeDamage(250);
        }
        else
        {
            Destroy(collider.gameObject);
        }
    }
}
