using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PackageTable : WorkStation
{
    public int GetStoredFoodCount => foodReceiver.stack.Count;
    public bool IsFoodStorageFull => foodReceiver.IsFull;
    public int GetStoredPackageCount => packageStack.stack.Count;
    public bool IsPackageStorageFull => packageStack.IsFull;

    public Receiver foodReceiver;
    public Giver packageStack;
    [SerializeField] private Transform packageBox;

    #region PackageTable Stats
    [SerializeField] private float baseInterval = 1.5f;
    [SerializeField] private int baseCapacity = 30;
    #endregion

    private float packingInterval;
    private float packingTimer = 0.0f;
    private int packingCapacity;
    private int currentPackingCount = 0;

    const int maxPackingCount = 4;

    void Update()
    {
        Packing();
    }

    public override void UpgradeStats()
    {
        packingInterval = baseInterval / upgradeLevel;
        packingCapacity = baseCapacity + upgradeLevel * 5;
    }

    private void Packing()
    {
        packingTimer += Time.deltaTime;

        if (packingTimer >= packingInterval)
        {
            packingTimer = 0.0f;
            if (HasWorker && foodReceiver.Count > 0 && !packageStack.IsFull)
            {
                // �ڽ� ������Ʈ �ѱ�
                packageBox.gameObject.SetActive(true);
                var food = foodReceiver.RequestObject();
                food.transform.DOJump(packageBox.GetChild(currentPackingCount).position, 5f, 1, 0.3f)
                    .OnComplete(() =>
                    {
                        // package box's child object set active true
                        packageBox.GetChild(currentPackingCount).gameObject.SetActive(true);
                        GameManager.instance.PoolManager.Return(food);
                        currentPackingCount++;

                        // count�� max �̻��̸�
                        if (currentPackingCount >= maxPackingCount)
                        {
                            currentPackingCount = 0;
                            StartCoroutine(FinishPacking());
                        }
                    });

            }
        }
    }

    private IEnumerator FinishPacking()
    {
        yield return new WaitForSeconds(0.1f);
        // �ڽ� ������Ʈ ����
        foreach (Transform child in packageBox)
        {
            child.gameObject.SetActive(false);
        }
        packageBox.gameObject.SetActive(false);

        // ������ ������Ʈ Ǯ���� ��������
        var burgerPack = GameManager.instance.PoolManager.SpawnObject("Burger_Package");
        burgerPack.transform.position = packageBox.position;
        // Ʈ��
        //burgerPack.transform.DOJump(packageStorage.PeekPoint, 5f, 1, 0.5f).WaitForCompletion();
        // ������ stack�� �߰�
        packageStack.ReceiveObject(burgerPack, eObjectType.BURGERPACK, packageStack.objectHeight);
    }
}