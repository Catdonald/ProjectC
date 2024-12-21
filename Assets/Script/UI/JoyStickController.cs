using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoyStickController : MonoBehaviour
{
    public Vector3 mouseDelta;

    // Update is called once per frame
    void Update()
    {
        float posX = Mathf.Clamp(mouseDelta.x, -75.0f, 75.0f);
        float posY = Mathf.Clamp(mouseDelta.y, -75.0f, 75.0f);
        transform.localPosition = new Vector3(posX, posY, 0);
    }
}
