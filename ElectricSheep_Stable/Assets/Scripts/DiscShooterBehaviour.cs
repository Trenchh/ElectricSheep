using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscShooterBehaviour : MonoBehaviour
{

    public static float leftRotation;
    public static float rightRotation;
    public static float currentRotation;
    public static bool fromLeft = true;
    public static bool fromRight = false;
    private float timeBetweenShots;
    public float startTimeBetweenShots;
    public GameObject projectile;
    private Transform player;
    public int attackDistance;
    private Vector3 moveDirection;
    public float turnSpeed;
    private AudioSource audioSrc;
    public AudioClip bulletShot;

    // Start is called before the first frame update
    void Start()
    {
        currentRotation = transform.eulerAngles.z;
        leftRotation = currentRotation - 30;
        rightRotation = currentRotation + 30;
        timeBetweenShots = Random.Range(0.1f, 1f);
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        audioSrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        AnimateAttack();
        RaycastHit2D sight = Physics2D.Raycast(transform.position, player.position - transform.position, 10000f);
        if (sight.collider != null) {
            Debug.DrawLine(transform.position, sight.point, Color.red);
            if (sight.collider.CompareTag("Player") || sight.collider.CompareTag("Projectile")) {
                AnimateAttack();
                Shooting();
            } else {
                AnimateIdle();
            }
        }         
    
        /*
        if (Vector3.Distance(transform.position, player.position) <= attackDistance) 
        {
            AnimateAttack();
            Shooting();
        }
        else
        {
            AnimateIdle();
        }
        */
    }
    void Shooting()
    {
        if (timeBetweenShots <= 0)
        {
            Instantiate(projectile, transform.position, transform.rotation);
            audioSrc.PlayOneShot(bulletShot);
            timeBetweenShots = Random.Range(0f,1f);
        }
        else
        {
            timeBetweenShots -= Time.deltaTime;
        }
    }

    void AnimateIdle()
    {
        if (Mathf.Abs(currentRotation - leftRotation) <= 0.5f && fromRight == true)
        {
            currentRotation += 0.1f;
            fromRight = false;
            fromLeft = true;
        }
        else if (fromRight == true)
        {
            currentRotation -= 0.1f;
        }
        if (Mathf.Abs(currentRotation - rightRotation) <= 0.5f && fromLeft == true)
        {
            currentRotation -= 0.1f;
            fromRight = true;
            fromLeft = false;
        }
        else if (fromLeft == true)
        {
            currentRotation += 0.1f;
        }
        transform.rotation = transform.rotation = Quaternion.Euler(Vector3.forward * currentRotation);
    }

    void AnimateAttack()
    {
        Vector3 currentPosition = transform.position;
        Vector3 moveToward = player.position;
        moveDirection = moveToward - currentPosition;
        moveDirection.z = 0;
        moveDirection.Normalize();
        float targetAngle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg + 90f;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, targetAngle), turnSpeed * Time.deltaTime);
    }
}
