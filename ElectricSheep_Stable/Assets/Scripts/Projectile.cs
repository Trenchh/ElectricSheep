using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Projectile : MonoBehaviour
{
    public float speed;
    public TextMesh healthText;
    private Transform player;
    private Vector3 target;
    public GameObject explosion;
    private Vector3 directionMove;
    public Sprite[] sprites;
    public Sprite[] sprites2;
    public float framesPerSecond;
    public float framesPerSecond2;
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer spriteRenderer2;
    public GameObject projectile;
    private Vector3 moveDirection;
    public float turnSpeed;
    private Color old;
   


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        target = new Vector3(player.position.x, player.position.y);
        directionMove = (target - transform.position).normalized;
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.spriteRenderer.enabled = true;
        old = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>().color;
        

        Animate();
        //transform.LookAt(player);
    }

    // Update is called once per frame
    void Update()
    {
        int index = (int)(Time.timeSinceLevelLoad * framesPerSecond);
        index %= sprites.Length;
        spriteRenderer.sprite = sprites[index];
        transform.position += directionMove * speed * Time.deltaTime;
    }

    void DestoryProjectile()
    {
        Destroy(gameObject);
        //destroyAnimation();

    }

    void Animate()
    {
        Vector3 currentPosition = transform.position;
        Vector3 moveToward = target;
        moveDirection = moveToward - currentPosition;
        moveDirection.z = 0;
        moveDirection.Normalize();
        float targetAngle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg + 180f;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, targetAngle),
                                turnSpeed * Time.deltaTime);
    }

    //void destroyAnimation ()
    //{
    //    int index2 = (int)(Time.timeSinceLevelLoad * framesPerSecond2);
    //    index2 = index2 % sprites2.Length;
    //    spriteRenderer2.sprite = sprites2[index2];
    //    transform.position += directionMove * speed * Time.deltaTime;

    //}
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) {
            if (explosion != null)
            {
                GameObject xplo = Instantiate(explosion, transform.position, transform.rotation);
                Destroy(xplo, 0.2f);
            }
            other.GetComponent<PlayerMovement>().Hit(other);
            PlayerAttack.healthCount -= 1;
            DestoryProjectile();
        }
        if (other.CompareTag("Wall")) {
            if (explosion != null)
            {
                GameObject xplo = Instantiate(explosion, transform.position, transform.rotation);
                Destroy(xplo, 0.2f);
            }
            DestoryProjectile();
        }
        if (other.CompareTag("Enemy")) {
            ;
        }
    }
}
