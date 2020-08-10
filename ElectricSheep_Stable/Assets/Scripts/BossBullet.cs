using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{

    public Sprite[] sprites;
    private SpriteRenderer spriteRenderer;
    public float speed;
    private Vector3 directionMove;
    public Rigidbody2D rb;
    public float bulForce;
    public GameObject explosion;

    // Start is called before the first frame update
    void Start()
    {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.spriteRenderer.enabled = true;
        directionMove = transform.forward;
        rb = GetComponent<Rigidbody2D>();
        //bulForce = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        //rb.AddForce(transform.up * bulForce, ForceMode2D.Impulse);
        //transform.position += directionMove * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject xplo = Instantiate(explosion, transform.position, transform.rotation);
            Destroy(xplo, 0.2f);
            other.GetComponent<PlayerMovement>().Hit(other);
            PlayerAttack.healthCount -= 1;
            DestoryProjectile();
        }
        if (other.CompareTag("Wall"))
        {
            GameObject xplo = Instantiate(explosion, transform.position, transform.rotation);
            Destroy(xplo, 0.2f);
            DestoryProjectile();
        }
        if (other.CompareTag("Boss"))
        {
            //Physics.IgnoreCollision(GameObject.FindGameObjectWithTag("Boss").GetComponent<Collider2D>(), GetComponent<Collider2D>());
            ;
        }
    }

    void DestoryProjectile()
    {
        Destroy(gameObject);
        //destroyAnimation();

    }



}
