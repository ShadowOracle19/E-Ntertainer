using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<TutorialCharacterMovement>())
        {
            collision.gameObject.GetComponent<TutorialCharacterMovement>().jumpTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<TutorialCharacterMovement>())
        {
            collision.gameObject.GetComponent<TutorialCharacterMovement>().jumpTrigger = false;
        }
    }
}
