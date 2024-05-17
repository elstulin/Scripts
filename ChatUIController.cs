
using System.Xml;
using UnityEngine;
using System.IO;
using UnityEngine.UIElements;
using Mirror;

public class ChatUIController : MonoBehaviour
{
    public class ChatText
    {
        public string text;
        public byte channel;
        public ChatText(string text, byte channel)
        {
            this.text = text;
            this.channel = channel;
        }

    }
    public bool[] channelsActive = new bool[] {true,true,true,true,true,true,true,true };
    public GameObject[] buttonsChannelsSelect;
    public GameObject[] buttonsChannelsUnselect;
    public RectTransform rectTransform;
    bool moving;
    bool scale;

    Vector2 startPos;
    Vector2 startScale;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(InterfaceSettings.chatPosX, InterfaceSettings.chatPosY);
        rectTransform.sizeDelta = new Vector2(InterfaceSettings.chatSizeX, InterfaceSettings.chatSizeY);
        bool[] channelsActive = InterfaceSettings.channelsActive;
        for (int i = 0; i < channelsActive.Length; i++)
        {
            if (channelsActive[i])
            {
                if (buttonsChannelsSelect[i])
                    buttonsChannelsSelect[i].SetActive(true);
                if (buttonsChannelsUnselect[i])
                    buttonsChannelsUnselect[i].SetActive(false);
            }
            else
            {
                if (buttonsChannelsSelect[i])
                    buttonsChannelsSelect[i].SetActive(false);
                if (buttonsChannelsUnselect[i])
                    buttonsChannelsUnselect[i].SetActive(true);
            }
        }
    }

    void Update()
    {


        if (moving)
        {
            rectTransform.anchoredPosition = (Vector2)Input.mousePosition - startPos;
            rectTransform.anchoredPosition = new Vector2(Mathf.Clamp(rectTransform.anchoredPosition.x, 2, Screen.width - 2 - rectTransform.sizeDelta.x), Mathf.Clamp(rectTransform.anchoredPosition.y, 2, Screen.height - 24 - rectTransform.sizeDelta.y));
        }
        if (scale)
        {
            rectTransform.sizeDelta = (Vector2)Input.mousePosition - startScale;
            rectTransform.sizeDelta = new Vector2(Mathf.Clamp(rectTransform.sizeDelta.x, 332, 1000), Mathf.Clamp(rectTransform.sizeDelta.y, 206, 600));
        }

    }
    public void ClickMoveDown()
    {
        moving = true;
        startPos = (Vector2)Input.mousePosition - rectTransform.anchoredPosition;
    }
    public void ClickMoveUp()
    {
        moving = false;
        InterfaceSettings.SaveChatSettings((int)rectTransform.anchoredPosition.x, (int)rectTransform.anchoredPosition.y, (int)rectTransform.sizeDelta.x, (int)rectTransform.sizeDelta.y,channelsActive,ChatOmegaLul.blockList.ToArray());

    }
    public void ClickScaleDown()
    {
        scale = true;
        startScale = (Vector2)Input.mousePosition - rectTransform.sizeDelta;
    }
    public void ClickScaleUp()
    {
        scale = false;
        InterfaceSettings.SaveChatSettings((int)rectTransform.anchoredPosition.x, (int)rectTransform.anchoredPosition.y, (int)rectTransform.sizeDelta.x, (int)rectTransform.sizeDelta.y, channelsActive, ChatOmegaLul.blockList.ToArray());
    }

}
