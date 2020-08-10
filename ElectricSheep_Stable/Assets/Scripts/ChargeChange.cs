using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeChange : MonoBehaviour
{

    private AudioSource audioSrc;
    public AudioClip chargeSpawn;

    void Start()
    {
        audioSrc = GetComponent<AudioSource>();
        audioSrc.PlayOneShot(chargeSpawn);
    }




    void DestroyCharge()
    {
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D coll) 
    {
        if (coll != null)
        {
            if (coll.gameObject.CompareTag("Player"))
            {
                DestroyCharge();
                PlayerAttack.healthCount += 1;
                GameObject.FindObjectOfType<PlayerAttack>().healthMax += 1;
                coll.gameObject.GetComponent<PlayerMovement>().ChargePickUp();
            }
            if (coll.gameObject.CompareTag("Enemy"))
            {
                Physics2D.IgnoreCollision(coll.otherCollider, GetComponent<Collider2D>());
            }
        }
    }
}
