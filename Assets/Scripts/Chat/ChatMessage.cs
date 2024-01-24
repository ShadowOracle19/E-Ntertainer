using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChatMessage", menuName = "ScriptableObjects/ChatMessageList", order = 1)]
public class ChatMessage : ScriptableObject
{
    public List<string> messages = new List<string>();
}
