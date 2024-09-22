using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TableManager : MonoBehaviour
{
    public static TableManager Instance { get; private set; }
    [SerializeField]
    private List<Table> tables = new List<Table>();
    public List<Table> Tables => tables;
    public List<Table> DirtyTables => tables.Where(x => x.TrashCount > 0).ToList();

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        tables = GameObject.FindObjectsOfType<Table>(true).ToList();
    }

    public GameObject GetAvailableSeat(CustomerController customer, eObjectType type)
    {
        if (tables.Count > 0)
        {
            var semiFullTable = tables.Where(table => table.gameObject.activeInHierarchy && table.IsSemiFull && table.StackType == type).FirstOrDefault();
            if (semiFullTable != null)
            {
                return semiFullTable.AssignSeat(customer);
            }

            var emptyTables = tables.Where(table => table.gameObject.activeInHierarchy && table.IsEmpty && table.StackType == type).ToList();
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
        if (tables.Count > 0)
        {
            var semiFullTable = tables.Where(table => table.gameObject.activeInHierarchy && table.IsSemiFull && table.StackType == type).FirstOrDefault();
            if (semiFullTable != null)
            {
                return true;
            }

            var emptyTables = tables.Where(table => table.gameObject.activeInHierarchy && table.IsEmpty && table.StackType == type).ToList();
            if (emptyTables.Count > 0)
            {
                return true;
            }
        }

        return false;
    }
}
