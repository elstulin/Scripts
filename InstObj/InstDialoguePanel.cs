using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstDialoguePanel : MonoBehaviour
{
    void Awake()
    {
        GOManager.dialoguePanel = gameObject;
        gameObject.SetActive(false);
    }

}
