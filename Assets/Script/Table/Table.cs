using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    public Chair[] chairs;
    public GameObject trash;
    public int TrashCount { get; private set; }
    public int CarryingFoodCount { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        List<Chair> chairList = new List<Chair>();
        for (int i = 0; i < transform.childCount; i++)
        {
            Chair chair = transform.GetChild(i).gameObject.GetComponent<Chair>();
            if (chair != null)
            {
                chairList.Add(chair);
            }

            if(transform.GetChild(i).gameObject.name == "Trashes")
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
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Employee"))
        {
            if(TrashCount > 0)
            {
                GameManager.instance.TableManager.CleanTable(this);
                // 플레이어나 직원에게 쓰레기 쌓이게 해야 함.
            }
        }
    }
    public GameObject GetEmptySeat()
    {
        foreach (Chair chair in chairs)
        {
            if (!chair.isChairUsing)
            {
                chair.isChairUsing = true;
                return chair.gameObject;
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
        // Todo) 테이블 위 음식 중 가장 위에 있는 음식 제거

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
}
