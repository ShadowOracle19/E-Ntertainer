using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VtuberDialogueSystem : MonoBehaviour
{
    public float dialogueActiveTime = 5;
    public float dialogueSpawnTimer = 10;
    public bool dialogueActive = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(typeOutDialogue(MoodDialogue()));
    }

    // Update is called once per frame
    void Update()
    {
        ChatSpawner();
    }

    private void ChatSpawner()
    {
        if (dialogueSpawnTimer > 0)//Countdown to 10 seconds
        {
            dialogueSpawnTimer -= Time.deltaTime;
        }
        else
        {
            int rand = Random.Range(1, 4);

            string chosenMessageType1 = "";
            switch (rand)
            {
                case 1:
                    chosenMessageType1 = MoodDialogue();
                    break;
                case 2:
                    chosenMessageType1 = AudienceDialogue();
                    break;
                case 3:
                    chosenMessageType1 = ApprovalDialogue();
                    break;
                default:
                    break;
            }


            StartCoroutine(typeOutDialogue(chosenMessageType1));
            Debug.Log("do i get here");
        }
    }

    IEnumerator typeOutDialogue(string dialogue)
    {
        dialogueActive = true;
        GameManager.Instance.dialogueText.gameObject.SetActive(true);
        GameManager.Instance.dialogueText.text = ""; //clear dialogue text

        //type out each letter and make the vtuber sound effect play
        foreach(char letter in dialogue)
        {
            GameManager.Instance.dialogueText.text += letter.ToString();
            GameManager.Instance.VTuberSpeakAudioSource.pitch = Random.Range(1.0f, 1.5f);//variable pitch
            GameManager.Instance.VTuberSpeakAudioSource.Play();
            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(dialogueActiveTime);

        GameManager.Instance.dialogueText.text = ""; //clear dialogue text
        GameManager.Instance.dialogueText.gameObject.SetActive(false);
        dialogueActive = false;
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
