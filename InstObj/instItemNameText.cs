using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class instItemNameText : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        GOManager.itemNameText = GetComponent<Text>();

    }

}
