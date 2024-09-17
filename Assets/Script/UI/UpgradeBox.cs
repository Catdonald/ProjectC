using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeBox : Interactable
{
    [Header("Button info")]
    public float allPrice;
    public float curPrice;
    public int width;
    public int height;
    public bool isActive = false;
    public bool isPushed = true;

    [Header("Image obj")]
    [SerializeField] private Image background;
    [SerializeField] private Image fill;

    // Start is called before the first frame update
    void Start()
    {
        allPrice = 100;
        curPrice = 0;
        fill.fillAmount = 0;
        isActive = false;
        isPushed = false;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (isPushed && GameManager.instance.GetMoney() > 0)
        {
            curPrice = GameManager.instance.GetMoney() / allPrice;
            StartCoroutine(Filling());
        }

        if (fill.fillAmount >= 1.0f)
        {
            gameObject.SetActive(false);
        }
    }
    IEnumerator Filling()
    {
        while (isPushed)
        {
            yield return new WaitForSeconds(0.1f);

            fill.fillAmount = Mathf.Lerp(fill.fillAmount, curPrice, 0.0001f);
        }
    }
}
