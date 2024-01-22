using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChatUsers", menuName = "ScriptableObjects/ChatUsernameList", order = 1)]
public class ChatUsername : ScriptableObject
{
    public List<string> usersFirst = new List<string>();
    
    public List<string> usersSecond = new List<string>();
}
