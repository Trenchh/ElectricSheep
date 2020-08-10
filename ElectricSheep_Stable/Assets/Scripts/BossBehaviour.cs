using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : MonoBehaviour
{
    public Transform target;
    public bool playerInRoom;
    public bool idleState;
    public bool shootState;
    public bool firstState;
    public bool finalState;
    public bool deathState;
    public float idleTime;
    public float shootTime;
    public float firstTime;
    public float spawnTime;
    private float tempSpawnTime;
    private float tempFirstTime;
    private float tempShootTime;
    public GameObject bEnemy;
    public Sprite redEyes;
    public Sprite happyBoss;

    private float tempIdleTime;
    private float tempFinalTime;
    private SpriteRenderer spriteRenderer;
    public Sprite[] scaredCurrent;
    public Sprite[] finalSprite;
    public Sprite normalCurrent;
    public float framesPerSecond;
    public GameObject[] chargeSpawners;
    public GameObject dropCharge;
    public int numChargesPerIdle;
    public GameObject spawnEvent;
    public GameObject spawner;
    public bool start = true;
    public bool passiveEnding;
    public bool done = false;

    public int HP = 123; //just for win menu
    Color old = new Color(1, 14, 31);
    Color bg = Color.white;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = true;
        Physics2D.queriesStartInColliders = false;
        playerInRoom = false;
        passiveEnding = false;
        idleState = false;
        shootState = false;
        firstState = true;
        idleTime = 5f;
        shootTime = Random.Range(15f, 25f);
        firstTime = 9.9f;
        spawnTime = 15f;
        tempFirstTime = firstTime;
        tempShootTime = shootTime;
        tempIdleTime = idleTime;
        tempSpawnTime = spawnTime;
        tempFinalTime = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!FindObjectOfType<PauseMenu>().GameIsPaused)
        {
            CheckRayCast();
            if (playerInRoom)
            {
                StartBattle();
                HP = GetComponent<BossHealth>().healthPoints; //just for win menu
            }
        }
    }

    void CheckRayCast()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        RaycastHit2D sight = Physics2D.Raycast(transform.position, target.position - transform.position, 10000f);
        if (sight.collider != null)
        {
            Debug.DrawLine(transform.position, sight.point, Color.red);
            if (sight.collider.CompareTag("Player"))
            {
                playerInRoom = true;
            }
        }
    }

    void StartBattle()
    {
        // set health here, one time
        if (start) {
            GetComponent<BossHealth>().InitHealth();
            start = false;
            Camera.main.backgroundColor = Color.Lerp(old, bg, 1);
            if (!passiveEnding)
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().gameMusic.Stop();
                PlayerMovement.bossMusic = true;
            }            
        }

        StateCheck();
        if (tempSpawnTime <= 0)
        {
            SpawnCharges();
            tempSpawnTime = 10f;
        }
        else
        {
            tempSpawnTime -= Time.deltaTime;
        }
    }
    

    void StateCheck()
    {
        spriteRenderer.sprite = normalCurrent;

        if (firstState)
        {
            if (tempFirstTime <= 0)
            {
                shootState = true;
                idleState = false;
                firstState = false;
                normalCurrent = redEyes;
            }
            else
            {
                tempFirstTime -= Time.deltaTime;
            }
        }

        if (shootState)
        {
            BossShooting.shootMode1 = true;
            if (tempShootTime <= 0)
            {
                shootState = false;
                idleState = true;
                tempShootTime = Random.Range(15f, 25f);
            }
            else
            {
                tempShootTime -= Time.deltaTime;
            }
        }
        if (idleState)
        {
            AnimateIdle();
            BossShooting.shootMode1 = false;
            if (tempIdleTime <= 0)
            {
                shootState = true;
                idleState = false;
                tempIdleTime = 5f;
                //spawnCharges();
            }
            else
            {
                tempIdleTime -= Time.deltaTime;
            }
        }
        if (finalState)
        {
            if (tempFinalTime <= 0)
            {
                //spawnCharges();
                tempFinalTime = 10f;
            }
            else
            {
                tempFinalTime -= Time.deltaTime;
            }
            BossShooting.shootMode1 = false;
            BossShooting.shootMode2 = true;
            AnimateFinal();
        }
        if (deathState)
        {
            BossShooting.shootMode2 = false;
            EndGame();
            if (done)
            {
                FindObjectOfType<PauseMenu>().Win();
            }
            if (!done)
            {
               if(passiveEnding)
                {
                    spriteRenderer.sprite = happyBoss;
                }
                else
                {
                    GameObject.FindWithTag("BossFace").SetActive(false);
                }
                GameObject.FindWithTag("BossFace1").SetActive(false);
                FindObjectOfType<DialogueTrigger>().TriggerDialogue(FindObjectOfType<BossHealth>().epilogue);
                done = true;
            }
        }
    }

    void SpawnCharges()
    {
        List<int> randomNums = new List<int>();
        List<Transform> charges = new List<Transform>();
        int count = numChargesPerIdle;
        while (count != 0)
        {
            int num = Random.Range(0, 6);
            if (!randomNums.Contains(num))
            {
                randomNums.Add(num);
                charges.Add(chargeSpawners[num].GetComponent<Transform>());
                count--;
            }
        }
        foreach (Transform charge in charges)
        {
            Instantiate(dropCharge, charge.position, charge.rotation);
            Instantiate(bEnemy, charge.position, charge.rotation);
        }
    }


    void AnimateIdle()
    {
        int index = (int)(Time.timeSinceLevelLoad * framesPerSecond);
        index %= scaredCurrent.Length;
        spriteRenderer.sprite = scaredCurrent[index];
    }

    void AnimateFinal()
    {
        int index = (int)(Time.timeSinceLevelLoad * framesPerSecond);
        index %= finalSprite.Length;
        spriteRenderer.sprite = finalSprite[index];
    }

    void EndGame()
    {
        Destroy(spawnEvent);
        //Destroy(gameObject);
    }        
}
