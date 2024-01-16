using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticHazards : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Check if what the collision is touching is the player
        if (collision.gameObject.tag == "Player")
        {
            GameManager.Instance.Death();

            GameManager.Instance.SpawnChatPopup("<#8F3CE0>RandomChatter:</color> Damn Streamer get good! Play a different game!!");
            GameManager.Instance.SpawnChatPopup("<#8F3CE0>SupportiveChatter:</color> It's ok you were really close!");
            GameManager.Instance.SpawnChatPopup("<#8F3CE0>InsightJohn:</color> Lol");
        }
    }
}
