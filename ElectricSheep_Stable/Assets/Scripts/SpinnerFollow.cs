using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinnerFollow : MonoBehaviour
{
    private Color old;
    public float speed;
    public float turnSpeed;
    public float stoppingDistance;
    public float retreatDistance;
    private float timeBetweenShots;
    public float startTimeBetweenShots;
    public Sprite[] sprites;
    public float framesPerSecond;
    private SpriteRenderer spriteRenderer;
    private Transform target;
    private Vector3 moveDirection;
    private bool targetHit;
    private float attackWait;
    public float startAttackWait;
    private AudioSource audioSrc;
    public AudioClip swordSlash;

    public AudioClip hurtClip;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.spriteRenderer.enabled = true;
        targetHit = false;
        attackWait = startAttackWait;
        old = GetComponent<SpriteRenderer>().color;
        Physics2D.queriesStartInColliders = false;
        audioSrc = GetComponent<AudioSource>();
    }

    void movement()
    {
        animateWalking();
        if (!targetHit) {
            if (Vector3.Distance(transform.position, target.position) < 1f) {
                PlayerAttack.healthCount -= 1;
                targetHit = true;
                audioSrc.PlayOneShot(swordSlash);
                Hit(GameObject.FindGameObjectWithTag("Player"));
            }
            simpleFollow();
            
        }
        if (targetHit) {
            transform.position = Vector3.MoveTowards(transform.position, target.position, -speed*Time.deltaTime);
            attackWait -=Time.deltaTime;
            if (attackWait <= 0) {
                targetHit = false;
                attackWait = startAttackWait;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        animate();
        //advanceFollow();
        //simpleFollow();
        //shooting();

        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        RaycastHit2D sight = Physics2D.Raycast(transform.position, target.position - transform.position, 10000f);
        if (sight.collider != null) {
            Debug.DrawLine(transform.position, sight.point, Color.red);
            if (sight.collider.CompareTag("Player")  || (sight.collider.CompareTag("Enemy"))) {
                movement();
            }
            else {
                animateWalking();
            }
        }
    }
    void simpleFollow()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed*Time.deltaTime);
        animate();
    }

    void advanceFollow()
    {
        if (Vector3.Distance(transform.position, target.position) > stoppingDistance) {
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed*Time.deltaTime);
        } 
        else if ((Vector3.Distance(transform.position, target.position) < stoppingDistance) && 
        (Vector3.Distance(transform.position, target.position) > retreatDistance)) {
            transform.position = this.transform.position;
        }
        else if (Vector3.Distance(transform.position, target.position) < retreatDistance) {
            transform.position = Vector3.MoveTowards(transform.position, target.position, -speed*Time.deltaTime);

        }
        animate();
    }
    
    void animate() 
    {
        Vector3 currentPosition = transform.position;
        Vector3 moveToward = target.position;
        moveDirection = moveToward - currentPosition;
        moveDirection.z = 0; 
        moveDirection.Normalize();
        float targetAngle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg +90f;
        transform.rotation = Quaternion.Slerp( transform.rotation, Quaternion.Euler( 0, 0, targetAngle ), 
                                turnSpeed * Time.deltaTime );
    }

    void animateWalking()
    {
        int index = (int)(Time.timeSinceLevelLoad * framesPerSecond);
        index = index % sprites.Length;
        spriteRenderer.sprite = sprites[index];
    }

    public void Hit(GameObject other)
    {
        audioSrc.PlayOneShot(hurtClip);
        StartCoroutine(Flasher(other));

    }

    IEnumerator Flasher(GameObject other)
    {
        for (int i = 0; i < 2; i++)
        {
            other.GetComponent<SpriteRenderer>().color = Color.red;
            yield return new WaitForSeconds(.1f);
            other.GetComponent<SpriteRenderer>().color = old;
            yield return new WaitForSeconds(.1f);
        }
    }
}
