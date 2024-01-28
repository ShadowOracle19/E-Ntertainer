using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticHazards : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        //Check if what the collision is touching is the player
        if (collision.gameObject.tag == "Player")
        {
            GameManager.Instance.Death();

            GameManager.Instance.SpawnChatPopup();
            GameManager.Instance.SpawnChatPopup();
            GameManager.Instance.SpawnChatPopup();
        }
    }
}
