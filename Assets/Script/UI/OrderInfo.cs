using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderInfo : MonoBehaviour
{
    private GameObject orderUI;
    private Text orderCountText;
    private GameObject noSeatUI;

    // Start is called before the first frame update
    void Start()
    {
        orderUI = transform.GetChild(0).gameObject;
        noSeatUI = transform.GetChild(1).gameObject;
        orderCountText = orderUI.transform.GetChild(1).GetComponent<Text>();

        HideInfo();
    }

    public void ShowInfo(int orderCount)
    {
        if(orderCount > 0)
        {
            orderUI.SetActive(true);
            noSeatUI.SetActive(false);
            orderCountText.text = orderCount.ToString();
        }
        else
        {
            orderUI.SetActive(false);
            noSeatUI.SetActive(true);
        }
    }

    public void HideInfo()
    {
        orderUI.SetActive(false);
        noSeatUI.SetActive(false);
    }
}
