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
    //Live2D stuff
    public Animator livie2d;
    public float idleTime = 10;

    //only variables relating to the platformer should go here
    [Header("Platformer")]
    public Transform spawnPoint;
    public GameObject player;
    [Range(0.1f, 1)]
    public float cameraMoveSpeed = 0.5f;
    public GameObject camera;

    [Header("Chat")]
    public ChatManager chatManager;
    public ChatUsername usernames;
    public ChatMessage highApprovalMessages;
    public ChatMessage generalMessages;
    public ChatMessage lowApprovalMessages;
    public ChatMessage lowMoodMessages;
    public ChatMessage highMoodMessages;
    public ChatMessage highAudienceMessages;
    public ChatMessage lowAudienceMessages;
    public ChatMessage deathMessages;
    private float audienceStatTimer = 3;
    public Color currentColor;
    public Color lMoodColor;
    public Color hMoodColor;
    public Color lAppColor;
    public Color hAppColor;
    public Color lAudColor;
    public Color hAudColor;
    public Color defaultColor;
    public Color deathColor;

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
    public bool gamePaused = false;

    [Header("Collectible")]
    public TextMeshProUGUI collectibleText;
    public Transform collectibleParent;
    public int collectiblesCount;
    public int collectiblesMax;

    [Header("Cutscenes")]
    public CutsceneSequence introCutscene;
    public CutsceneSequence lowMood;
    public CutsceneSequence lowAudience;
    public CutsceneSequence trueEnding;
    public bool statEndingPlaying = false;
    public bool cutscenePlaying = false;

    //[Header("Telemetry")]
    //public bool moodEnd = false;
    //public bool audienceEnd = false;


    private void Start()
    {
        startingCam = camView.position;
        collectiblesMax = collectibleParent.childCount;
        //print(ColorUtility.ToHtmlStringRGBA(lMoodColor));
        //PlayIntroCutscene(introCutscene);
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

        audienceApproval -= Time.deltaTime / 8;
        idleTime -= Time.deltaTime;

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
            chatManager.enabled = false;
            LowAudienceEnding(lowAudience);
        }

        if (idleTime <= 0)
        {
            livie2d.SetInteger("idle choose", Random.Range(1, 4));
            livie2d.SetTrigger("idle trigger");
            idleTime = Random.Range(8, 20);
        }
    }

    #region pause menu
    public void PauseGame()
    {

        gamePaused = true;
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        gamePaused = false;
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
            if (audienceApproval >= 20)//High approval
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
            else if (audienceApproval <= -15)//low approval
            {
                Debug.Log("Low");
                audience += 1;
            }

            /*if (audience <= 1 && audienceEnd == false)
            {
                audienceEnd = true;
                TelemetryLogger.Log(this, "Audience Ending Achieved");
            }
            */

            if (audience <= 30)
            {
                VTuberMood-=1;
            }

            //adjusts viewership UI
            ViewershipAdjust();
        }
    }

    private void Mood()
    {
        VTuberMood += ((audienceApproval * audience) / 3000) * Time.deltaTime;
        /*if (VTuberMood <= 1 && moodEnd == false)
        {
            moodEnd = true;
            TelemetryLogger.Log(this, "Mood Ending Achieved");
        }
        */
    }

    public void VTuberEmotionSwitch(float mood)
    {
        if (mood >= 70)//High mood
        {
            //VTuberImage.sprite = VTuberPositive;
            livie2d.SetBool("high mood", true);
        }
        else if (mood >= 31 && mood <= 69)//Average mood
        {
            livie2d.SetBool("high mood", false);
            livie2d.SetBool("low mood", false);
            //VTuberImage.sprite = VTuberDefault;
        }
        else if (mood <= 30)//low mood
        {
            livie2d.SetBool("low mood", true);
            //VTuberImage.sprite = VTuberNegative;
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

        popup.GetComponent<ChatPopup>().message.text = "<#" + ColorUtility.ToHtmlStringRGBA(currentColor) + ">" + _username + ":</color> " + _message;
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
            currentColor = lMoodColor;
            _message = lowMoodMessages.messages[Random.Range(0, lowMoodMessages.messages.Count)];
        }
        else if (VTuberMood >= 31 && VTuberMood <= 69)//mid level mood
        {
            currentColor = defaultColor;
            _message = generalMessages.messages[Random.Range(0, generalMessages.messages.Count)];
        }
        else if (VTuberMood >= 70)//high level mood
        {
            currentColor = hMoodColor;
            _message = highMoodMessages.messages[Random.Range(0, highMoodMessages.messages.Count)];
        }
        return _message;
    }

    private string SpawnAudienceChatpopup()
    {
        string _message = " ";
        if (audience <= 25)//low level audience
        {
            currentColor = lAudColor;
            _message = lowAudienceMessages.messages[Random.Range(0, lowAudienceMessages.messages.Count)];
        }
        else if (audience >= 26 && audience <= 70)//mid level audience
        {
            currentColor = defaultColor;
            _message = generalMessages.messages[Random.Range(0, generalMessages.messages.Count)];
        }
        else if (audience >= 70)//high level audience
        {
            currentColor = hAudColor;
            _message = highAudienceMessages.messages[Random.Range(0, highAudienceMessages.messages.Count)];
        }
        return _message;
    }

    private string SpawnApprovalChatpopup()
    {
        string _message = " ";

        if (audienceApproval >= 30)//High approval
        {
            currentColor = hAppColor;
            _message = highApprovalMessages.messages[Random.Range(0, highApprovalMessages.messages.Count)];
        }
        else if (audienceApproval >= -29 && audienceApproval <= 29)//Average approval
        {
            currentColor = defaultColor;
            _message = generalMessages.messages[Random.Range(0, generalMessages.messages.Count)];
        }
        else if (audienceApproval <= -30)//low approval
        {
            currentColor = lAppColor;
            _message = lowApprovalMessages.messages[Random.Range(0, lowApprovalMessages.messages.Count)];
        }

        return _message;
    }

    void spawnDeathChat()
    {
        GameObject popup = Instantiate(chatPopupPrefab, chatpopupParent);

        string _username = usernames.usersFirst[Random.Range(0, usernames.usersFirst.Count)] + usernames.usersSecond[Random.Range(0, usernames.usersSecond.Count)];

        string _message = deathMessages.messages[Random.Range(0, deathMessages.messages.Count)];

        popup.GetComponent<ChatPopup>().message.text = "<#" + ColorUtility.ToHtmlStringRGBA(deathColor) + ">" + _username + ":</color> " + _message;
        popup.GetComponent<RectTransform>().SetAsFirstSibling();

        chatPopups.Add(popup);
    }
    #endregion

    #region Death Respawn
    public void Death()
    {
        deathAudioSource.Play();
        audienceApproval -= 5f;
        player.GetComponentInChildren<Animator>().SetTrigger("Death");
        player.GetComponent<Rigidbody2D>().simulated = false;
        livie2d.SetTrigger("death");
        TelemetryLogger.Log(this, "Death", player.transform.position);
        if (dialogueSystem.dialogueActive)
        {
            dialogueSystem.StopCoroutine(dialogueSystem.thisCoroutine);
        }
        dialogueSystem.vtuberTalking = dialogueSystem.typeOutSpecificDialogue(dialogues.death[Random.Range(0, dialogues.death.Count)]);
        dialogueSystem.thisCoroutine = dialogueSystem.StartCoroutine(dialogueSystem.vtuberTalking);

        for(int count = 0; count < Random.Range(2,4) ; count++)
        {
            spawnDeathChat();
        }
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
            GameManager.Instance.livie2d.SetTrigger("sparkle");
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
            donationTime = ((Random.Range(15, 30) * 50) / audience); //randomly generates a window in which donations show up. higher audience = more frequent donations
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

    /*public void Speaking()
    {
        camView.position = new Vector3(camView.position.x, startingCam.y, 0f) + new Vector3(0f, Mathf.Sin(Time.time * 15f) * 4, 0f);
    }*/

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
        //player.GetComponent<PlayerMovement>().enabled = false;
        player.GetComponent<PlayerMovement>()._moveInput = Vector2.zero;
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player.GetComponent<PlayerMovement>().cutscenePlaying = true;
        GetComponent<VtuberDialogueSystem>().cutscenePlaying = true;
    }

    public void PlayIntroCutscene(CutsceneSequence cutscene)
    {
        StartCutscene();
    }

    public void PlayTrueEndingCutscene(CutsceneSequence cutscene)
    {
        StartCutscene();
        dialogueSystem.StopCoroutine(dialogueSystem.thisCoroutine);
        CutsceneManager.Instance.PlayCutscene(cutscene);
    }

    public void LowMoodEnding(CutsceneSequence cutscene)
    {
        livie2d.SetTrigger("mood cutscene");
        livie2d.SetBool("inCutscene", true);
        StartCutscene();
        TelemetryLogger.Log(this, "Mood Ending Achieved");
        //dialogueSystem.StopCoroutine(dialogueSystem.thisCoroutine);
        if (dialogueSystem.dialogueActive)
        {
            dialogueSystem.StopCoroutine(dialogueSystem.thisCoroutine);
        }
        dialogueSystem.vtuberTalking = dialogueSystem.typeOutCutsceneDialogue(cutscene.vTuberLines);
        dialogueSystem.thisCoroutine = dialogueSystem.StartCoroutine(dialogueSystem.vtuberTalking);
    }

    public void LowAudienceEnding(CutsceneSequence cutscene)
    {
        livie2d.SetTrigger("audience cutscene");
        livie2d.SetBool("inCutscene", true);
        StartCutscene();
        TelemetryLogger.Log(this, "Audience Ending Achieved");
        //dialogueSystem.StopCoroutine(dialogueSystem.thisCoroutine);
        if (dialogueSystem.dialogueActive)
        {
            dialogueSystem.StopCoroutine(dialogueSystem.thisCoroutine);
        }
        dialogueSystem.vtuberTalking = dialogueSystem.typeOutCutsceneDialogue(cutscene.vTuberLines);
        dialogueSystem.thisCoroutine = dialogueSystem.StartCoroutine(dialogueSystem.vtuberTalking);
    }

    public void EndCutscene()//play this once the cutscene endss
    {
        //player.GetComponent<PlayerMovement>().enabled = true;
        player.GetComponent<PlayerMovement>().cutscenePlaying = false;
        GetComponent<VtuberDialogueSystem>().cutscenePlaying = false;
    }

    public void EndingCutsceneFinished()//send player back to menu
    {
        SceneManager.LoadScene("Menu");
    }
    #endregion
}
