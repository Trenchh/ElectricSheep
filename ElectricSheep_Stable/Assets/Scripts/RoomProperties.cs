using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomProperties : MonoBehaviour
{
    // How many enemies to spawn (in points)
    public int enemyBudget;

    public bool activated = false;
    RoomTemplates roomTemp;

    private void Start()
    {
        roomTemp = FindObjectOfType<RoomTemplates>();
    }
    private void Update()
    {
        if (!activated)
        {
            foreach (LoadNewRoom scr in GetComponentsInChildren<LoadNewRoom>())
            {
                if (scr.activated)
                {
                    if (!activated)
                    {
                        activated = true;
                        roomTemp.activeRooms += 1;
                    }
                }
            }
        }
    }

}
