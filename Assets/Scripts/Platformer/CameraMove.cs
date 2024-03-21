using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform spawnPoint;
    public bool FirstEntered = true;

    [Header("Tutorial stuff")]
    public bool hasTutorial = false;
    public GameObject tutorialGal;

    private void Start()
    {
        GetComponentInChildren<SpriteRenderer>().gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
       
        //if (Camera.main.transform.position == new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -10))
        //    return;
        if (collision.gameObject.tag == "Player")
        {
            GameManager.Instance.spawnPoint = spawnPoint;
            GameManager.Instance.camera.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -10);
            if (FirstEntered)//checks if the player is entering this section for the first time
            {
                GameManager.Instance.audienceApproval += 4;
                FirstEntered = false;
            }
            
        }
    }


}
