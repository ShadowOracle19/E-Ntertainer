using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VTuberDialogue", menuName = "ScriptableObjects/VTuberDialogueList", order = 1)]
public class VtuberDialogues : ScriptableObject
{
    public List<string> general = new List<string>();
    public List<string> lowMood = new List<string>();
    public List<string> highMood = new List<string>();
    public List<string> lowAudience = new List<string>();
    public List<string> highAudience = new List<string>();
    public List<string> lowApproval = new List<string>();
    public List<string> highApproval = new List<string>();
    public List<string> death = new List<string>();
    public List<string> collectible = new List<string>();
    public List<string> checkpoint = new List<string>();
    public List<string> tutorial = new List<string>();
}
