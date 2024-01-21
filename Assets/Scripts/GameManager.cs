using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Game Manager singleton no need to touch this
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if(_instance is null )
            {
                Debug.LogError("Game Manager is NULL");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }
    #endregion

    
    [Header("VTuber Attributes")]
    [Range(-50, 50)]//starting approval should be zero
    public int audienceApproval;//-50 to -30 low, -29 to 29 average, 30 to 50 high
    public int audience;//should not go lower than zero
    public int VTuberMood;//-50 to -30 low, -29 to 29 average, 30 to 50 high
    public float timer;
    //These sprites will eventually change
    public Sprite VTuberDefault;
    public Sprite VTuberPositive;
    public Sprite VTuberNegative;

    //only variables relating to the platformer should go here
    [Header("Platformer")]
    public Transform spawnPoint;
    public GameObject player;
    [Range(0.1f, 1)]
    public float CameraMoveSpeed = 0.5f;

    [Header("UI")]
    public Image VTuberImage;
    public Transform chatpopupParent;
    public GameObject chatPopupPrefab;
    public int position = 50;
    private List<GameObject> chatPopups = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        //this keeps the player mood variable clamped to -1 and 1 so it will never go past that point
        Mathf.Clamp(audience, 0, Mathf.Infinity);
        Mathf.Clamp(audienceApproval, -50, 50);

        //run this statement
        VTuberEmotionSwitch(audienceApproval);

        //rebuild vertical layout to avoid spawning messages incorrectly
        LayoutRebuilder.ForceRebuildLayoutImmediate(chatpopupParent.GetComponent<RectTransform>());
    }

    public void VTuberEmotionSwitch(int approval)
    {
        if(approval >= 30)//High approval
        {
            VTuberImage.sprite = VTuberPositive;
        }
        else if(approval >= -29 && approval <= 29)//Average approval
        {
            VTuberImage.sprite = VTuberDefault;
        }
        else if(approval <= -30 && approval >= -50)//low approval
        {
            VTuberImage.sprite = VTuberNegative;
        }
        
    }

    public void SpawnChatPopup(string message)
    {
        //create the popup 
        GameObject popup = Instantiate(chatPopupPrefab, chatpopupParent);
        popup.GetComponent<ChatPopup>().message.text = message;
        popup.GetComponent<RectTransform>().SetAsFirstSibling();
        
        chatPopups.Add(popup);
        //foreach (var _popup in chatPopups)
        //{
        //    _popup.GetComponent<RectTransform>().anchoredPosition += Vector2.up * position;
        //}
    }

    public void Death()
    {
        audienceApproval -= 5;
        player.transform.position = spawnPoint.position;
    }
}
