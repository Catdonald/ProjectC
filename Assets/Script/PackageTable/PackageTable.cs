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
    public Vector3 BurgerStoragePosition => transform.TransformPoint(burgerStoragePosition);
    public Vector3 PackageStoragePosition => transform.TransformPoint(packageStoragePosition);

    public Receiver foodReceiver;
    public Giver packageStack;
    [SerializeField] private Transform packageBox;
    [SerializeField] private Vector3 burgerStoragePosition = Vector3.zero;
    [SerializeField] private Vector3 packageStoragePosition = Vector3.zero;
    [SerializeField] private GameObject maxImg;

    #region PackageTable Stats
    [SerializeField] private float baseInterval = 1.5f;
    [SerializeField] private int baseCapacity = 30;
    #endregion

    private float packingInterval;
    private float packingTimer = 0.0f;
    private int currentPackingCount = 0;

    const int maxPackingCount = 4;

    void Update()
    {
        Packing();

        if (IsFoodStorageFull && !maxImg.activeSelf)
            maxImg.SetActive(true);
        else if (!IsFoodStorageFull && maxImg.activeSelf)
            maxImg.SetActive(false);
    }

    public override void UpgradeStats()
    {
        packingInterval = baseInterval / upgradeLevel;
        foodReceiver.MaxStackCount = baseCapacity + upgradeLevel * 5;
        packageStack.MaxStackCount = baseCapacity + upgradeLevel * 5;
    }

    private void Packing()
    {
        packingTimer += Time.deltaTime;

        if (packingTimer >= packingInterval)
        {
            packingTimer = 0.0f;
            if (HasWorker && foodReceiver.Count > 0 && !packageStack.IsFull)
            {
                // 박스 오브젝트 켜기
                packageBox.gameObject.SetActive(true);
                var food = foodReceiver.RequestObject();
                food.transform.DOJump(packageBox.GetChild(currentPackingCount).position, 5f, 1, 0.3f)
                    .OnComplete(() =>
                    {
                        // package box's child object set active true
                        packageBox.GetChild(currentPackingCount).gameObject.SetActive(true);
                        GameManager.instance.PoolManager.Return(food);
                        currentPackingCount++;

                        // count가 max 이상이면
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
        // 박스 오브젝트 끄기
        foreach (Transform child in packageBox)
        {
            child.gameObject.SetActive(false);
        }
        packageBox.gameObject.SetActive(false);

        // 버거팩 오브젝트 풀에서 꺼내오기
        var burgerPack = GameManager.instance.PoolManager.SpawnObject("Burger_Package");
        burgerPack.transform.position = packageBox.position;
        // 트윈
        //burgerPack.transform.DOJump(packageStorage.PeekPoint, 5f, 1, 0.5f).WaitForCompletion();
        // 버거팩 stack에 추가
        packageStack.ReceiveObject(burgerPack, eObjectType.BURGERPACK, packageStack.objectHeight);
    }

#if UNITY_EDITOR
    protected void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        
        Gizmos.color = Color.red;
        Vector3 center = BurgerStoragePosition;
        Vector3 size = new Vector3(2.0f, 0.2f, 2.0f);
        Gizmos.DrawCube(center, size);

        Gizmos.color = Color.green;
        center = PackageStoragePosition;
        Gizmos.DrawCube(center, size);
    }
#endif
}