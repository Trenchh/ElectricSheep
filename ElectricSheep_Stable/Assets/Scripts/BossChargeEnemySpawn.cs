using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChargeEnemySpawn : MonoBehaviour
{
    public GameObject[] spawners;
    public GameObject dropCharge;
    public GameObject bEnemy;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnCharges", 2f, 6f);
        InvokeRepeating("SpawnEnemy", 10f, 6f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnCharges()
    {
        List<int> randomNums = new List<int>();
        List<Transform> charges = new List<Transform>();
        int count = 4;
        while (count != 0)
        {
            int num = Random.Range(0, 6);
            if (!randomNums.Contains(num))
            {
                randomNums.Add(num);
                charges.Add(spawners[num].GetComponent<Transform>());
                count--;
            }
        }
        foreach (Transform charge in charges)
        {
            Instantiate(dropCharge, charge.position, charge.rotation);
            //Instantiate(slimeEnemy, charge.position, charge.rotation);
        }

    }

    void SpawnEnemy()
    {
        List<int> randomNums = new List<int>();
        List<Transform> enemy = new List<Transform>();
        int count = 4;
        while (count != 0)
        {
            int num = Random.Range(0, 6);
            if (!randomNums.Contains(num))
            {
                randomNums.Add(num);
                enemy.Add(spawners[num].GetComponent<Transform>());
                count--;
            }
        }
        foreach (Transform en in enemy)
        {
            Instantiate(bEnemy, en.position, en.rotation);
            //Instantiate(slimeEnemy, charge.position, charge.rotation);
        }
    }
}
