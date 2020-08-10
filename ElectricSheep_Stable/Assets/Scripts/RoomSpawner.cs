using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public string openDir;

    // 1,2,3,4 = need bottom, top, left, right door to spawn

    private RoomTemplates templates;
    private int rand;
    private bool spawned = false;
    private GameObject roomSpawned;
    public static int[] seeds = {85937, 111111, 1123410, 1323410, 1423410, 5743410, 4133711 };


    private void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Room_Template").GetComponent<RoomTemplates>();
        Debug.Log("SEED INDEX:" + PlayerMovement.seedIndex);
        UnityEngine.Random.InitState(seeds[PlayerMovement.seedIndex]);
        Debug.Log("SEED:" + UnityEngine.Random.seed);
        Invoke("Spawn", 0.2f);
    }


    // Update is called once per frame


    void Spawn()
    {
        if (!spawned) { 
                   
            // all room arrays are the same length, 8.
            rand = UnityEngine.Random.Range(0, 8);
            switch (openDir)
            {
                case "B":
                    //rand = Random.Range(0, templates.bottomDoorRooms.Length);
                    roomSpawned = Instantiate(templates.bottomDoorRooms[rand], transform.position, templates.bottomDoorRooms[rand].transform.rotation);
                    Destroy(roomSpawned.GetComponent<Transform>().Find("Bottom Spawn Point").gameObject);
                    break;
                case "T":
                    //rand = Random.Range(0, templates.topDoorRooms.Length);
                    roomSpawned = Instantiate(templates.topDoorRooms[rand], transform.position, templates.topDoorRooms[rand].transform.rotation);
                    Destroy(roomSpawned.GetComponent<Transform>().Find("Top Spawn Point").gameObject);
                    break;
                case "L":
                    //rand = Random.Range(0, templates.leftDoorRooms.Length);
                    roomSpawned = Instantiate(templates.leftDoorRooms[rand], transform.position, templates.leftDoorRooms[rand].transform.rotation);
                    Destroy(roomSpawned.GetComponent<Transform>().Find("Left Spawn Point").gameObject);
                    break;
                case "R":
                    //rand = Random.Range(0, templates.bottomDoorRooms.Length);
                    roomSpawned = Instantiate(templates.rightDoorRooms[rand], transform.position, templates.rightDoorRooms[rand].transform.rotation);
                    Destroy(roomSpawned.GetComponent<Transform>().Find("Right Spawn Point").gameObject);
                    break;
            }
            spawned = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("SpawnPoint"))
        {
            if(!collision.GetComponent<RoomSpawner>().spawned && !spawned )
            {
                /*openDir = openDir + collision.GetComponent<RoomSpawner>().openDir;
                String.Concat(openDir.OrderBy(c => c)).Distinct();
                Destroy(collision.gameObject);
                */
                Instantiate(templates.closedRoom, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            spawned = true;
            //Debug.Log("Collided and destroyed gameObject" + gameObject.name);
        }

    }

}
