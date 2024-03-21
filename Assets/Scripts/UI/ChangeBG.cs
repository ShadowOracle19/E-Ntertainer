using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class ChangeBG : MonoBehaviour
{
    public SpriteRenderer BGimage;
    
    public Sprite tutorialBG;
    public Sprite act1BG;
    public Sprite act2BG;
    public Sprite act3BG;



    // Start is called before the first frame update
    void Start()
    {
        BGimage.sprite = tutorialBG;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("this is being checked");
        if (other.gameObject.CompareTag("act1BG"))
        {
            BGimage.sprite = act1BG;
        }

        if (other.gameObject.CompareTag("act2BG"))
        {
            BGimage.sprite = act2BG;
        }

        if (other.gameObject.CompareTag("act3BG"))
        {
            BGimage.sprite = act3BG;
        }
    }
    */

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("this is being checked");
        if (collision.gameObject.CompareTag("act1BG"))
        {
            BGimage.sprite = act1BG;
        }

        if (collision.gameObject.CompareTag("act2BG"))
        {
            BGimage.sprite = act2BG;
        }

        if (collision.gameObject.CompareTag("act3BG"))
        {
            BGimage.sprite = act3BG;
        }
    }
}
