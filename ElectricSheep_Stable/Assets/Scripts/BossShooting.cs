using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShooting : MonoBehaviour
{
    public float rotationSpeed;
    public GameObject projectile;
    public float startTimeBetweenShots;
    private float timeBetweenShots;
    public float bulForce;
    public static bool shootMode1;
    public static bool shootMode2;
    public AudioClip floppyShot;
    private AudioSource audioSrc;
    // Start is called before the first frame update
    void Start()
    {
        timeBetweenShots = startTimeBetweenShots;
        audioSrc = GetComponent<AudioSource>();
        bulForce = 15f;
        shootMode1 = false;
        shootMode2 = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (shootMode1 == true)
        {
            Shooting1();
        }
        if (shootMode2 == true)
        {
            Shooting2();
        }
    }

    void Shooting1()
    {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        if (timeBetweenShots <= 0)
        {
            GameObject bul, bul2, bul3, bul4;
            audioSrc.PlayOneShot(floppyShot);
            bul = Instantiate(projectile, transform.position, transform.rotation);
            bul2 = Instantiate(projectile, transform.position, transform.rotation);

            Rigidbody2D rb,rb2,rb3,rb4 = bul2.GetComponent<Rigidbody2D>();
            rb = bul.GetComponent<Rigidbody2D>();
            rb2 = bul2.GetComponent<Rigidbody2D>();

            rb2.AddForce(transform.up * -bulForce, ForceMode2D.Impulse);
            rb.AddForce(transform.up * bulForce, ForceMode2D.Impulse);
            timeBetweenShots = startTimeBetweenShots;
        }
        else
        {
            timeBetweenShots -= Time.deltaTime;
        }
    }
    
    void Shooting2()
    {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        if (timeBetweenShots <= 0)
        {
            audioSrc.PlayOneShot(floppyShot);
            GameObject bul, bul2, bul3, bul4;
            bul = Instantiate(projectile, transform.position, transform.rotation);
            bul2 = Instantiate(projectile, transform.position, transform.rotation);
            bul3 = Instantiate(projectile, transform.position, transform.rotation);
            bul4 = Instantiate(projectile, transform.position, transform.rotation);

            Rigidbody2D rb, rb2, rb3, rb4 = bul2.GetComponent<Rigidbody2D>();
            rb = bul.GetComponent<Rigidbody2D>();
            rb2 = bul2.GetComponent<Rigidbody2D>();
            rb3 = bul3.GetComponent<Rigidbody2D>();
            rb4 = bul4.GetComponent<Rigidbody2D>();

            rb2.AddForce(transform.up * -bulForce, ForceMode2D.Impulse);
            rb4.AddForce(transform.right * -bulForce, ForceMode2D.Impulse);
            rb3.AddForce(transform.right * bulForce, ForceMode2D.Impulse);
            rb.AddForce(transform.up * bulForce, ForceMode2D.Impulse);
            timeBetweenShots = startTimeBetweenShots;
        }
        else
        {
            timeBetweenShots -= Time.deltaTime;
        }
    }
}
