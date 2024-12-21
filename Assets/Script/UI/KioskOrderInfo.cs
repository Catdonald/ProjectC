using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KioskOrderInfo : MonoBehaviour
{
    [SerializeField] private Vector3 firstIconOffset;
    [SerializeField] private Vector3 secondIconOffset;
    [SerializeField] private Vector3 firstIconCenterOffset;
    [SerializeField] private Vector3 secondIconCenterOffset;
    [SerializeField] private Vector3 firstTextOffset;
    [SerializeField] private Vector3 secondTextOffset;
    [SerializeField] private Vector3 textCenterOffset;

    private GameObject firstUI;
    private GameObject secondUI;
    private Image firstIcon;
    private Image secondIcon;
    private Text firstText;
    private Text secondText;

    // Start is called before the first frame update
    void Start()
    {
        firstUI = transform.GetChild(2).gameObject;
        firstIcon = firstUI.GetComponentInChildren<Image>();
        firstText = firstUI.GetComponentInChildren<Text>();
        secondUI = transform.GetChild(3).gameObject;
        secondIcon = secondUI.GetComponentInChildren<Image>();
        secondText = secondUI.GetComponentInChildren<Text>();
        HideInfo();
    }

    public void ShowInfo(int firstOrderCount, int secondOrderCount)
    {
        gameObject.SetActive(true);
        // 둘 다 주문개수 1개 이상
        if (firstOrderCount >= 1 && secondOrderCount >= 1)
        {
            firstUI.SetActive(true);
            secondUI.SetActive(true);
            firstIcon.rectTransform.position = transform.TransformPoint(firstIconOffset);
            firstText.rectTransform.position = transform.TransformPoint(firstTextOffset);
            secondIcon.rectTransform.position = transform.TransformPoint(secondIconOffset);
            secondText.rectTransform.position = transform.TransformPoint(secondTextOffset);
            firstText.text = firstOrderCount.ToString();
            secondText.text = secondOrderCount.ToString();
        }
        else if (firstOrderCount >= 1)
        {
            // 첫번째 음식만 주문
            firstUI.SetActive(true);
            secondUI.SetActive(false);
            firstIcon.rectTransform.position = transform.TransformPoint(firstIconCenterOffset);
            firstText.rectTransform.position = transform.TransformPoint(textCenterOffset);           
            firstText.text = firstOrderCount.ToString();  
        }
        else if (secondOrderCount >= 1)
        {
            // 두번째 음식만 주문
            firstUI.SetActive(false);
            secondUI.SetActive(true);
            secondIcon.rectTransform.position = transform.TransformPoint(secondIconCenterOffset);
            secondText.rectTransform.position = transform.TransformPoint(textCenterOffset);
            secondText.text = secondOrderCount.ToString();
        }
        else
        {
            firstUI.SetActive(false);
            secondUI.SetActive(false);
        }
    }

    public void HideInfo()
    {
        gameObject.SetActive(false);
    }
}
