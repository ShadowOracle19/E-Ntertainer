using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ChatUsers", menuName = "ScriptableCutscenes/CreateInGameCutscene", order = 1)]
public class CutsceneSequence : ScriptableObject
{
    public Animation animation;
    

    public Line[] lines;

    public string[] vTuberLines;
}

[System.Serializable]
public struct Line
{
    public string characterName;

    [TextArea(2, 5)]
    public string text;
}
