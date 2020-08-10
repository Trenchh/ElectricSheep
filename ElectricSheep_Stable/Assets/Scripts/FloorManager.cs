using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{

    public  int maxRooms, remainingRooms;
    private int rand1, rand2;
    private RoomTemplates templates;
    private GameObject[] room_array, eligRoomArray;
    public List<GameObject> roomList;


    /// <summary>
    /// roomList is list of all rooms.  
    /// we prune this to get subset of rooms we can afford
    /// we pick one randomly to start, and note its door positions.
    /// we add the doors to a list / array, and iterate through them creating as we go. 
    /// need some way to handle room collisions.
    /// </summary>

    // Start is called before the first frame update
    void Start()
    {
        remainingRooms = maxRooms;
        templates = GameObject.FindGameObjectWithTag("Room_Template").GetComponent<RoomTemplates>();

        while (remainingRooms > 0)
        {
            GenerateEligibleRooms();
            Debug.Log("remainingRooms = " + remainingRooms + " MaxRooms = " + maxRooms);
            // create list of eligible rooms, then pick one randomly
            // room is eligible iff: roomCost <= maxRooms-curRooms && has required door connections
            // create a room randomly
            rand1 = Random.Range(1, 4);
            Debug.Log("rand1 = " + rand1);
            switch (rand1)
            {
                case 1:
                    rand2 = Random.Range(0, templates.bottomDoorRooms.Length);
                    Debug.Log("rand2 = " + rand2);
                    Instantiate(templates.bottomDoorRooms[rand2], transform.position, transform.rotation);
                    break;
                case 2:
                    rand2 = Random.Range(0, templates.topDoorRooms.Length);
                    Debug.Log("rand2 = " + rand2);
                    Instantiate(templates.topDoorRooms[rand2], transform.position, transform.rotation);
                    break;
                case 3:
                    rand2 = Random.Range(0, templates.leftDoorRooms.Length);
                    Debug.Log("rand2 = " + rand2);
                    Instantiate(templates.leftDoorRooms[rand2], transform.position, transform.rotation);
                    break;
                case 4:
                    rand2 = Random.Range(0, templates.rightDoorRooms.Length);
                    Debug.Log("rand2 = " + rand2);
                    Instantiate(templates.rightDoorRooms[rand2], transform.position, transform.rotation);
                    break;
            }
            Debug.Log("remainingRooms = " + remainingRooms + " MaxRooms = " + maxRooms);

            remainingRooms--;
            Debug.Log("curRooms = " + remainingRooms + " MaxRooms = " + maxRooms);


            //create random doors in this room between 1 and min (maxrooms-currooms,4)

        }
    }

    void GenerateEligibleRooms()
    {
        Debug.Log("Generating Rooms");
        room_array = GameObject.FindGameObjectsWithTag("Rooms");
        Debug.Log("Room ength = " + room_array.Length);
        List<GameObject> eligRoomList = new List<GameObject>();

        for (int i = 0; i < roomList.Count; i++)
        {
            /* roomCost removed, commented this so no errors
             * if (roomList[i].GetComponent<RoomProperties>().roomCost <= remainingRooms)
            {
                eligRoomList.Add(roomList[i]);
                //Debug.Log("Added Room " + roomList[i].name);
            }*/
        }

        foreach (var obj in eligRoomList)
        {
            Debug.Log(obj.name);
        }

    }

}
