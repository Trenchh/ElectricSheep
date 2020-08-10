using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public List<GameObject> Enemies1, Enemies2, Enemies3;
    public List<List<GameObject>> enemies = new List<List<GameObject>>();
    public List<GameObject> Props;
    Vector3 whereToSpawn;
    float randX, randY;
    private int enemyBudget, curEnemyPoints;
    private bool spawned = false;
    
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Room_Template").GetComponent<RoomTemplates>().doorClean && !spawned)
        {
            spawned = true;
            curEnemyPoints = 0;
            enemyBudget = GetComponentInParent<RoomProperties>().enemyBudget;
            while (curEnemyPoints < enemyBudget)
            {
                int rand = Mathf.RoundToInt(Random.value * 100000);
                //Debug.Log("Random number is: " + rand);
                // 1-cost enemies will always be valid if we got in here
                enemies.Add(Enemies1);
                int availPts = enemyBudget - curEnemyPoints;

                if (availPts > 1)
                {
                    enemies.Add(Enemies2);
                }
                if (availPts > 2)
                {
                    enemies.Add(Enemies3);
                }
                //Debug.Log("Enemy count is: " + enemies.Count);
                // better randomization goes in here.
                // per excel figuring
                Random.InitState(rand);
                int enemyPt = Random.Range(1, enemies.Count + 1);
                //Debug.Log("Rand enemyPt is: " + enemyPt);
                //pick random location and valid enemy to instantiate
                randX = Random.Range( - 8f,  8f);
                randY = Random.Range( - 3.8f, 3.8f);
                whereToSpawn = new Vector3(randX, randY);
                Instantiate(enemies[enemyPt-1][Random.Range(0, enemies[enemyPt - 1].Count)], transform.parent.gameObject.transform.position + whereToSpawn, Quaternion.identity, transform.parent.gameObject.transform);
                curEnemyPoints += enemyPt;
                enemies.Clear();
            }

            for (int i = 0; i < Random.Range(4,7); i++)
            {
                int rand = Mathf.RoundToInt(Random.value * 100000);
                Random.InitState(rand);
                //randX = Random.Range(transform.position.x - 8.5f, transform.position.x + 8.5f);
                //randY = Random.Range(transform.position.y - 4f, transform.position.y + 4f);
                randX = Random.Range( -8.5f, 8.5f);
                randY = Random.Range( -4f, 4f);
                if (!((randX < -5f & (randY < 1f & randY > -1f)) | (randX > 5f & (randY < 1f & randY > -1f)) | (randY < -2f & (randX < 1.5f & randX > -1.5f)) | (randY > 2f & (randX < 1.5f & randX > -1.5f))))
                {
                    //Debug.LogWarning("RandX = " + randX  + " and RandY = " + randY);
                    whereToSpawn = new Vector3(randX, randY);
                    Instantiate(Props[Random.Range(0, Props.Count)], transform.parent.gameObject.transform.position+whereToSpawn, Quaternion.identity, transform.parent.gameObject.transform);
                }
            }
        }
    }
}
