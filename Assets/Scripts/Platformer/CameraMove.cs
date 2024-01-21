using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform spawnPoint;

    private void Start()
    {
        GetComponentInChildren<SpriteRenderer>().gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Camera.main.transform.position == new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -10))
            return;
        if (collision.gameObject.tag == "Player")
        {
            StopAllCoroutines();
            GameManager.Instance.spawnPoint = spawnPoint;
            //Camera.main.gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -10);
            StartCoroutine(MoveCam(GameManager.Instance.CameraMoveSpeed));
        }
    }

    //private IEnumerator MoveCam()
    //{
    //    Debug.Log("We have entered the move camera coroutine");
    //    while(true)
    //    {
    //        if (Camera.main.transform.position == new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -10))
    //        {
    //            break;
    //        }
    //        Debug.Log("We have passed the if statement");
    //        Camera.main.gameObject.transform.position = Vector3.MoveTowards(Camera.main.gameObject.transform.position,
    //            new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -10), 10 * Time.deltaTime);

    //    }
    //    Debug.Log("We have arrived");
    //    yield return null;
    //}

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
