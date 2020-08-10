using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    //attributes
    public float enemyMaxHealth;
    public float enemyCurrentHealth;
    public GameObject dropCharge;
    private SpriteRenderer rend;
    private Color currentColor;
    public Color flashColor;
    public GameObject deathState;
    
    // Start is called before the first frame update
    void Start()
    {
        enemyCurrentHealth = enemyMaxHealth;
        rend = GetComponent<SpriteRenderer>();
        rend.enabled = true;
        currentColor = rend.color;
    }

    void Update()
    {
        if (enemyCurrentHealth <= 0)
        {
            DropObject();
            EnemyDeath();
        }
    }

    void Damage(float damage)
    {
        enemyCurrentHealth -= damage;
        rend.color = Color.red;
        rend.material.color = currentColor;
        if (enemyCurrentHealth <= 0) {
            DropObject();
            EnemyDeath();
        }
    }
    public void EnemyDeath() 
    {
        // record deaths somewhere
        Color old = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().old;
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>().color == Color.red) {
            GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>().color = old;
        }
        if (transform.parent.CompareTag("Turret"))
        {
            transform.parent.gameObject.tag = "Untagged";
        }
        FindObjectOfType<RoomTemplates>().deadEnemies += 1;
        Destroy(gameObject);
    }

    
    void DropObject() 
    {
        Instantiate(dropCharge, transform.position, Quaternion.identity);
        if (deathState != null)
        {
            Instantiate(deathState, transform.position, Quaternion.identity);
        }
        
    }

}
