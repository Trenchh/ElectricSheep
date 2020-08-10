using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerAttack : MonoBehaviour
{

    public Transform attackPoint;
    //public GameObject swipePrefab;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI healthText2;
    public TextMeshProUGUI healthText3;
    public int healthMax;
    public static int healthCount;
    public static int healthCount2;
    public static int healthCount3;
    private AudioSource audioSrc;
    
    public AudioClip cantHit;
    public AudioClip bulletShot;
    public LayerMask whatIsEnemy;
    public float attackRange;
    public int damage;
    public Sprite[] sprites;
    public float framesPerSecond;
    private SpriteRenderer spriteRenderer;
    public float slashWait;
    public bool slashing;
    public GameObject redProjectile;
    public GameObject blueProjectile;
    public float bulForce;
    public GameObject firePointRed;
    public GameObject firePointBlue;
    public string currentFire, title;
    public string timeFormatted, finalText;
    private bool healthMessage;
    private string text1, text2, text3, text4, text5, text6;
    private bool paci, search, assassin, exter, asc;

    //public float swipeForce = 0f;

    void Start()
    {
        healthMax = 15;
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.spriteRenderer.enabled = true;
        audioSrc = GetComponent<AudioSource>();
        healthCount = healthMax;
        healthCount2 = healthCount;
        healthCount3 = healthCount;
        bulForce = 15f;
        currentFire = "Red";
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 shootDirection;
        shootDirection = Input.mousePosition;
        shootDirection.z = 0.0f;
        shootDirection = Camera.main.ScreenToWorldPoint(shootDirection);
        RaycastHit2D sight = Physics2D.Raycast(transform.position, shootDirection - firePointRed.GetComponent<Transform>().position, 10000f);
        //Debug.DrawLine(firePointRed.GetComponent<Transform>().position, sight.point, Color.red);
        healthCount2 = healthCount;
        healthCount3 = healthCount;
        if(!FindObjectOfType<PauseMenu>().GameIsPaused)
        {
            /*
            if (healthCount == 10 && !healthMessage)
            {
                DialogueTrigger go = FindObjectOfType<DialogueTrigger>();
                go.TriggerDialogue(0);
                healthMessage = true;
            }*/
            DetermineTitle();

            if (Input.GetButtonDown("Fire1"))
            {
                if (healthCount > 1)
                {
                    currentFire = "Blue";
                    Attack();
                    audioSrc.PlayOneShot(bulletShot);
                }
                else
                {
                    audioSrc.PlayOneShot(cantHit);
                }
            }

            if (Input.GetButtonDown("Fire2"))
            {
                if (healthCount > 1)
                {
                    currentFire = "Red";
                    Attack();
                    audioSrc.PlayOneShot(bulletShot);
                }
                else
                {
                    audioSrc.PlayOneShot(cantHit);
                }
            }
        }
        

        /*
        if (slashing) 
        {
            slashWait -= Time.deltaTime;
            Animate();
            if (slashWait <= 0) {
                slashing = false;
                slashWait = startSlashWait;
            }
        }
        */
        healthText.text = healthCount.ToString();
        healthText2.text = healthCount2.ToString();
        healthText3.text = healthCount3.ToString();
        //FindObjectOfType<BossBehaviour>().deathState
        if (healthCount < 1 || FindObjectOfType<BossBehaviour>().deathState)
        {
            GenerateSummary();
            if (healthCount < 1)
            {
                FindObjectOfType<PauseMenu>().Death();
            }
        }
        
      
    }

    void Attack()
    {
        /*
        GameObject swipe = Instantiate(swipePrefab, attackPoint.position, attackPoint.rotation);
        Rigidbody2D rb =  swipe.GetComponent<Rigidbody2D>();
        rb.AddForce(attackPoint.up * swipeForce, ForceMode2D.Impulse);
        Destroy(swipe, .1f);
        */

        /*
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, whatIsEnemy);

                    for (int i = 0; i < enemiesToDamage.Length; i++)
                    {
                        StartCoroutine(Flasher(enemiesToDamage[i]));
                        enemiesToDamage[i].GetComponent<EnemyHealth>().enemyCurrentHealth -= damage;
                        Debug.Log("Damage dealt");
                    }
                    healthCount -= 1;
                    */
        //Vector3 target = transform.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y));
        Vector3 shootDirection;
        shootDirection = Input.mousePosition;
        shootDirection = Camera.main.ScreenToWorldPoint(shootDirection);
        GameObject bul;
        Vector3 eAngles;
        Quaternion rot;
        float ang;
        if (currentFire == "Red") {
            shootDirection = (shootDirection - firePointRed.GetComponent<Transform>().position).normalized;
            shootDirection.z = 0.0f;
            ang = Vector3.Angle(firePointRed.GetComponent<Transform>().up, shootDirection);
            eAngles = firePointRed.GetComponent<Transform>().rotation.eulerAngles;
            eAngles.z += ang;
            rot = Quaternion.Euler(eAngles.x, eAngles.y, eAngles.z);
            bul = Instantiate(redProjectile, firePointRed.GetComponent<Transform>().position, rot);
        } else {
            shootDirection = (shootDirection - firePointBlue.GetComponent<Transform>().position).normalized;
            shootDirection.z = 0.0f;
            ang = Vector3.Angle(firePointBlue.GetComponent<Transform>().up, shootDirection);
            eAngles = firePointBlue.GetComponent<Transform>().rotation.eulerAngles;
            eAngles.z -= ang;
            rot = Quaternion.Euler(eAngles.x, eAngles.y, eAngles.z);
            bul = Instantiate(blueProjectile, firePointBlue.GetComponent<Transform>().position, rot);
        }
        Rigidbody2D rb = bul.GetComponent<Rigidbody2D>();
        rb.AddForce(bul.GetComponent<Transform>().up * bulForce, ForceMode2D.Impulse);
        healthCount -= 1;
    }

     
    void Animate()
    {
        int index = (int)(Time.timeSinceLevelLoad * framesPerSecond);
        index %= sprites.Length;
        spriteRenderer.sprite = sprites[index];
    }

    void GenerateSummary()
    {
        DetermineTime();
        DetermineTitle();
        text1 = "It took you ";
        text2 = " to ";
        text3 = ".  You explored ";
        text4 = ", terminated ";
        text5 = ", and will be remembered as ";
        text6 = " in the making.";
        string win;

        string roomText = FindObjectOfType<RoomTemplates>().activeRooms + " room";
        if (FindObjectOfType<RoomTemplates>().activeRooms > 1)
        {
            roomText += "s";
        }

        string enemyText = FindObjectOfType<RoomTemplates>().deadEnemies + " rogue process";
        if (FindObjectOfType<RoomTemplates>().deadEnemies > 1 || FindObjectOfType<RoomTemplates>().deadEnemies == 0)
        {
            enemyText += "es";
        }

        if (healthCount < 1)
        {
            win = "die";
            if (title == "an Exterminator")
            {
                text6 = ".";
            }
        }
        else
        {
            win = "win";
            text6 = ".";
        }
        finalText = text1 + timeFormatted + text2 + win + text3 + roomText + text4 + enemyText + text5 + title + text6;
    }

    void DetermineTitle()
    {
        int actEnemies = FindObjectOfType<RoomTemplates>().activeEnemies;
        int actRooms = FindObjectOfType<RoomTemplates>().activeRooms;
        int deadEnemies = FindObjectOfType<RoomTemplates>().deadEnemies;
        if (deadEnemies == 0)
        {
            paci = true;
        }
        else
        {
            paci = false;
        }
        if (1.0f * actRooms/(FindObjectOfType<RoomTemplates>().maxRooms-1)>0.7f)
        {
            search = true;
        }
        if (deadEnemies > 0 && actEnemies == 0)
        {
            assassin = true;
        }
        else
        {
            assassin = false;
        }
        if (search && assassin)
        {
            exter = true;
        }
        if (paci && search)
        {
            asc = true;
        }

        if (asc)
        {
            title = "an Ascendant";
        }
        else if (exter)
        {
            title = "an Exterminator";
        }
        else if (search)
        {
            title = "a Searcher";
        }
        else if (assassin)
        {
            title = "an Assassin";
        }
        else if (paci)
        {
            title = "a Pacifist";
        }
        else
        {
            title = "a Sheep";
        }
    }
    void DetermineTime()
    {
        if (Time.timeSinceLevelLoad < 60)
        {
            timeFormatted = Mathf.Round(Time.timeSinceLevelLoad * 100f) / 100f + "s";
        }
        else
        {
            float minutes = Mathf.Round(Time.timeSinceLevelLoad / 60);
            float seconds = Mathf.Round(Time.timeSinceLevelLoad % 60 * 100f)/100f;
            timeFormatted = minutes + "m " + seconds + "s";
        }
    }
}