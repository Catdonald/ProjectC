using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Table : MonoBehaviour
{
    public Chair[] chairs;
    public GameObject trash;
    public Receiver tableStack;

    public int TrashCount { get; private set; }
    public int CarryingFoodCount { get; set; }

    void Start()
    {
        tableStack = GetComponentInChildren<Receiver>();

        List<Chair> chairList = new List<Chair>();
        for (int i = 0; i < transform.childCount; i++)
        {
            Chair chair = transform.GetChild(i).gameObject.GetComponent<Chair>();
            if (chair != null)
            {
                chairList.Add(chair);
            }

            if (transform.GetChild(i).gameObject.name == "Trashes")
            {
                trash = transform.GetChild(i).gameObject;
            }
        }
        chairs = new Chair[chairList.Count];
        for (int i = 0; i < chairList.Count; i++)
        {
            chairs[i] = chairList[i];
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Employee"))
        {
            if (TrashCount > 0)
            {
                StartCoroutine(StackTrashToStaff(other.gameObject));
            }
        }
    }
    public GameObject GetEmptySeat()
    {
        if (TrashCount <= 0)
        {
            foreach (Chair chair in chairs)
            {
                if (!chair.isChairUsing)
                {
                    chair.isChairUsing = true;
                    return chair.gameObject;
                }
            }
        }
        return null;
    }

    public bool IsTableFull()
    {
        foreach (Chair chair in chairs)
        {
            if (chair.GetSittingCustomer() == null)
            {
                return false;
            }
        }
        return true;
    }

    public void RemoveFoodOnTable()
    {
        GameManager.instance.PoolManager.Return(tableStack.stack.Pop());
        CarryingFoodCount--;
    }

    public void CleanTable()
    {
        // 쓰레기 오브젝트 비활성화
        trash.SetActive(false);
        TrashCount = 0;
    }

    public void MakeDirtyTable()
    {
        trash.SetActive(true);
        int randomCount = Random.Range(1, 5);
        TrashCount += randomCount;
    }

    public IEnumerator StackTrashToStaff(GameObject staff)
    {
        while (TrashCount > 0)
        {
            // 쓰레기 하나를 플레이어에게 옮김
            staff.GetComponentInChildren<playerStack>().ReceiveObject(GameManager.instance.PoolManager.Get(2), eObjectType.TRASH, 0.2f);
            TrashCount--;
            Debug.Log("TrasCount: " + TrashCount);

            yield return new WaitForSeconds(0.2f);

            // 쓰레기 개수가 0이 되면 책상 청소 함수 호출
            if (TrashCount == 0)
            {
                GameManager.instance.TableManager.CleanTable(this);
            }

            yield break;
        }
    }
}
