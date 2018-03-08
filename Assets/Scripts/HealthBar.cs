using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour {
    public float TotalHp;
    public float CurrentHp;
    //updates the a simple green graphic against a frame backdrop depending on the percentages given to it in setTotalHP.
    //primitive hp bar, but it gets the work done.
    public void TakeDamage(float damage)
    {
        CurrentHp -= damage;
        if(CurrentHp<0) transform.localScale = new Vector3(0, 1, 1);
        else transform.localScale = new Vector3((CurrentHp / TotalHp), 1, 1);
    }
    public void setTotalHP(float hp)
    {
        TotalHp = hp;
        CurrentHp = hp;
    }
}
