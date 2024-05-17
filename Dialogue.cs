using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{

    [System.Serializable]
    public struct DiaInfo
    {
        public string dias;
        public int talkerId;
        public AudioClip audioClip;
    }
    public int id;
    public Image image;
    public Text text;
    public DiaInfo[] diaInfos;
    public GameObject trigObject;
    //public float destroytimetrigobj;

}
