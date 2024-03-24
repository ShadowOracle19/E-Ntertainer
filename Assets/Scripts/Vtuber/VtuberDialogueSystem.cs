using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VtuberDialogueSystem : MonoBehaviour
{
    public float dialogueActiveTime = 8;
    public float dialogueSpawnTimer;
    public float timeWaited = 0.025f;
    public float dialogueSpawnTimerMax = 15;
    public bool dialogueActive = false;
    public int dialogueVoice = 4;

    public string message1;
    public string message2;

    public IEnumerator vtuberTalking;
    public Coroutine thisCoroutine;
    public bool cutscenePlaying = false;
    // Start is called before the first frame update
    void Start()
    {
        dialogueSpawnTimer = dialogueSpawnTimerMax;
    }

    // Update is called once per frame
    void Update()
    {
        if(!cutscenePlaying)
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
            DialogueSelector(); 

            vtuberTalking = typeOutDialogue();

            thisCoroutine = StartCoroutine(vtuberTalking); //play first dialogue

            
        }
    }

    //types out random spawning Dialogue
    IEnumerator typeOutDialogue()
    {
        dialogueActive = true;
        GameManager.Instance.dialogueText.gameObject.SetActive(true);
        GameManager.Instance.dialogueText.text = string.Empty; //clear dialogue text

        GameManager.Instance.livie2d.SetBool("speaking", true);

        //type out each letter and make the vtuber sound effect play
        foreach(char letter in message1)
        {
            GameManager.Instance.dialogueText.text += letter.ToString();

            DialogueVoice();

            yield return new WaitForSeconds(timeWaited);
        }

        //Debug.Log("Hi");
        GameManager.Instance.livie2d.SetBool("speaking", false);

        yield return new WaitForSeconds(dialogueActiveTime);

        GameManager.Instance.livie2d.SetBool("speaking", true);

        GameManager.Instance.dialogueText.text = string.Empty; //clear dialogue text

        //type out each letter and make the vtuber sound effect play for second dialogue
        foreach (char letter in message2)
        {
            GameManager.Instance.dialogueText.text += letter.ToString();

            DialogueVoice();

            yield return new WaitForSeconds(timeWaited);
        }

        GameManager.Instance.livie2d.SetBool("speaking", false);
        yield return new WaitForSeconds(dialogueActiveTime);

        EndDialogue();
        yield return null; 
    }

    //types out specific dialogue from anywhere
    //This should override text natrually generated from this script
    public IEnumerator typeOutSpecificDialogue(string dialogue)
    {
        dialogueActive = true;
        GameManager.Instance.dialogueText.gameObject.SetActive(true);
        GameManager.Instance.dialogueText.text = string.Empty; //clear dialogue text
        GameManager.Instance.livie2d.SetBool("speaking", true);

        //type out each letter and make the vtuber sound effect play
        foreach (char letter in dialogue)
        {
            GameManager.Instance.dialogueText.text += letter.ToString();

            DialogueVoice();

            yield return new WaitForSeconds(timeWaited);
        }

        GameManager.Instance.livie2d.SetBool("speaking", false);
        yield return new WaitForSeconds(dialogueActiveTime);

        EndDialogue();
        yield return null;
    }


    //takes an array then writes out the dialogue until the array finishes
    public IEnumerator typeOutCutsceneDialogue(string[] dialogue)
    {
        dialogueActive = true;
        GameManager.Instance.dialogueText.gameObject.SetActive(true);
        GameManager.Instance.dialogueText.text = string.Empty; //clear dialogue text

        for (int i = 0; i < dialogue.Length; i++)//iterate through the string array
        {
            GameManager.Instance.livie2d.SetBool("speaking", true);
            //type out each letter and make the vtuber sound effect play
            foreach (char letter in dialogue[i])
            {
                GameManager.Instance.dialogueText.text += letter.ToString();

                DialogueVoice();
                
                yield return new WaitForSeconds(timeWaited);
            }
            GameManager.Instance.livie2d.SetBool("speaking", false);
            yield return new WaitForSeconds(dialogueActiveTime);
            GameManager.Instance.dialogueText.text = string.Empty; //clear dialogue text
        }

        EndDialogue();
        GameManager.Instance.EndingCutsceneFinished();
        yield return null;
    }

    private void EndDialogue()
    {
        GameManager.Instance.dialogueText.gameObject.SetActive(false);
        GameManager.Instance.dialogueText.text = string.Empty; //clear dialogue text
        dialogueActive = false;
        dialogueSpawnTimer = dialogueSpawnTimerMax;//reset timer
    }

    private void DialogueVoice()
    {
        //GameManager.Instance.Speaking();
        dialogueVoice--;
        if (dialogueVoice == 0)
        {
            GameManager.Instance.VTuberSpeakAudioSource.pitch = Random.Range(1.0f, 1.5f);//variable pitch
            GameManager.Instance.VTuberSpeakAudioSource.Play();
            dialogueVoice = 4;
        }
    }

    private void DialogueSelector()
    {
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
            //Debug.Log("Positive Messages");
            _message = GameManager.Instance.dialogues.highApproval[Random.Range(0, GameManager.Instance.dialogues.highApproval.Count)];
        }
        else if (GameManager.Instance.audienceApproval >= -29 && GameManager.Instance.audienceApproval <= 29)//general
        {
            //Debug.Log("Neutral Messages");
            _message = GameManager.Instance.dialogues.general[Random.Range(0, GameManager.Instance.dialogues.general.Count)];
        }
        else if (GameManager.Instance.audienceApproval <= -30)//low approval
        {
            //Debug.Log("Negative Messages");
            _message = GameManager.Instance.dialogues.lowApproval[Random.Range(0, GameManager.Instance.dialogues.lowApproval.Count)];
        }

        return _message;
    }
}
