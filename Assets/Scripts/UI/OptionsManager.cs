using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OptionsManager : MonoBehaviour
{
    [Header("Volume")]
    [Range(0, 10)]
    public int musicSFXLevel = 10;
    [Range(0, 10)]
    public int gameSFXLevel = 10;
    [Range(0, 10)]
    public int vtuberSFXLevel = 10;


    [Header("UI")]
    //Volume
    public TextMeshProUGUI musicLevelText;
    public TextMeshProUGUI gameLevelText;
    public TextMeshProUGUI vtuberLevelText;
    public GameObject musicLevelRaiseButton;
    public GameObject musicLevelLowerButton;
    public GameObject gameLevelRaiseButton;
    public GameObject gameLevelLowerButton;
    public GameObject vtuberLevelRaiseButton;
    public GameObject vtuberLevelLowerButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Volume
        musicSFXLevel = Mathf.Clamp(musicSFXLevel, 0, 10);
        gameSFXLevel = Mathf.Clamp(gameSFXLevel, 0, 10);
        vtuberSFXLevel = Mathf.Clamp(gameSFXLevel, 0, 10);

        musicLevelText.text = musicSFXLevel.ToString();
        gameLevelText.text = gameSFXLevel.ToString();
        vtuberLevelText.text = vtuberSFXLevel.ToString();

        Volume();

        #region volume raise lower buttons
        if (musicSFXLevel > 0)
        {
            musicLevelLowerButton.SetActive(true);
        }
        else if(musicSFXLevel <= 0)
        {
            musicLevelLowerButton.SetActive(false);
        }

        if (musicSFXLevel >= 10)
        {
            musicLevelRaiseButton.SetActive(false);
        }
        else if(musicSFXLevel < 10)
        {
            musicLevelRaiseButton.SetActive(true);
        }

        if (gameSFXLevel > 0)
        {
            gameLevelLowerButton.SetActive(true);
        }
        else if(gameSFXLevel <= 0)
        {
            gameLevelLowerButton.SetActive(false);
        }

        if (gameSFXLevel >= 10)
        {
            gameLevelRaiseButton.SetActive(false);
        }
        else if(gameSFXLevel < 10)
        {
            gameLevelRaiseButton.SetActive(true);
        }

        //Vtuber volume
        if (vtuberSFXLevel > 0)
        {
            vtuberLevelLowerButton.SetActive(true);
        }
        else if (vtuberSFXLevel <= 0)
        {
            vtuberLevelLowerButton.SetActive(false);
        }

        if (vtuberSFXLevel >= 10)
        {
            vtuberLevelRaiseButton.SetActive(false);
        }
        else if (vtuberSFXLevel < 10)
        {
            vtuberLevelRaiseButton.SetActive(true);
        }

        #endregion
    }

    #region raise lower volume functions
    public void RaiseMusicVolume()
    {
        musicSFXLevel += 1;
    }

    public void LowerMusicVolume()
    {
        musicSFXLevel -= 1;
    }

    public void RaiseGameVolume()
    {
        gameSFXLevel += 1;
    }

    public void LowerGameVolume()
    {
        gameSFXLevel -= 1;
    }

    public void RaiseVtuberVolume()
    {
        vtuberSFXLevel += 1;
    }

    public void LowerVtuberVolume()
    {
        vtuberSFXLevel -= 1;
    }

    public void Volume()
    {
        //Game SFX
        GameManager.Instance.collectibleAudioSource.volume = (float)gameSFXLevel/10;
        GameManager.Instance.dashAudioSource.volume = (float)gameSFXLevel / 10;
        GameManager.Instance.deathAudioSource.volume = (float)gameSFXLevel / 10;
        GameManager.Instance.jumpAudioSource.volume = (float)gameSFXLevel / 10;
        GameManager.Instance.landAudioSource.volume = (float)gameSFXLevel / 10;
        GameManager.Instance.respawnAudioSource.volume = (float)gameSFXLevel / 10;
        GameManager.Instance.walkAudioSource.volume = (float)gameSFXLevel / 10;
        GameManager.Instance.donationAudioSource.volume = (float)gameSFXLevel / 10;

        //Voice
        GameManager.Instance.VTuberSpeakAudioSource.volume = (float)vtuberSFXLevel / 10;

        //Music

    }
    #endregion
}
