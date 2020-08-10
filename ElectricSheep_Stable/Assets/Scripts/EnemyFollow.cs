using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public float speed;
    public float turnSpeed;
    public float stoppingDistance;
    public float retreatDistance;
    private float timeBetweenShots;
    public float startTimeBetweenShots;
    public Sprite[] sprites;
    public float framesPerSecond;
    private SpriteRenderer spriteRenderer;
    public GameObject firePoint;
    private AudioSource audioSrc;
    public AudioClip bulletShot;

    private Transform target;
    public GameObject projectile;
    private Vector3 moveDirection;
    private bool playerClose;
    

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.queriesStartInColliders = false;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        startTimeBetweenShots = Random.Range(0f,1.5f);
        timeBetweenShots = startTimeBetweenShots;
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.spriteRenderer.enabled = true;
        audioSrc = GetComponent<AudioSource>();
        playerClose = false;
    }

    // Update is called once per frame
    void Update()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        RaycastHit2D sight = Physics2D.Raycast(transform.position, target.position - transform.position, 10000f);
        if (sight.collider != null) {
            Debug.DrawLine(transform.position, sight.point, Color.red);
            if (sight.collider.CompareTag("Player")) {
                AdvanceFollow();
                Animate();
                //simpleFollow();
                Shooting();
                AnimateWalking();
            }
            else {
                AnimateWalking();
            }
        }

        /*
        animateWalking();
        if (playerClose == true) {
            advanceFollow();
            animate();
            //simpleFollow();
            shooting();
        }
        
        if (Vector3.Distance(transform.position, target.position) > 8f) {
            playerClose = false;
        } else {
            playerClose = true;
        }
        */
        
    


    }
    void SimpleFollow()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed*Time.deltaTime);
        Animate();
    }

    void AdvanceFollow()
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
        Animate();
    }
    
    void Animate() 
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
    void Shooting() {
        if (timeBetweenShots <= 0) {
            Instantiate(projectile, firePoint.GetComponent<Transform>().position, Quaternion.identity);
            audioSrc.PlayOneShot(bulletShot);
            timeBetweenShots = Random.Range(0f,2f);
        }
        else {
            timeBetweenShots -= Time.deltaTime; 
        }
    }

    void AnimateWalking()
    {
        int index = (int)(Time.timeSinceLevelLoad * framesPerSecond);
        index %= sprites.Length;
        spriteRenderer.sprite = sprites[index];
    }

}
    
    

