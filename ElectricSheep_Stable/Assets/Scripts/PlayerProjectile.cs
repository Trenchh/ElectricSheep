using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{

    public int damage;
    public float speed;
    public float turnSpeed;
    public Sprite[] sprites;
    public float framesPerSecond;
    private SpriteRenderer spriteRenderer;
    public GameObject explosion;
    public static Vector3 shootDirection; 

    // Start is called before the first frame update
    void Start()
    {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.spriteRenderer.enabled = true;
        

    }

    // Update is called once per frame
    void Update()
    {
        int index = (int)(Time.timeSinceLevelLoad * framesPerSecond);
        index %= sprites.Length;
        spriteRenderer.sprite = sprites[index];
        //transform.Translate(shootDirection * speed * Time.deltaTime);
    }



    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy")) {
            other.GetComponent<EnemyHealth>().enemyCurrentHealth -= damage;
            other.GetComponent<EnemyHitFlash>().Hit(other);
            //StartCoroutine(Flasher(other));
            DestoryProjectile();
        }
        if (other.CompareTag("PatrolEnemy")) {
            other.GetComponent<EnemyHealth>().enemyCurrentHealth -= damage;
            other.GetComponent<EnemyHitFlash>().Hit(other);
            other.GetComponent<PatrolEnemyMovement>().chase = true;
            DestoryProjectile();
        }
        if (other.CompareTag("Wall")) {
            DestoryProjectile();
        }

        if (other.CompareTag("Boss"))
        {
            if (other.GetComponent<BossBehaviour>().idleState == true || other.GetComponent<BossBehaviour>().finalState == true)
            {
                other.GetComponent<BossHealth>().healthPoints -= damage;
                other.GetComponent<EnemyHitFlash>().Hit(other);
            }
            DestoryProjectile();
        }
        if (explosion != null)
        {
            GameObject xplo = Instantiate(explosion, transform.position, transform.rotation);
            Destroy(xplo, 0.2f);
        }
    }

    void DestoryProjectile()
    {
        Destroy(gameObject);
        //destroyAnimation();
    }

 

    IEnumerator Flasher(Collider2D enemy) 
    {
        for (int i = 0; i < 2; i++)
        {
            Color old = enemy.GetComponent<SpriteRenderer>().color;
            enemy.GetComponent<SpriteRenderer>().color = Color.magenta;
            yield return new WaitForSeconds(.1f);
            enemy.GetComponent<SpriteRenderer>().color = old;
            yield return new WaitForSeconds(.1f);
        }
    }
}
