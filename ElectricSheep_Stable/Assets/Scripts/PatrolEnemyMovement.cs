using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class PatrolEnemyMovement : MonoBehaviour
{

    public float speed;
    public float turnSpeed;
    private float timeBetweenShots;
    public float chaseDistance;
    public float returnDistance;
    public float startTimeBetweenShots;
    public Sprite[] sprites;
    public float framesPerSecond;
    private SpriteRenderer spriteRenderer;
    public Vector3 targetPatrol1;
    public Vector3 targetPatrol2;
    private Vector3 currentTarget;
    private Transform player;
    private Vector3 moveDirection;
    private int current = 0;
    public GameObject projectile;
    public bool chase;
    private bool done;
    private AudioSource audioSrc;
    public AudioClip bulletShot;
    private Rigidbody2D rb;
    

    // Start is called before the first frame update
    void Start()
    {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.spriteRenderer.enabled = true;
        done = false;
        audioSrc = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        timeBetweenShots = startTimeBetweenShots;
        chase = false;
        Physics2D.queriesStartInColliders = false;
        rb = GetComponent<Rigidbody2D>();
       
    }

    // Update is called once per frame
    void Update()
    {
        if (!done) {
            Transform closeDoor = FindClosestDoor(transform);
            SetPatrolTargets(closeDoor);
            currentTarget = targetPatrol1;
            done = true;
        }

        if (done)
        { 
            AnimateWalking();
            RaycastHit2D sight;
            if (chase == false)
            {
                sight = Physics2D.Raycast(transform.position, currentTarget - transform.position, 10000f);
            }
            else
            {
                sight = Physics2D.Raycast(transform.position, player.position - transform.position, 10000f);
            }
            if (sight.collider != null)
            {
                Debug.DrawLine(transform.position, sight.point, Color.red);
                if (sight.collider.CompareTag("Player") || sight.collider.CompareTag("Projectile"))
                {
                    if (Vector3.Distance(transform.position, player.position) >= 1)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
                    }
                    //transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
                    chase = true;
                    AnimateApproach();
                    Shooting();
                }
                else
                {
                    if (Vector3.Distance(transform.position, player.position) <= 5f)
                    {
                        chase = true;
                    } else
                    {
                        chase = false;
                    }
                    AnimateIdle();
                    if (Vector3.Distance(currentTarget, transform.position) <= 0.2)
                    {
                        SwitchTarget();
                    }
                    transform.position = Vector3.MoveTowards(transform.position, currentTarget, speed * Time.deltaTime);
                }
            }
            
        }


/*
        if (transform != null)
        {
            AnimateWalking();
            if (Vector3.Distance(currentTarget, player.position) <= chaseDistance &&
            Vector3.Distance(transform.position, currentTarget) >= returnDistance)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
                AnimateApproach();
                Shooting();
            }
            else
            {
                AnimateIdle();
                if (currentTarget == transform.position)
                {
                    SwitchTarget();
                }
                transform.position = Vector3.MoveTowards(transform.position, currentTarget, speed * Time.deltaTime);
            }
        }
        */
    }

    void SwitchTarget() {
        if (current == 1) {
            currentTarget = targetPatrol1;
            current = 0;
        } else {
            currentTarget = targetPatrol2;
            current = 1;
        }
    }

    void Shooting() {
        if (timeBetweenShots <= 0) {
            Instantiate(projectile, transform.position, Quaternion.identity);
            audioSrc.PlayOneShot(bulletShot);
            timeBetweenShots = startTimeBetweenShots;
        }
        else {
            timeBetweenShots -= Time.deltaTime; 
        }
    }

    void AnimateIdle() 
    {
        Vector3 currentPosition = transform.position;
        Vector3 moveToward = currentTarget;
        moveDirection = moveToward - currentPosition;
        moveDirection.z = 0; 
        moveDirection.Normalize();
        float targetAngle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg +90f;
        transform.rotation = Quaternion.Slerp( transform.rotation, Quaternion.Euler( 0, 0, targetAngle ), 
                                turnSpeed * Time.deltaTime );
    }

    void AnimateApproach()
    {
        Vector3 currentPosition = transform.position;
        Vector3 moveToward = player.position;
        moveDirection = moveToward - currentPosition;
        moveDirection.z = 0; 
        moveDirection.Normalize();
        float targetAngle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg +90f;
        transform.rotation = Quaternion.Slerp( transform.rotation, Quaternion.Euler( 0, 0, targetAngle ), 
                                turnSpeed * Time.deltaTime );
    }

    void AnimateWalking()
    {
        int index = (int)(Time.timeSinceLevelLoad * framesPerSecond);
        index %= sprites.Length;
        spriteRenderer.sprite = sprites[index];
    }

    Transform FindClosestDoor(Transform self)
    {
        Transform[] transArray = FindObjectsOfType<LoadNewRoom>().Select(f => f.transform).ToArray();
        //get closest characters (to referencePos)
        var nClosest = transArray.OrderBy(t => (t.position - self.position).sqrMagnitude).FirstOrDefault();
        return nClosest;
    }

    void SetPatrolTargets(Transform closeDoor)
    {
        //Debug.Log("Parent name  = " + closeDoor.parent.name);
        
        switch (closeDoor.parent.name)
        {
            case "TopDoor":
                targetPatrol1 = new Vector3(closeDoor.parent.position.x+5, closeDoor.parent.position.y-1);
                targetPatrol2 = new Vector3(closeDoor.parent.position.x-5, closeDoor.parent.position.y-1);
                break;
            case "BottomDoor Variant":
                targetPatrol1 = new Vector3(closeDoor.parent.position.x+5, closeDoor.parent.position.y+1);
                targetPatrol2 = new Vector3(closeDoor.parent.position.x-5, closeDoor.parent.position.y+1);
                break;
            case "LeftDoor Variant":
                targetPatrol1 = new Vector3(closeDoor.parent.position.x+1, closeDoor.parent.position.y+3);
                targetPatrol2 = new Vector3(closeDoor.parent.position.x+1, closeDoor.parent.position.y-3);
                break;
            case "RightDoor Variant":
                targetPatrol1 = new Vector3(closeDoor.parent.position.x-1, closeDoor.parent.position.y+3);
                targetPatrol2 = new Vector3(closeDoor.parent.position.x-1, closeDoor.parent.position.y-3);
                break;
        }

    }
}
