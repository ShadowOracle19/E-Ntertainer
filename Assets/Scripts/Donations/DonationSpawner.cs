using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DonationSpawner : MonoBehaviour
{
    public TextMeshProUGUI donationPopup;
    public TextMeshProUGUI dMessage;
    public CanvasGroup dCanvas;
    public float time = 0f;
    bool donationOn;

    void Start() 
    {
        dCanvas.alpha = 0f;
    }

    void Update()
    {
        DonationTime();
    }

    public void DonationSpawn(string username, string message, float money) //sets up the donation message
    {
        donationPopup.text = "<#27a9d9>" + username + "</color> has donated <#27a9d9>$" + money.ToString() +"</color>!";
        dMessage.text = "\"" + message + "\"";
        GameManager.Instance.donationAudioSource.Play();
    }
    
    public void DonationTime()
    {
        if (GameManager.Instance.donationOn) //waits out the donation time before disappearing
        {
            time += Time.deltaTime;

            if (time < 5)
            {
                dCanvas.alpha += Time.deltaTime; //transition-in animation
            }
            else
            {
                dCanvas.alpha -= Time.deltaTime; //transition-out animation

                if (dCanvas.alpha <= 0)
                {
                    GameManager.Instance.donationOn = false;
                    time = 0f;
                }
            }
        }
    }

}
