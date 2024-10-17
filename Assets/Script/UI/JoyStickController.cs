using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoyStickController : MonoBehaviour
{
    public Vector3 mouseDelta;
    public Vector3 mouseClickedPos;
    private Image moveControllerImage;
    private float radius;

    // Start is called before the first frame update
    void Start()
    {
        moveControllerImage = GetComponentInParent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        float posX = Mathf.Clamp(mouseDelta.x, -75.0f, 75.0f);
        float posY = Mathf.Clamp(mouseDelta.y, -75.0f, 75.0f);
        transform.localPosition = new Vector3(posX, posY, 0);
        
        /*Vector3 mousePosition = Input.mousePosition;
        float posX = Mathf.Clamp(mousePosition.x, mouseClickedPos.x - moveControllerImage.rectTransform.sizeDelta.x / 2.0f,
            mouseClickedPos.x + moveControllerImage.rectTransform.sizeDelta.x / 2.0f);
        radius = moveControllerImage.rectTransform.sizeDelta.x / 2.0f;
        float value = Mathf.Sqrt(radius * radius - (posX * posX) + 2.0f * mouseClickedPos.x * posX - mouseClickedPos.x * mouseClickedPos.x);
        float maxY = value + mouseClickedPos.y;
        float minY = -value + mouseClickedPos.y;
        float posY = Mathf.Clamp(mousePosition.y, minY, maxY);
        transform.position = new Vector3(posX, posY, 0);*/
    }
}
