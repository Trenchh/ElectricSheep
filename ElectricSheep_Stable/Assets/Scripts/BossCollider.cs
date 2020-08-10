using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCollider : MonoBehaviour
{
    RoomTemplates roomTemp;

    private void Start()
    {
        roomTemp = FindObjectOfType<RoomTemplates>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // check # rooms activated
            // 
            // check # enemies killed
            //  if enemies killed = 0
            //  brackets for killed % - < 20%, <50 <
            // check max charges
            // determines how powerful the boss is
            // 
            
            //Debug.Log("activeRooms = " + roomTemp.activeRooms);
            //Debug.Log("MaxRooms = " + roomTemp.maxRooms);
            float explore = (float)roomTemp.activeRooms / (float)roomTemp.maxRooms * 100;
            int aliveEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;

            Debug.Log("You win!");
            Debug.Log("You explored " + explore.ToString("F2") + "% of the level.");
            /*Debug.Log("There were " + roomTemp.totalEnemies + " enemies and you killed " + (roomTemp.totalEnemies - aliveEnemies) + " of them.");
            if (roomTemp.totalEnemies == aliveEnemies)
            {
                Debug.Log("You learned that violence doesn't solve problems.");
            }
            else {
                Debug.Log("You Monster!");
            }*/
            
             /*   Destroy(gameObject);
                SceneManager.LoadSceneAsync("Win");
            */
        }
    }
}
