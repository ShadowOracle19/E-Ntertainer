using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    #region CutsceneManager Manager singleton no need to touch this
    private static CutsceneManager _instance;
    public static CutsceneManager Instance
    {
        get
        {
            if (_instance is null)
            {
                Debug.LogError("Cutscene Manager is NULL");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }
    #endregion

    public CutsceneSequence currentSequence;
    public TextMeshProUGUI textComponent;
    public TextMeshProUGUI speakerName;
    private float textSpeed;
    public int index;

    public void PlayCutscene(CutsceneSequence sequence)
    {
        currentSequence = sequence;
        index = 0;
        textComponent.text = string.Empty;
        speakerName.text = string.Empty;
        StartCoroutine(TypeOutText());
    }


    IEnumerator TypeOutText()
    {
        yield return new WaitForSeconds(0.1f);
        //type each character 1 by 1 
        foreach (char c in currentSequence.lines[index].text.ToCharArray())
        {
            speakerName.text = currentSequence.lines[index].characterName;
            textComponent.text += c;
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }

    

    void NextLine()
    {
        if (index < currentSequence.lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeOutText());
        }
        else//finish dialogue
        {
            GameManager.Instance.EndingCutsceneFinished();
            return;
        }
    }

    public void DialogueClick()
    {
        if (textComponent.text == currentSequence.lines[index].text)
        {
            NextLine();
        }
        else
        {
            StopAllCoroutines();
            textComponent.text = currentSequence.lines[index].text;
        }
    }
}
