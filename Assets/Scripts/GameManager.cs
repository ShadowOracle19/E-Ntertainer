using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Game Manager singleton no need to touch this
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if(_instance is null )
            {
                Debug.LogError("Game Manager is NULL");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }
    #endregion

    //Temp variable only for the POC demo
    //This should never pass -1, 0, 1
    [Range(-1, 1)]
    public int playerMood;

    public Sprite VTuberDefault;
    public Sprite VTuberPositive;
    public Sprite VTuberNegative;

    //only variables relating to the platformer should go here
    [Header("Platformer")]
    public Transform spawnPoint;

    [Header("UI")]
    public Image VTuberImage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //this keeps the player mood variable clamped to -1 and 1 so it will never go past that point
        Mathf.Clamp(playerMood, -1, 1);

        //run this statement
        VTuberEmotionSwitch(playerMood);
    }

    public void VTuberEmotionSwitch(int mood)
    {
        //Switch statement to manage the three moods this will need to be improved for the final product
        switch (mood)
        {
            //negative
            case -1:
                VTuberImage.sprite = VTuberNegative;
                break;

            //default
            case 0:
                VTuberImage.sprite = VTuberDefault;
                break;

            //poisitive
            case 1:
                VTuberImage.sprite = VTuberPositive;
                break;

            default:
                break;
        }
    }
}
