using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstPhrasePanel : MonoBehaviour
{
    void Awake()
    {
        GOManager.phrasePanel = gameObject;
    }
}
