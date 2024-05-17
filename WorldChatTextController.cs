
using UnityEngine;
using UnityEngine.UI;

public class WorldChatTextController : MonoBehaviour
{
    public Transform cameraTransform;
    public Transform targetTransform;
    RectTransform rectTransform;
    float lifeTimer;
    public Text text;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        Destroy(gameObject, 5);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = targetTransform.position + new Vector3(0, 2.2f, 0); ;
        if (!cameraTransform)
        {
            cameraTransform = Camera.main.transform;
        }
        if (GOManager.playerTransform)
        {
            float dot = Vector3.Dot(position - cameraTransform.position, cameraTransform.forward);
            if (dot < 0)
                text.enabled = false;
            else
                text.enabled = true;
        }
        rectTransform.anchoredPosition = Camera.main.WorldToScreenPoint(position);
        float rgbReduce = lifeTimer / 1.667f;
        text.color = new Color(1, 1, 1, 3f - rgbReduce);
        lifeTimer += Time.deltaTime;



    }
}
