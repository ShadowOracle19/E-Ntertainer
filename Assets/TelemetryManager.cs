using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TelemetryManager : MonoBehaviour
{
    public TextMeshProUGUI playtestID;

    public void OnConnectionSuccess(int sessionID)
    {
        if (sessionID < 0)
        {
            playtestID.text = $"Local test Session ID: {sessionID}";
        }
        else
        {
            playtestID.text = $"Playtest Session ID: {sessionID}";
        }
    }

    public void OnConnectionFail(string errorMessage)
    {
        playtestID.text = $"Error: {errorMessage}";
    }
}
