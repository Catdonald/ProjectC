using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyStickController : MonoBehaviour
{
    public Vector3 mouseDelta;
    private RectTransform rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        float posX = Mathf.Clamp(mouseDelta.x, -70.0f, 70.0f);
        float posY = Mathf.Clamp(mouseDelta.y, -70.0f, 70.0f);
        rectTransform.localPosition = new Vector3(posX, posY, 0);
    }
}
