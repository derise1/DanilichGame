using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random=UnityEngine.Random;

public class registerHit : MonoBehaviour
{
    [HideInInspector] public int damage;
    public bool isHitPlayer = false;

    void OnCollisionEnter(Collision col)
    {
        //If we (the bullet) hit the col object check for Player tag
        if (col.transform.tag == "Player")
        {
            Destroy(gameObject);
            Debug.Log("Hit an player");
            //If the root object we hit has a healthcontroller then apply damage
            if (col.transform.root.gameObject.GetComponent<Health>())
            {
                damage = Random.Range(11, 13);
                col.transform.root.gameObject.GetComponent<Health>().TakeDamage(damage);
            }
        }
        else
        {
            Destroy(gameObject);
        }

        //Finally, destroy us (the bullet)
        Destroy(gameObject);
    }
}
