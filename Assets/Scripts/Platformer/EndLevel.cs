using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevel : MonoBehaviour
{
    //when player touches the end point they will spawn at the spawn point
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Check if what the collision is touching is the player
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.position = GameManager.Instance.spawnPoint.position;
            GameManager.Instance.audienceApproval += 1;

            GameManager.Instance.SpawnChatPopup();
            GameManager.Instance.SpawnChatPopup();
            GameManager.Instance.SpawnChatPopup();
        }
    }
}
