using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RoomTemplates : MonoBehaviour
{
    public GameObject[] bottomDoorRooms;
    public GameObject[] topDoorRooms;
    public GameObject[] rightDoorRooms;
    public GameObject[] leftDoorRooms;

    public GameObject closedRoom;
    public GameObject bossRoom;

    public List<GameObject> rooms;

    public int maxRooms, activeRooms, totalEnemies, currentEnemies, activeEnemies, deadEnemies;
    public GameObject[] enemyList;

    public bool genComplete = false;
    public bool doorClean = false;
    private bool killed, searcher = false;
    
    private GameObject[] extraSpawns;
    private GameObject[] blankRooms;
    private RaycastHit2D[] doors;
    private LayerMask doorMask;

    private void Start()
    {
       doorMask = LayerMask.GetMask("Door");
    }
    private void Update()
    {
        // the ordering here is necessary to support proper execution - the Update happens within one frame, 
        // and destruction of objects also takes a frame to complete.
        if (genComplete && !doorClean)
        {
            // clean up doors to nowhere
            for (int i = 0; i < rooms.Count; i++)
            {
                /*Debug.LogWarning("Cleaning room #" + i);

                Vector3 upRay = new Vector3(0, 8f, 0);
                Vector3 downRay = new Vector3(0, -8f, 0);
                Vector3 leftRay = new Vector3(-13f, 0, 0);
                Vector3 rightRay = new Vector3(13f, 0, 0);

                Debug.DrawRay(rooms[i].transform.position, upRay, Color.green, 10f);
                //Debug.DrawRay(rooms[i].transform.position, downRay, Color.red, 10f);
                //Debug.DrawRay(rooms[i].transform.position, leftRay, Color.blue, 10f);
                //Debug.DrawRay(rooms[i].transform.position, rightRay, Color.yellow, 10f);
                */
                DoorClean(rooms[i], Vector2.up, 8f);
                DoorClean(rooms[i], Vector2.down, 8f);
                DoorClean(rooms[i], Vector2.left, 13f);
                DoorClean(rooms[i], Vector2.right, 13f);
            }

            blankRooms = GameObject.FindGameObjectsWithTag("Room Blank");
            for (int i = 0; i < blankRooms.Length; i++)
            {
                Destroy(blankRooms[i]);
            }

            RebalanceRooms();

            doorClean = true;
        } // end doorClean

        //Debug.LogError("First Roomcount = " + rooms.Count);
        if (rooms.Count >= maxRooms && !genComplete)
        {
            // remove spawn points as we've reached the room limit
            extraSpawns = GameObject.FindGameObjectsWithTag("SpawnPoint");
            for (int i = 0; i < extraSpawns.Length; i++)
            {
                Destroy(extraSpawns[i]);
            }

            // if we have gone over the limit, trim back to the max rooms value.
            if (rooms.Count > maxRooms)
            {
                for (int i = rooms.Count; i > maxRooms; i--)
                {
                    Destroy(rooms[i - 1]);
                    rooms.RemoveAt(i - 1);
                }
                //Debug.LogError("Post-max cleanup Roomcount = " + rooms.Count);
            }
            // replace the last room generated with the boss room
            Vector3 bossPos = rooms[rooms.Count - 1].transform.position;
            Destroy(rooms[rooms.Count - 1]);
            rooms.RemoveAt(rooms.Count - 1);
            bossRoom = Instantiate(bossRoom, bossPos, Quaternion.identity);
            rooms.Add(bossRoom);
            genComplete = true;
        }

        if (doorClean)
        {
            enemyList = GameObject.FindGameObjectsWithTag("Enemy").Concat(GameObject.FindGameObjectsWithTag("PatrolEnemy")).ToArray();
            currentEnemies = enemyList.Length;
            if (currentEnemies > totalEnemies)
            {
                totalEnemies = currentEnemies;
            }
            activeEnemies = 0;
            for (int room = 0; room < rooms.Count; room++)
            {
                if (rooms[room].GetComponent<RoomProperties>().activated)
                {
                    foreach (Transform child in rooms[room].transform)
                    {
                        if (child.CompareTag("Enemy") || child.CompareTag("PatrolEnemy") || child.CompareTag("Turret"))
                        {
                            activeEnemies += 1;
                        }
                    }
                }
            }
            //Debug.Log("Killed is: " + killed);
            if (!killed && currentEnemies < totalEnemies && activeEnemies == 0)
            {
                FindObjectOfType<DialogueTrigger>().TriggerDialogue(2);
                killed = true;
            }
            //Debug.LogError("activeRooms/rooms.Count = " + 1.0*activeRooms / rooms.Count);
            if (!searcher && 1.0*activeRooms/(rooms.Count-1) > .7f)
            {
                FindObjectOfType<DialogueTrigger>().TriggerDialogue(4);
                searcher = true;
            }


            
            /*Debug.Log("Total Enemies = " + totalEnemies);
            Debug.Log("Active Enemies = " + activeEnemies);
            Debug.Log("Active Rooms = " + activeRooms);*/
        }
    }

    void DoorClean(GameObject room, Vector2 direction, float distance)
    {
        doors = Physics2D.RaycastAll(room.transform.position, direction, distance, doorMask);
        //Debug.Log("New cast for room " + room.name);
        /*for (int i = 0; i < doors.Length; i++)
        {
            Debug.Log("ray hits: " + doors[i].transform.name);
        }*/
        switch (doors.Length)
        {
            // door without corresponding door, delete it
            case 1:
                Destroy(doors[0].collider.gameObject.transform.parent.gameObject);
                break;
            // these are good and expected, no action (no doors, or two doors)
            case 0:
                break;
            case 2:
                // if the room name is Boss, swap sprites on doors to BossDoor

                //Debug.LogWarning("doors[0].transform.parent.parent.name = " + doors[0].transform.parent.parent.name); 
                //Debug.LogWarning("doors[1].transform.parent.parent.name = " + doors[1].transform.parent.parent.name);
                if (doors[0].transform.parent.parent.name == "Boss(Clone)" || doors[1].transform.parent.parent.name == "Boss(Clone)")
                {
                    doors[0].transform.parent.GetComponent<SpriteRenderer>().sprite = doors[0].transform.parent.GetComponent<AltDoorSprite>().mySprite;
                    doors[1].transform.parent.GetComponent<SpriteRenderer>().sprite = doors[1].transform.parent.GetComponent<AltDoorSprite>().mySprite;
                    doors[0].transform.parent.GetChild(1).gameObject.GetComponent<SpriteRenderer>().enabled = true;
                    doors[1].transform.parent.GetChild(1).gameObject.GetComponent<SpriteRenderer>().enabled = true;
                }
                break;
            // anything else we should log, since it isn't expected
            default:
                Debug.LogError("Strange number of doors (" + doors.Length + ") detected.");
                break;                  
        }
    }

    void RebalanceRooms()
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            if (i < rooms.Count / 3)
            {
                rooms[i].GetComponent<RoomProperties>().enemyBudget = 4;
            }
            else if (i < rooms.Count / 3 * 2)
            {
                rooms[i].GetComponent<RoomProperties>().enemyBudget = 6;
            }
            else
            {
                rooms[i].GetComponent<RoomProperties>().enemyBudget = 8;
            }
        }
    }
}
