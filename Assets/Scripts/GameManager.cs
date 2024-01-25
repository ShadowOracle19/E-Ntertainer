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

    
    [Header("VTuber Attributes")]
    [Range(-50, 50)]//starting approval should be zero
    public float audienceApproval;//-50 to -30 low, -29 to 29 average, 30 to 50 high
    public float audience;//0-100
    public float VTuberMood;
    public float timer;
    //These sprites will eventually change
    public Sprite VTuberDefault;
    public Sprite VTuberPositive;
    public Sprite VTuberNegative;

    //only variables relating to the platformer should go here
    [Header("Platformer")]
    public Transform spawnPoint;
    public GameObject player;
    [Range(0.1f, 1)]
    public float CameraMoveSpeed = 0.5f;

    [Header("Chat")]
    public ChatUsername usernames;
    public ChatMessage highApprovalMessages;
    public ChatMessage generalMessages;
    public ChatMessage lowApprovalMessages;
    public ChatMessage lowMoodMessages;
    public ChatMessage highMoodMessages;
    public ChatMessage highAudienceMessages;
    public ChatMessage lowAudienceMessages;
    private float audienceStatTimer = 5;

    [Header("UI")]
    public Image VTuberImage;
    public Transform chatpopupParent;
    public GameObject chatPopupPrefab;
    public int position = 50;
    private List<GameObject> chatPopups = new List<GameObject>();

    [Header("Audio")]
    public AudioSource collectibleAudioSource;
    public AudioSource dashAudioSource;
    public AudioSource deathAudioSource;
    public AudioSource jumpAudioSource;


    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        //run this statement
        VTuberEmotionSwitch(audienceApproval);

        //Calculate Audience stat
        Audience();

        //Calculate Mood Stat
        Mood();

        //rebuild vertical layout to avoid spawning messages incorrectly
        LayoutRebuilder.ForceRebuildLayoutImmediate(chatpopupParent.GetComponent<RectTransform>());
        audienceApproval = Mathf.Clamp(audienceApproval, -50, 50);
        audience = Mathf.Clamp(audience, 0, 100);
    }

    private void Audience()
    {
        if (audienceStatTimer > 0)//Countdown to 5 seconds
        {
            audienceStatTimer -= Time.deltaTime;
        }
        else
        {
            audienceStatTimer = 5;
            if (audienceApproval >= 30)//High approval
            {
                Debug.Log("High");
                audience += 1;
            }
            else if (audienceApproval <= 29 && audienceApproval >= -29)//Average approval
            {
                Debug.Log("Average");
                int rand = Random.Range(1, 5);
                if(rand == 1)
                {
                    audience -= 1;
                }
            }
            else if (audienceApproval <= -30)//low approval
            {
                Debug.Log("Low");
                audience += 1;
            }
        }
    }

    private void Mood()
    {
        VTuberMood += ((audienceApproval * audience) / 1000) * Time.deltaTime;
    }

    public void VTuberEmotionSwitch(float approval)
    {
        if(approval >= 30)//High approval
        {
            VTuberImage.sprite = VTuberPositive;
        }
        else if(approval >= -29 && approval <= 29)//Average approval
        {
            VTuberImage.sprite = VTuberDefault;
        }
        else if(approval <= -30)//low approval
        {
            VTuberImage.sprite = VTuberNegative;
        }
        
    }

    public void SpawnChatPopup()
    {
        //create the popup 
        GameObject popup = Instantiate(chatPopupPrefab, chatpopupParent);

        string _username = usernames.usersFirst[Random.Range(0, usernames.usersFirst.Count)] + usernames.usersSecond[Random.Range(0, usernames.usersSecond.Count)];

        int rand = Random.Range(1, 4);

        string chosenMessageType = "";
        switch (rand)
        {
            case 1:
                chosenMessageType = SpawnApprovalChatpopup();
                break;
            case 2:
                chosenMessageType = SpawnMoodChatpopup();
                break;
            case 3:
                chosenMessageType = SpawnAudienceChatpopup();
                break;
            default:
                break;
        }


        string _message = chosenMessageType;

        popup.GetComponent<ChatPopup>().message.text = "<#8F3CE0>"+_username + ":</color> " + _message;
        popup.GetComponent<RectTransform>().SetAsFirstSibling();
        
        chatPopups.Add(popup);
        //foreach (var _popup in chatPopups)
        //{
        //    _popup.GetComponent<RectTransform>().anchoredPosition += Vector2.up * position;
        //}
    }

    private string SpawnMoodChatpopup()
    {
        string _message = " ";
        if (VTuberMood <= 30)//low level mood
        {
            _message = lowMoodMessages.messages[Random.Range(0, lowMoodMessages.messages.Count)];
        }
        else if (VTuberMood >= 31 && VTuberMood <= 69)//mid level mood
        {
            _message = generalMessages.messages[Random.Range(0, generalMessages.messages.Count)];
        }
        else if (VTuberMood >= 70)//high level mood
        {
            _message = highMoodMessages.messages[Random.Range(0, highMoodMessages.messages.Count)];
        }
        return _message;
    }

    private string SpawnAudienceChatpopup()
    {
        string _message = " ";
        if (audience <= 25)//low level audience
        {
            _message = lowAudienceMessages.messages[Random.Range(0, lowAudienceMessages.messages.Count)];
        }
        else if (audience >= 26 && audience <= 50)//mid level audience
        {
            _message = generalMessages.messages[Random.Range(0, generalMessages.messages.Count)];
        }
        else if (audience >= 51)//high level audience
        {
            _message = highAudienceMessages.messages[Random.Range(0, highAudienceMessages.messages.Count)];
        }
        return _message;
    }

    private string SpawnApprovalChatpopup()
    {
        string _message = " ";

        if (audienceApproval >= 30)//High approval
        {
            Debug.Log("Positive Messages");
            _message = highApprovalMessages.messages[Random.Range(0, highApprovalMessages.messages.Count)];
        }
        else if (audienceApproval >= -29 && audienceApproval <= 29)//Average approval
        {
            Debug.Log("Neutral Messages");
            _message = generalMessages.messages[Random.Range(0, generalMessages.messages.Count)];
        }
        else if (audienceApproval <= -30)//low approval
        {
            Debug.Log("Negative Messages");
            _message = lowApprovalMessages.messages[Random.Range(0, lowApprovalMessages.messages.Count)];
        }

        return _message;
    }

    public void Death()
    {
        deathAudioSource.Play();
        audienceApproval -= 5;
        player.transform.position = spawnPoint.position;
    }
}
