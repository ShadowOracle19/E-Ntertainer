using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevel : MonoBehaviour
{
    public Transform[] collectibles;
    public Transform allCollect;

    public GameObject finalCutsceneUI;

    //when player touches the end point they will spawn at the spawn point
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Check if what the collision is touching is the player
        if (collision.gameObject.tag == "Player")
        {
            collectibles = new Transform[allCollect.childCount];

            for (int i = 0; i < collectibles.Length; i++)
            {
                collectibles[i] = allCollect.GetChild(i);
            }

            //TelemetryLogger.Log(this, "Final Approval", GameManager.Instance.audienceApproval);
            //TelemetryLogger.Log(this, "Final Mood", GameManager.Instance.VTuberMood);
            //TelemetryLogger.Log(this, "Final Audience", GameManager.Instance.audience);
            //TelemetryLogger.Log(this, "Time Spent", Time.time);
            //TelemetryLogger.Log(this, "Collected Collectibles", GameManager.Instance.collectiblesCount);

            foreach (Transform coin in collectibles)
            {
                //TelemetryLogger.Log(this, "Uncollected Collectible", coin.position);
            }
            
            collision.gameObject.transform.position = GameManager.Instance.spawnPoint.position;
            GameManager.Instance.audienceApproval += 1;
            //SceneManager.LoadScene("Menu");

            finalCutsceneUI.SetActive(true);
            GameManager.Instance.PlayTrueEndingCutscene(GameManager.Instance.trueEnding);
        }
    }
}
