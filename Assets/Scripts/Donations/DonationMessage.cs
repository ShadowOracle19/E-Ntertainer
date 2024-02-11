using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DonationMessage", menuName = "ScriptableObjects/DonationMessageList", order = 2)]
public class DonationMessage : ScriptableObject
{
    public List<string> messages = new List<string>();
}
