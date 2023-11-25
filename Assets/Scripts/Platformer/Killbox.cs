using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killbox : MonoBehaviour
{
    //when player touches kill box send player back to spawn point and decrease player mood
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Check if what the collision is touching is the player
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.position = GameManager.Instance.spawnPoint.position;
            GameManager.Instance.playerMood -= 1;
        }
    }
}
