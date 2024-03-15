using System.Collections;
using System.Collections.Generic;
using TMPro;
//using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Game Manager singleton no need to touch this
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance is null)
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

    [Header("Managers")]
    public VtuberDialogueSystem dialogueSystem;

    [Header("VTuber Attributes")]
    [Range(-50, 50)]//starting approval should be zero
    public float audienceApproval;//-50 to -30 low, -29 to 29 average, 30 to 50 high
    public float audience = 50;//0-100
    public float VTuberMood = 50; //0-100
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
    public float cameraMoveSpeed = 0.5f;
    public GameObject camera;

    [Header("Chat")]
    public ChatUsername usernames;
    public ChatMessage highApprovalMessages;
    public ChatMessage generalMessages;
    public ChatMessage lowApprovalMessages;
    public ChatMessage lowMoodMessages;
    public ChatMessage highMoodMessages;
    public ChatMessage highAudienceMessages;
    public ChatMessage lowAudienceMessages;
    private float audienceStatTimer = 3;

    [Header("VTuber Dialogue")]
    public VtuberDialogues dialogues;

    [Header("Donations")]
    public DonationMessage generalDonation;
    public float money;
    public float donationTime = 10f;
    public bool donationOn;
    string dMessage;
    string dUser;

    [Header("Viewership Counter")]
    public int viewership;
    public TextMeshProUGUI viewText;

    [Header("UI")]
    public Image VTuberImage;
    public Transform chatpopupParent;
    public GameObject chatPopupPrefab;
    public Transform camView;
    public Vector3 startingCam;
    public int position = 50;
    private List<GameObject> chatPopups = new List<GameObject>();
    public TextMeshProUGUI dialogueText;
    public bool triggerAchievement = false;
    public Animator achievementPopup;

    [Header("Audio")]
    public AudioSource collectibleAudioSource;
    public AudioSource dashAudioSource;
    public AudioSource deathAudioSource;
    public AudioSource jumpAudioSource;
    public AudioSource landAudioSource;
    public AudioSource respawnAudioSource;
    public AudioSource VTuberSpeakAudioSource;
    public AudioSource walkAudioSource;
    public AudioSource donationAudioSource;

    [Header("Pause Menu")]
    public GameObject pauseMenu;
    private bool gamePaused = false;

    [Header("Collectible")]
    public TextMeshProUGUI collectibleText;
    public Transform collectibleParent;
    public int collectiblesCount;
    public int collectiblesMax;

    [Header("Cutscenes")]
    public CutsceneSequence lowMood;
    public CutsceneSequence lowAudience;
    public CutsceneSequence trueEnding;
    public bool statEndingPlaying = false;

    [Header("Telemetry")]
    public bool moodEnd = false;
    public bool audienceEnd = false;


    private void Start()
    {
        startingCam = camView.position;
        collectiblesMax = collectibleParent.childCount;
    }

    // Update is called once per frame
    void Update()
    {
        CollectibleCount();
        //Escape button to pause game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gamePaused = !gamePaused;
            if(gamePaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }

        timer += Time.deltaTime;
        donationTime -= Time.deltaTime;

        //run this statement
        VTuberEmotionSwitch(VTuberMood);

        //Calculate Audience stat
        Audience();

        //Calculate Mood Stat
        Mood();

        //plays out donations
        Donations();

        audienceApproval -= Time.deltaTime / 5;

        //rebuild vertical layout to avoid spawning messages incorrectly
        LayoutRebuilder.ForceRebuildLayoutImmediate(chatpopupParent.GetComponent<RectTransform>());
        audienceApproval = Mathf.Clamp(audienceApproval, -50, 50);
        audience = Mathf.Clamp(audience, 0, 100);
        VTuberMood = Mathf.Clamp(VTuberMood, 0, 100);

        //Achievement popup trigger
        if(collectiblesCount == collectiblesMax)
        {
            AchievementPopup();
        }

        //stat ending cutscenes 
        //low mood
        if(VTuberMood == 0 && !statEndingPlaying)
        {
            statEndingPlaying = true;
            LowMoodEnding(lowMood);
        }

        //low audience
        if(audience == 0 && !statEndingPlaying)
        {
            statEndingPlaying = true;
            LowAudienceEnding(lowAudience);
        }

    }

    #region pause menu
    public void PauseGame()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }
    #endregion

    #region stats
    private void Audience()
    {
        if (audienceStatTimer > 0)//Countdown to 5 seconds
        {
            audienceStatTimer -= Time.deltaTime;
        }
        else
        {
            audienceStatTimer = 4;
            if (audienceApproval >= 30)//High approval
            {
                Debug.Log("High");
                audience += 1;
            }
            else if (audienceApproval <= 29 && audienceApproval >= -29)//Average approval
            {
                /*Debug.Log("Average");
                int rand = Random.Range(1, 5);
                if(rand == 1)
                {*/
                audience -= 1;
                //}
            }
            else if (audienceApproval <= -30)//low approval
            {
                Debug.Log("Low");
                audience += 1;
            }

            if (audience <= 1 && audienceEnd == false)
            {
                audienceEnd = true;
                TelemetryLogger.Log(this, "Audience Ending Achieved");
            }

            //adjusts viewership UI
            ViewershipAdjust();
        }
    }

    private void Mood()
    {
        VTuberMood += ((audienceApproval * audience) / 10000) * Time.deltaTime;
        if (VTuberMood <= 1 && moodEnd == false)
        {
            moodEnd = true;
            TelemetryLogger.Log(this, "Mood Ending Achieved");
        }
    }

    public void VTuberEmotionSwitch(float mood)
    {
        if (mood >= 70)//High mood
        {
            VTuberImage.sprite = VTuberPositive;
        }
        else if (mood >= 31 && mood <= 69)//Average mood
        {
            VTuberImage.sprite = VTuberDefault;
        }
        else if (mood <= 30)//low mood
        {
            VTuberImage.sprite = VTuberNegative;
        }

    }
    #endregion

    #region Chat
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

        popup.GetComponent<ChatPopup>().message.text = "<#8F3CE0>" + _username + ":</color> " + _message;
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
        else if (audience >= 26 && audience <= 70)//mid level audience
        {
            _message = generalMessages.messages[Random.Range(0, generalMessages.messages.Count)];
        }
        else if (audience >= 70)//high level audience
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
            //Debug.Log("Positive Messages");
            _message = highApprovalMessages.messages[Random.Range(0, highApprovalMessages.messages.Count)];
        }
        else if (audienceApproval >= -29 && audienceApproval <= 29)//Average approval
        {
            //Debug.Log("Neutral Messages");
            _message = generalMessages.messages[Random.Range(0, generalMessages.messages.Count)];
        }
        else if (audienceApproval <= -30)//low approval
        {
            //Debug.Log("Negative Messages");
            _message = lowApprovalMessages.messages[Random.Range(0, lowApprovalMessages.messages.Count)];
        }

        return _message;
    }
    #endregion

    #region Death Respawn
    public void Death()
    {
        deathAudioSource.Play();
        audienceApproval -= 2.5f;
        player.GetComponentInChildren<Animator>().SetTrigger("Death");
        player.GetComponent<Rigidbody2D>().simulated = false;
        TelemetryLogger.Log(this, "Death", player.transform.position);
        if (dialogueSystem.dialogueActive)
        {
            dialogueSystem.StopCoroutine(dialogueSystem.thisCoroutine);
        }
        dialogueSystem.vtuberTalking = dialogueSystem.typeOutSpecificDialogue(dialogues.death[Random.Range(0, dialogues.death.Count)]);
        dialogueSystem.thisCoroutine = dialogueSystem.StartCoroutine(dialogueSystem.vtuberTalking);
    }
    public void Respawn()
    {
        player.transform.position = spawnPoint.position;
        respawnAudioSource.Play();
    }
    #endregion

    #region collectables
    public void CollectibleCount()
    {
        collectibleText.text = collectiblesCount + "/" + collectiblesMax;
    }

    public void collectYarn()
    {
        collectiblesCount += 1;
        audienceApproval += 1;
        collectibleAudioSource.Play();
        if(!dialogueSystem.dialogueActive)
        {
            dialogueSystem.vtuberTalking = dialogueSystem.typeOutSpecificDialogue(dialogues.collectible[Random.Range(0, dialogues.collectible.Count)]);
            dialogueSystem.thisCoroutine = dialogueSystem.StartCoroutine(dialogueSystem.vtuberTalking);

        }
    }
    #endregion

    #region Donations
    public void Donations()
    {
        //waits out the time until next donation and checks if another donation is already running before running a new one
        if (donationTime <= 0 && donationOn == false)
        {
            if (audienceApproval <= -20)
            {
                LowApprovalDonations();
            }
            else if (audienceApproval > -20 && audienceApproval < 20)
            {
                GeneralDonations();
            }
            else if (audienceApproval >= 20)
            {
                HighApprovalDonations();
            }
            donationOn = true;
            donationTime = ((Random.Range(10, 30) * 50) / audience); //randomly generates a window in which donations show up. higher audience = more frequent donations
        }
    }

    //general donation message generated
    public void GeneralDonations()
    {
        money = Random.Range(100, 5000) / 100;
        dMessage = generalDonation.messages[Random.Range(0, generalDonation.messages.Count)]; //placeholder
        dUser = usernames.usersFirst[Random.Range(0, usernames.usersFirst.Count)] + usernames.usersSecond[Random.Range(0, usernames.usersSecond.Count)];
        GetComponent<DonationSpawner>().DonationSpawn(dUser, dMessage, money);
    }

    public void HighApprovalDonations()
    {
        money = Random.Range(500, 10000) / 100;
        dMessage = generalDonation.messages[Random.Range(0, generalDonation.messages.Count)];
        dUser = usernames.usersFirst[Random.Range(0, usernames.usersFirst.Count)] + usernames.usersSecond[Random.Range(0, usernames.usersSecond.Count)];
        GetComponent<DonationSpawner>().DonationSpawn(dUser, dMessage, money);
    }
    

    public void LowApprovalDonations()
    {
        money = Random.Range(100, 500) / 100;
        dMessage = generalDonation.messages[Random.Range(0, generalDonation.messages.Count)]; //placeholder
        dUser = usernames.usersFirst[Random.Range(0, usernames.usersFirst.Count)] + usernames.usersSecond[Random.Range(0, usernames.usersSecond.Count)];
        GetComponent<DonationSpawner>().DonationSpawn(dUser, dMessage, money);
    }
    #endregion

    #region viewers
    public void ViewershipAdjust()
    {
        if (audience >= 80)
        {
            viewership = Mathf.RoundToInt((ViewCalcuation() * Mathf.Lerp(1, 10, ((audience - 80) / 20))));
        }
        else
        {
            viewership = Mathf.RoundToInt(ViewCalcuation());
        }
        viewText.text = viewership.ToString();
    }

    private float ViewCalcuation()
    {
        float views = Random.Range((audience * audience * 0.95f), (audience * audience * 1.10f));
        return views;
    }
    #endregion

    public void Speaking()
    {
        camView.position = new Vector3(camView.position.x, startingCam.y, 0f) + new Vector3(0f, Mathf.Sin(Time.time * 15f) * 4, 0f);
    }

    public void AchievementPopup()
    {
        if(triggerAchievement)
        {
            return;
        }    
        triggerAchievement = true;
        achievementPopup.SetTrigger("achievement");
    }


    #region cutscene
    public void StartCutscene()//play this to start cutscene
    {
        player.GetComponent<PlayerMovement>().enabled = false;
        GetComponent<VtuberDialogueSystem>().cutscenePlaying = true;
        dialogueSystem.StopCoroutine(dialogueSystem.thisCoroutine);
    }

    public void PlayTrueEndingCutscene(CutsceneSequence cutscene)
    {
        StartCutscene();
        CutsceneManager.Instance.PlayCutscene(cutscene);
    }

    public void LowMoodEnding(CutsceneSequence cutscene)
    {
        StartCutscene();
        if (dialogueSystem.dialogueActive)
        {
            dialogueSystem.StopCoroutine(dialogueSystem.thisCoroutine);
        }
        dialogueSystem.vtuberTalking = dialogueSystem.typeOutCutsceneDialogue(cutscene.vTuberLines);
        dialogueSystem.thisCoroutine = dialogueSystem.StartCoroutine(dialogueSystem.vtuberTalking);
    }

    public void LowAudienceEnding(CutsceneSequence cutscene)
    {
        StartCutscene();
        if (dialogueSystem.dialogueActive)
        {
            dialogueSystem.StopCoroutine(dialogueSystem.thisCoroutine);
        }
        dialogueSystem.vtuberTalking = dialogueSystem.typeOutCutsceneDialogue(cutscene.vTuberLines);
        dialogueSystem.thisCoroutine = dialogueSystem.StartCoroutine(dialogueSystem.vtuberTalking);
    }

    public void EndCutscene()//play this once the cutscene ends
    {
        player.GetComponent<PlayerMovement>().enabled = true;
        GetComponent<VtuberDialogueSystem>().cutscenePlaying = false;
    }

    public void EndingCutsceneFinished()//send player back to menu
    {
        SceneManager.LoadScene("Menu");
    }
    #endregion
}
