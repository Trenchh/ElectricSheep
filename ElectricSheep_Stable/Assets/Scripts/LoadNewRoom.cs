using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNewRoom : MonoBehaviour
{

    public bool activated = false;
    public string exitPoint;
    private GameObject mainCam;
    private LayerMask doorMask;
    private RaycastHit2D rayResult;
    private Vector2 vect2temp;
    SpriteRenderer rend;

    private void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera");
        doorMask = LayerMask.GetMask("Door");
        rend = gameObject.GetComponent<SpriteRenderer>();
        rend.enabled = activated;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //move the camera fixed amount based on direction
            //Vector2 vect = new Vector2(transform.position.x, transform.position.y);
            // move the player to the door exitPoint
            switch (exitPoint)
            {
                case "North":
                    // camera moves up
                    mainCam.transform.position = new Vector3(mainCam.transform.position.x, mainCam.transform.position.y + 12, mainCam.transform.position.z);
                    // player moves up
                    collision.gameObject.transform.position = new Vector3(collision.gameObject.transform.position.x, collision.gameObject.transform.position.y + 4.3f, 0);
                    // door in new room is activated
                    vect2temp = new Vector2(transform.position.x, transform.position.y + 1f);
                    //Debug.DrawRay(vect2temp, Vector2.up * 4, Color.yellow, 10f);
                    rayResult = Physics2D.Raycast(vect2temp, Vector2.up, 4f, doorMask);
                    rayResult.collider.gameObject.GetComponent<LoadNewRoom>().activated = true;
                    rayResult.collider.gameObject.GetComponent<SpriteRenderer>().enabled = true;
                    if (rayResult.transform.parent.parent.name == "Boss(Clone)")
                    {
                        Destroy(rayResult.transform.parent.gameObject);
                    }
                    break;
                case "East":
                    //camera moves right
                    mainCam.transform.position = new Vector3(mainCam.transform.position.x + 21, mainCam.transform.position.y, mainCam.transform.position.z);
                    // player moves right
                    collision.gameObject.transform.position = new Vector3(collision.gameObject.transform.position.x + 4.3f, collision.gameObject.transform.position.y, 0);
                    // door in new room is activated
                    vect2temp = new Vector2(transform.position.x + 1, transform.position.y);
                    //Debug.DrawRay(vect2temp, Vector2.right * 4, Color.blue, 10f);
                    rayResult = Physics2D.Raycast(vect2temp, Vector2.right, 4f, doorMask);
                    rayResult.collider.gameObject.GetComponent<LoadNewRoom>().activated = true;
                    rayResult.collider.gameObject.GetComponent<SpriteRenderer>().enabled = true;
                    if (rayResult.transform.parent.parent.name == "Boss(Clone)")
                    {
                        Destroy(rayResult.transform.parent.gameObject);
                    }
                    break;
                case "South":
                    // camera moves down
                    mainCam.transform.position = new Vector3(mainCam.transform.position.x, mainCam.transform.position.y - 12, mainCam.transform.position.z);
                    // player moves down
                    collision.gameObject.transform.position = new Vector3(collision.gameObject.transform.position.x, collision.gameObject.transform.position.y - 4.3f, 0);
                    // door in new room is activated
                    vect2temp = new Vector2(transform.position.x, transform.position.y - 1f);
                    //Debug.DrawRay(vect2temp, Vector2.down * 4, Color.red, 10f);
                    rayResult = Physics2D.Raycast(vect2temp, Vector2.down, 4f, doorMask);
                    rayResult.collider.gameObject.GetComponent<LoadNewRoom>().activated = true;
                    rayResult.collider.gameObject.GetComponent<SpriteRenderer>().enabled = true;
                    if (rayResult.transform.parent.parent.name == "Boss(Clone)")
                    {
                        Destroy(rayResult.transform.parent.gameObject);
                    }
                    break;
                case "West":
                    // camera moves left
                    mainCam.transform.position = new Vector3(mainCam.transform.position.x - 21, mainCam.transform.position.y, mainCam.transform.position.z);
                    // player moves left
                    collision.gameObject.transform.position = new Vector3(collision.gameObject.transform.position.x - 4.3f, collision.gameObject.transform.position.y, 0);
                    // door in new room is activated
                    vect2temp = new Vector2(transform.position.x - 1, transform.position.y);
                    //Debug.DrawRay(vect2temp, Vector2.left * 4, Color.green, 10f);
                    rayResult = Physics2D.Raycast(vect2temp, Vector2.left, 4f, doorMask);
                    rayResult.collider.gameObject.GetComponent<LoadNewRoom>().activated = true;
                    rayResult.collider.gameObject.GetComponent<SpriteRenderer>().enabled = true;
                    if (rayResult.transform.parent.parent.name == "Boss(Clone)")
                    {
                        GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");
                        for (int i = 0; i < doors.Length; i++)
                        {
                            Destroy(doors[i]);
                        }
                    }
                    break;
            }

            // decrease player's max health, reset to max
            //PlayerAttack.healthCount
            if (!activated)
            {
                FindObjectOfType<PlayerAttack>().healthMax -= 1;
                PlayerAttack.healthCount = FindObjectOfType<PlayerAttack>().healthMax;
                activated = true;
                rend.enabled = activated;
            }
        }
    }

    /*IEnumerator LoadYourAsyncScene()
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(roomToLoad);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }*/

}
