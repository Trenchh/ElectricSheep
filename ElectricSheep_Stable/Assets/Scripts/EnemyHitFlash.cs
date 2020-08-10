using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitFlash : MonoBehaviour
{
    Color old;
    // Start is called before the first frame update
    void Start()
    {
        old = new Color(1, 1, 1, 1);
    }

    IEnumerator Flasher(Collider2D enemy) 
    {
        for (int i = 0; i < 2; i++)
        {
            enemy.GetComponent<SpriteRenderer>().color = Color.magenta;
            yield return new WaitForSeconds(.1f);
            enemy.GetComponent<SpriteRenderer>().color = old;
            yield return new WaitForSeconds(.1f);
        }
    }

    public void Hit(Collider2D enemy) {
        StartCoroutine(Flasher(enemy));
    }
}
