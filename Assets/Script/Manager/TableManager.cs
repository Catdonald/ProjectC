using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TableManager : MonoBehaviour
{
    public static TableManager Instance {  get; private set; }
    [SerializeField]
    private List<Table> tables_burger = new List<Table>();
    public List<Table> Tables_burger => tables_burger;
    public List<Table> DirtyTables => tables_burger.Where(x => x.TrashCount > 0).ToList();

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Table[] tableObjects = GameObject.FindObjectsOfType<Table>(true);
        foreach (Table table in tableObjects)
        {
            if (table.StackType == eObjectType.HAMBURGER)
            {
                tables_burger.Add(table);
            }
            else if (table.StackType == eObjectType.COFFEE)
            {
                // TODO
            }
        }
    }

    public GameObject GetAvailableSeat(CustomerController customer, eObjectType type)
    {
        if (type == eObjectType.HAMBURGER)
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

    public bool HasAvailableSeat(eObjectType type)
    {
        if (type == eObjectType.HAMBURGER)
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
