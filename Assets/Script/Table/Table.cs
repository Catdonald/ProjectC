using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    public Chair[] chairs;
    public int CarryingFoodCount { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        List<Chair> chairList = new List<Chair>();
        for(int i = 0; i < transform.childCount; i++)
        {
            Chair chair = transform.GetChild(i).gameObject.GetComponent<Chair>();
            if(chair != null)
            {
                chairList.Add(chair);
            }
        }
        chairs = new Chair[chairList.Count];
        for(int i = 0; i < chairList.Count; i++)
        {
            chairs[i] = chairList[i];
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Customer"))
        {
            CustomerController customer = other.gameObject.GetComponent<CustomerController>();
            if (customer.GetCurrentState() == CustomerController.State.EXIT)
            {
                // ����Ȯ���� Table ������ ������Ʈ Ȱ��ȭ
                // ������ ������Ʈ Ȱ��ȭ �Ǿ����� �ʴٸ� Ȱ��ȭ
                // TODO) ������ ġ������ TableManager�� emptyTables�� �� ������Ʈ�� �ٽ� �߰��Ѵ�.

                CleanTable();
            }
        }
    }

    public GameObject GetEmptySeat()
    {
        foreach(Chair chair in chairs)
        {
            if(!chair.isChairUsing)
            {
                chair.isChairUsing = true;
                return chair.gameObject;
            }
        }
        return null;
    }

    public bool IsTableFull()
    {
        foreach(Chair chair in chairs)
        {
            if(chair.GetSittingCustomer() == null)
            {
                return false;
            }
        }
        return true;
    }

    public void CleanTable()
    {
        // ������ ������Ʈ ��Ȱ��ȭ
        GameManager.instance.TableManager.AddTable(this);
    }
}
