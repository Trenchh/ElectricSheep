using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonFolow : MonoBehaviour
{
    private Color old;
    public float speed;
    public float turnSpeed;
    public Sprite[] sprites;
    public float framesPerSecond;
    private SpriteRenderer spriteRenderer;
    private Transform target;
    public GameObject dropCharge;
    public GameObject explosion;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.spriteRenderer.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!FindObjectOfType<PauseMenu>().GameIsPaused)
        {
            simpleFollow();
            animateWalking();
            if (Vector3.Distance(transform.position, target.position) < 1.2f)
            {
                PlayerAttack.healthCount -= 1;
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().Hit(GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>());
                GameObject xplo = Instantiate(explosion, transform.position, transform.rotation);
                Destroy(xplo, 0.3f);
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().BalloonPop();
                Destroy(gameObject);
            }
        }
    }

    void simpleFollow()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }

    void animateWalking()
    {
        int index = (int)(Time.timeSinceLevelLoad * framesPerSecond);
        index = index % sprites.Length;
        spriteRenderer.sprite = sprites[index];
    }

    /*
    public void Hit(GameObject other)
    {
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
    */

    void OnTriggerEnter2D(Collider2D other)
    {
        /*
        if (other.CompareTag("Player"))
        {
            PlayerAttack.healthCount -= 1;
            Hit(GameObject.FindGameObjectWithTag("Player"));
            GameObject xplo = Instantiate(explosion, transform.position, transform.rotation);
            Destroy(xplo, 0.2f);
            Destroy(gameObject);
        }
        */
        if (other.CompareTag("Projectile"))
        {
            GameObject xplo = Instantiate(explosion, transform.position, transform.rotation);
            Instantiate(dropCharge, transform.position, transform.rotation);
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().BalloonPop();
            Destroy(xplo, 0.2f);
            Destroy(gameObject);
        }

    }
}
