using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.Instance.audienceApproval += 1;
        GameManager.Instance.collectibleAudioSource.Play();
        Destroy(gameObject);
    }
}
