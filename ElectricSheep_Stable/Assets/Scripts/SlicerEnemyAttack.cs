using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlicerEnemyAttack : MonoBehaviour
{

    public Rigidbody2D rb;
    public float force = 1.0f;
    Transform playerPosition;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnCollisionEnter2D(Collision2D coll) 
    {
        if (coll.gameObject.tag == "Player") {
            PlayerAttack.healthCount -= 1;
        }

    }

    
}
