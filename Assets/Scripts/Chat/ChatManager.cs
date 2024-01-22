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
                float randTimer = Random.Range(2, 10);
                chatSpawnTimer = randTimer;
                int rand = Random.Range(1, 3);
                for (int i = 0; i < rand; i++)
                {
                    GameManager.Instance.SpawnChatPopup();
                }
            }
            else if (GameManager.Instance.audience >= 26 && GameManager.Instance.audience <= 50)//mid level audience
            {
                float randTimer = Random.Range(1, 5);
                chatSpawnTimer = randTimer;
                int rand = Random.Range(2, 4);
                for (int i = 0; i < rand; i++)
                {
                    GameManager.Instance.SpawnChatPopup();
                }
            }
            else if (GameManager.Instance.audience >= 51)//high level audience
            {
                float randTimer = Random.Range(0.5f, 2);
                chatSpawnTimer = randTimer;
                int rand = Random.Range(3, 6);
                for (int i = 0; i < rand; i++)
                {
                    GameManager.Instance.SpawnChatPopup();
                }
            }
        }
    }
}
