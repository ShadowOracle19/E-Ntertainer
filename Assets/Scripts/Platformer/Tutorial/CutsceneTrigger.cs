using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{
    public bool FirstEntered = true;
    public GameObject tutorialGal;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && FirstEntered)
        {
            GameManager.Instance.StartCutscene();
            tutorialGal.GetComponent<TutorialCharacterMovement>().enabled = true;
            FirstEntered = false;
        }
    }
}
