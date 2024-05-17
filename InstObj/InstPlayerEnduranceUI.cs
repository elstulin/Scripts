using UnityEngine;

public class InstPlayerEnduranceUI : MonoBehaviour
{
    void Awake()
    {
        GOManager.playerEnduranceUIGo = gameObject;
        GOManager.playerEnduranceUIRectTransform = transform.GetChild(0).GetComponent<RectTransform>();
        GOManager.playerEnduranceUIGo.SetActive(false);
        Destroy(this);
    }

}
