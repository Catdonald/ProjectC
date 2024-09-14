using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TableManager : MonoBehaviour
{
    private int randomTableIndex = 0;
    [SerializeField]
    private List<Table> tables_burger = new List<Table>();
    public List<Table> Tables_burger => tables_burger;

    void Start()
    {
        Table[] tableObjects = GameObject.FindObjectsOfType<Table>();
        foreach (Table table in tableObjects)
        {
            if (table.StackType == StackType.BURGER)
            {
                tables_burger.Add(table);
            }
            else if (table.StackType == StackType.COFFEE)
            {
                // TODO
            }
        }
        PickRandomTableIndex();
    }

    private void PickRandomTableIndex()
    {
        randomTableIndex = Random.Range(0, tables_burger.Count);
    }

    public GameObject GetAvailableSeat(CustomerController customer, StackType type)
    {
        if (type == StackType.BURGER)
        {
            if (tables_burger.Count == 0)
            {
                return null;
            }

            var semiFullTable = tables_burger.Where(table => table.gameObject.activeInHierarchy && table.IsSemiFull).FirstOrDefault();
            if (semiFullTable != null)
            {
                return semiFullTable.AssignSeat(customer);
            }

            var emptyTables = tables_burger.Where(table => table.gameObject.activeInHierarchy && table.IsEmpty).ToList();
            if (emptyTables.Count > 0)
            {
                int randomIndex = Random.Range(0, emptyTables.Count);               
                return emptyTables[randomIndex].AssignSeat(customer);
            }
        }

        return null;
    }

    public bool HasAvailableSeat(StackType type)
    {
        if (type == StackType.BURGER)
        {
            if (tables_burger.Count == 0)
            {
                return false;
            }

            var semiFullTable = tables_burger.Where(table => table.gameObject.activeInHierarchy && table.IsSemiFull).FirstOrDefault();
            if (semiFullTable != null)
            {
                return true;
            }

            var emptyTables = tables_burger.Where(table => table.gameObject.activeInHierarchy && table.IsEmpty).ToList();
            if (emptyTables.Count > 0)
            {
                return true;
            }
        }

        return false;
    }
}
