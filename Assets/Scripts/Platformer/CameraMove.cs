using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform spawnPoint;
    public bool FirstEntered = true;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Camera.main.transform.position == new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -10))
            return;
        if (collision.gameObject.tag == "Player")
        {
            if(FirstEntered)//checks if the player is entering this section for the first time
            {
                GameManager.Instance.audienceApproval += 5;
                FirstEntered = false;
            }
            StopAllCoroutines();
            GameManager.Instance.spawnPoint = spawnPoint;
            //Camera.main.gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -10);
            StartCoroutine(MoveCam(GameManager.Instance.CameraMoveSpeed));
        }
    }

    private IEnumerator MoveCam(float time)
    {
        Vector3 startingPos = Camera.main.transform.position;
        Vector3 finalPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -10);

        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            Camera.main.transform.position = Vector3.Slerp(startingPos, finalPos, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

}
