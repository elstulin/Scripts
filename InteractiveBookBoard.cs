using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveBookBoard : MonoBehaviour
{
    public string _name;
    public bool expBook;
    private void Awake()
    {
        GOManager.books.Add(this);
    }
    private void OnDestroy()
    {
        GOManager.books.Remove(this);
    }
}
