using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathRespawnAnim : MonoBehaviour
{
    

    public void Respawn()
    {
        GameManager.Instance.Respawn();
        gameObject.GetComponentInParent<Rigidbody2D>().simulated = true;
    }
}
