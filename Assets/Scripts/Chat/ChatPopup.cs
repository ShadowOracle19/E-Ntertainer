using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;

public class ChatPopup : MonoBehaviour
{
    public TextMeshProUGUI message;

    private void Start()
    {
        RectTransform rect = gameObject.transform as RectTransform;

        rect.localScale = Vector3.one;
    }
}
