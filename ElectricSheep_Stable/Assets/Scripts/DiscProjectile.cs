using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiscProjectile : MonoBehaviour
{
    public float speed;
    public Text healthText;
    //private Transform turret;
    //private Vector3 target;
    public GameObject explosion;
    private Vector3 directionMove;
    public Sprite[] sprites;
    public float framesPerSecond;
    private SpriteRenderer spriteRenderer;
    public GameObject projectile;
    private Vector3 moveDirection;
    public float turnSpeed;

    // Start is called before the first frame update
    void Start()
    {
        //turret = GameObject.FindGameObjectWithTag("Turret").transform;
        //target = new Vector3(turret.position.x, turret.position.y);
        directionMove = new Vector3(transform.position.x - 1, transform.position.x - 1);
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.spriteRenderer.enabled = true;
        Animate();
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

    }

    void Animate()
    {
        Vector3 currentPosition = transform.position;
        moveDirection = currentPosition;
        moveDirection.z = 0;
        moveDirection.Normalize();
        float targetAngle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg + 180f;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, targetAngle),
                                turnSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //Instantiate(explosion, transform.position, transform.rotation);
            //Destroy(GameObject.FindGameObjectWithTag("Explosion"));
            DestoryProjectile();
            PlayerAttack.healthCount -= 1;
        }
        if (other.CompareTag("Wall"))
        {
            DestoryProjectile();
        }
        if (other.CompareTag("Enemy"))
        {
            ;
        }
    }
}
