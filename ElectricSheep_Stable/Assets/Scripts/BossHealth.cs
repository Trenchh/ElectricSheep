using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    public int healthPoints;
    private SpriteRenderer spriteRenderer;
    public Sprite[] bossScared;
    public Sprite[] bossScaredDamage1, bossScaredDamage2, bossScaredDamage3, bossScaredDamage4, bossScaredDamage5;
    public Sprite bossNormal;
    public Sprite bossNormalDamage1, bossNormalDamage2, bossNormalDamage3, bossNormalDamage4, bossNormalDamage5;
    public Sprite[] bossNormalDamage2s;
    public Sprite bossHeart, bossHeart2, bossHeart3, bossHeart4, bossHeart5, bossHeart6, bossHeart7;
    private int health1, health2, health3, health4, health5, health6, health7;
    public Sprite bossHappy;
    public int epilogue;
    public bool mercy = false;
    // Start is called before the first frame update
    void Start()
    {
        healthPoints = FindObjectOfType<RoomTemplates>().maxRooms + 10;

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = true;
        bossNormal = spriteRenderer.sprite;
        FindObjectOfType<BossBehaviour>().normalCurrent = bossNormal;
        FindObjectOfType<BossBehaviour>().scaredCurrent = bossScared;
        
    }

    public void InitHealth()
    {
        int actEnemies = FindObjectOfType<RoomTemplates>().activeEnemies;
        int actRooms = FindObjectOfType<RoomTemplates>().activeRooms;
        int deadEnemies = FindObjectOfType<RoomTemplates>().deadEnemies;
        int totRooms = FindObjectOfType<RoomTemplates>().rooms.Count;
        healthPoints += deadEnemies - ((actEnemies / 2) + actRooms);
        if (deadEnemies > 0 && healthPoints < 15)
        {
            healthPoints = 15;
        }
        if (deadEnemies == 0)
        {
            GameObject face = GameObject.FindWithTag("BossFace");
            face.SetActive(false);
            FindObjectOfType<BossBehaviour>().passiveEnding = true;
            FindObjectOfType<BossBehaviour>().normalCurrent = bossHappy;
            if (actRooms+1 == totRooms)
            {
                //Ascendant ending
                FindObjectOfType<DialogueTrigger>().TriggerDialogue(6);
                epilogue = 12;
            }
            else
            {
                // Pacifist Ending
                FindObjectOfType<DialogueTrigger>().TriggerDialogue(5);
                epilogue = 10;
            }
            healthPoints = 0;
        }
        else
        {
            // Basic intro
            int healthDec = healthPoints / 7;
            health1 = healthPoints - healthDec;
            health2 = health1 - healthDec;
            health3 = health2 - healthDec;
            health4 = health3 - healthDec;
            health5 = health4 - healthDec;
            health6 = health5 - healthDec;
            if (FindObjectOfType<PlayerAttack>().title.Contains("Exterminator"))
            {
                healthDec = 1;
                health1 = healthPoints - healthDec;
                health2 = health1 - healthDec;
                health3 = health2 - healthDec;
                health4 = health3 - healthDec;
                health5 = health4 - healthDec;
                health6 = health5 - healthDec;
                // Exterminator intro
                FindObjectOfType<DialogueTrigger>().TriggerDialogue(7);
                epilogue = 11;
            }
            else
            {
                FindObjectOfType<DialogueTrigger>().TriggerDialogue(8);
                epilogue = 9;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!FindObjectOfType<PauseMenu>().GameIsPaused)
        {
            HealthStateChange();
        }
    }

    void HealthStateChange()
    {
        //Debug.Log(healthPoints);
        if (healthPoints <= health1 && healthPoints > health2)
        {
            FindObjectOfType<BossBehaviour>().normalCurrent = bossNormalDamage1;
            FindObjectOfType<BossBehaviour>().scaredCurrent = bossScaredDamage1;
            FindObjectOfType<BossHeartChange>().bossHeartCurrent = bossHeart2;
        }
        if (healthPoints <= health2 && healthPoints > health3)
        {
            FindObjectOfType<BossBehaviour>().normalCurrent = bossNormalDamage2;
            FindObjectOfType<BossBehaviour>().scaredCurrent = bossScaredDamage2;
            FindObjectOfType<BossHeartChange>().bossHeartCurrent = bossHeart3;
        }
        if (healthPoints <= health3 && healthPoints > health4)
        {
            FindObjectOfType<BossBehaviour>().normalCurrent = bossNormalDamage3;
            FindObjectOfType<BossBehaviour>().scaredCurrent = bossScaredDamage3;
            FindObjectOfType<BossHeartChange>().bossHeartCurrent = bossHeart4;
        }
        if (healthPoints <= health4 && healthPoints > health5)
        {
            FindObjectOfType<BossBehaviour>().normalCurrent = bossNormalDamage4;
            FindObjectOfType<BossBehaviour>().scaredCurrent = bossScaredDamage4;
            FindObjectOfType<BossHeartChange>().bossHeartCurrent = bossHeart5;
        }
        if (healthPoints <= health5 && healthPoints > health6)
        {
            FindObjectOfType<BossBehaviour>().normalCurrent = bossNormalDamage5;
            FindObjectOfType<BossBehaviour>().scaredCurrent = bossScaredDamage5;
            FindObjectOfType<BossHeartChange>().bossHeartCurrent = bossHeart6;
        }
        if (healthPoints <= health6 && healthPoints > 0)
        {
            if (!mercy)
            {
                FindObjectOfType<DialogueTrigger>().TriggerDialogue(13);
                mercy = true;
            }
            GetComponent<BossBehaviour>().idleState = false;
            GetComponent<BossBehaviour>().shootState = false;
            GetComponent<BossBehaviour>().finalState = true;
            FindObjectOfType<BossHeartChange>().bossHeartCurrent = bossHeart7;
            ;
        }
        if (healthPoints <= 0)
        {
            GetComponent<BossBehaviour>().idleState = false;
            GetComponent<BossBehaviour>().shootState = false;
            GetComponent<BossBehaviour>().finalState = false;
            GetComponent<BossBehaviour>().deathState = true;
        }
    }
}

