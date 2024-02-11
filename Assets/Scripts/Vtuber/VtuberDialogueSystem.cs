using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VtuberDialogueSystem : MonoBehaviour
{
    public float dialogueActiveTime = 5;
    public float dialogueSpawnTimer;
    public float dialogueSpawnTimerMax = 10;
    public bool dialogueActive = false;

    public string message1;
    public string message2;

    public IEnumerator vtuberTalking;

    // Start is called before the first frame update
    void Start()
    {
        dialogueSpawnTimer = dialogueSpawnTimerMax;
    }

    // Update is called once per frame
    void Update()
    {
        DialogueSpawner();
    }

    private void DialogueSpawner()
    {
        if (dialogueActive) return;
        if (dialogueSpawnTimer > 0)//Countdown to 10 seconds
        {
            dialogueSpawnTimer -= Time.deltaTime;
        }
        else
        {
            #region dialogue selector
            //pick first dialogue
            int rand = Random.Range(1, 4);

            //string message1 = "";
            switch (rand)
            {
                case 1:
                    message1 = MoodDialogue();
                    break;
                case 2:
                    message1 = AudienceDialogue();
                    break;
                case 3:
                    message1 = ApprovalDialogue();
                    break;
                default:
                    break;
            }

            //pick second dialogue
            rand = Random.Range(1, 4);

            //string message2 = "";
            switch (rand)
            {
                case 1:
                    message2 = MoodDialogue();
                    if (message1 == message2) message2 = MoodDialogue();
                    break;
                case 2:
                    message2 = AudienceDialogue();
                    if (message1 == message2) message2 = AudienceDialogue();
                    break;
                case 3:
                    message2 = ApprovalDialogue();
                    if (message1 == message2) message2 = ApprovalDialogue();
                    break;
                default:
                    break;
            }
            #endregion

            vtuberTalking = typeOutDialogue(message1, message2);

            StartCoroutine(vtuberTalking); //play first dialogue
        }
    }


    //types out random spawning Dialogue
    IEnumerator typeOutDialogue(string dialogue1, string dialogue2)
    {
        Debug.Log("How many times is this run");
        dialogueActive = true;
        GameManager.Instance.dialogueText.gameObject.SetActive(true);
        GameManager.Instance.dialogueText.text = ""; //clear dialogue text

        //type out each letter and make the vtuber sound effect play
        foreach(char letter in dialogue1)
        {
            GameManager.Instance.dialogueText.text += letter.ToString();
            GameManager.Instance.VTuberSpeakAudioSource.pitch = Random.Range(1.0f, 1.5f);//variable pitch
            GameManager.Instance.VTuberSpeakAudioSource.Play();
            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(dialogueActiveTime);

        GameManager.Instance.dialogueText.text = ""; //clear dialogue text

        //type out each letter and make the vtuber sound effect play for second dialogue
        foreach (char letter in dialogue2)
        {
            GameManager.Instance.dialogueText.text += letter.ToString();
            GameManager.Instance.VTuberSpeakAudioSource.pitch = Random.Range(1.0f, 1.5f);//variable pitch
            GameManager.Instance.VTuberSpeakAudioSource.Play();
            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(dialogueActiveTime);

        GameManager.Instance.dialogueText.gameObject.SetActive(false);
        GameManager.Instance.dialogueText.text = ""; //clear dialogue text
        dialogueActive = false;
        dialogueSpawnTimer = dialogueSpawnTimerMax;//reset timer
        yield return null; 
    }

    //types out specific dialogue from anywhere
    //This should override text natrually generated from this script
    public IEnumerator typeOutSpecificDialogue(string dialogue)
    {
        if (GameManager.Instance.dialogueSystem.dialogueActive) StopCoroutine(vtuberTalking);
        dialogueActive = true;
        GameManager.Instance.dialogueText.gameObject.SetActive(true);
        GameManager.Instance.dialogueText.text = ""; //clear dialogue text

        //type out each letter and make the vtuber sound effect play
        foreach (char letter in dialogue)
        {
            GameManager.Instance.dialogueText.text += letter.ToString();
            GameManager.Instance.VTuberSpeakAudioSource.pitch = Random.Range(1.0f, 1.5f);//variable pitch
            GameManager.Instance.VTuberSpeakAudioSource.Play();
            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(dialogueActiveTime);

        GameManager.Instance.dialogueText.gameObject.SetActive(false);
        GameManager.Instance.dialogueText.text = ""; //clear dialogue text
        dialogueActive = false;
        dialogueSpawnTimer = dialogueSpawnTimerMax;//reset timer
        yield return null;
    }

    private string MoodDialogue()
    {
        string _message = " ";
        if (GameManager.Instance.VTuberMood <= 30)//low level mood
        {
            _message = GameManager.Instance.dialogues.lowMood[Random.Range(0, GameManager.Instance.dialogues.lowMood.Count)];
        }

        else if (GameManager.Instance.VTuberMood >= 31 && GameManager.Instance.VTuberMood <= 69)//general
        {
            _message = GameManager.Instance.dialogues.general[Random.Range(0, GameManager.Instance.dialogues.general.Count)];
        }

        else if (GameManager.Instance.VTuberMood >= 70)//high level mood
        {
            _message = GameManager.Instance.dialogues.highMood[Random.Range(0, GameManager.Instance.dialogues.highMood.Count)];
        }
        return _message;
    }

    private string AudienceDialogue()
    {
        string _message = " ";
        if (GameManager.Instance.audience <= 25)//low level audience
        {
            _message = GameManager.Instance.dialogues.lowAudience[Random.Range(0, GameManager.Instance.dialogues.lowAudience.Count)];
        }
        else if (GameManager.Instance.audience >= 26 && GameManager.Instance.audience <= 70)//general
        {
            _message = GameManager.Instance.dialogues.general[Random.Range(0, GameManager.Instance.dialogues.general.Count)];
        }
        else if (GameManager.Instance.audience >= 70)//high level audience
        {
            _message = GameManager.Instance.dialogues.highAudience[Random.Range(0, GameManager.Instance.dialogues.highAudience.Count)];
        }
        return _message;
    }

    private string ApprovalDialogue()
    {
        string _message = " ";

        if (GameManager.Instance.audienceApproval >= 30)//High approval
        {
            Debug.Log("Positive Messages");
            _message = GameManager.Instance.dialogues.highApproval[Random.Range(0, GameManager.Instance.dialogues.highApproval.Count)];
        }
        else if (GameManager.Instance.audienceApproval >= -29 && GameManager.Instance.audienceApproval <= 29)//general
        {
            Debug.Log("Neutral Messages");
            _message = GameManager.Instance.dialogues.general[Random.Range(0, GameManager.Instance.dialogues.general.Count)];
        }
        else if (GameManager.Instance.audienceApproval <= -30)//low approval
        {
            Debug.Log("Negative Messages");
            _message = GameManager.Instance.dialogues.lowApproval[Random.Range(0, GameManager.Instance.dialogues.lowApproval.Count)];
        }

        return _message;
    }
}
