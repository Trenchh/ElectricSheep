using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlicerBounce : MonoBehaviour
{
    private Rigidbody2D rb;
    private Transform target;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D coll) 
    {
        if (coll.gameObject.tag == "Swipe") {
            Vector2 knockbackPosition = transform.position + (transform.position - target.position).normalized ;
            rb.MovePosition(Vector2.MoveTowards(transform.position, knockbackPosition, Time.deltaTime));
        }

    }
}
