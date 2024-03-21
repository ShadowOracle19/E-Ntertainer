using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatManager : MonoBehaviour
{

    public float chatSpawnTimer = 1;
    // Update is called once per frame
    void Update()
    {
        ChatSpawner();
    }

    private void ChatSpawner()
    {
        if (chatSpawnTimer > 0)//Countdown to 5 seconds
        {
            chatSpawnTimer -= Time.deltaTime;
        }
        else
        {
            if(GameManager.Instance.audience <= 25)//low level audience
            {
                float randTimer = Random.Range(8, 20);
                chatSpawnTimer = randTimer;
                GameManager.Instance.SpawnChatPopup();
            }
            else if (GameManager.Instance.audience >= 26 && GameManager.Instance.audience <= 69)//mid level audience
            {
                float randTimer = Random.Range(2, 6);
                chatSpawnTimer = randTimer;
                GameManager.Instance.SpawnChatPopup();
            }
            else if (GameManager.Instance.audience >= 70)//high level audience
            {
                float randTimer = Random.Range(0.05f, 0.3f);
                chatSpawnTimer = randTimer;
                GameManager.Instance.SpawnChatPopup();
            }
        }
    }
}
