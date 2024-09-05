using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TableManager : MonoBehaviour
{
    private int randomTableIndex = 0;
    //private List<Table> tables;
    private List<Table> emptyTables;

    // Start is called before the first frame update
    void Start()
    {
        Table[] tableObjects = GetComponentsInChildren<Table>();
        emptyTables = new List<Table>();
        foreach (Table table in tableObjects)
        {
            emptyTables.Add(table);
        }
        PickRandomTableIndex();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PickRandomTableIndex()
    {
        randomTableIndex = Random.Range(0, emptyTables.Count);
    }

    public GameObject GetEmptySeat()
    {
        if(emptyTables.Count == 0)
        {
            return null;
        }

        GameObject seat = emptyTables[randomTableIndex].GetEmptySeat();
        if (seat == null)
        {
            emptyTables.Remove(emptyTables[randomTableIndex]);
            if(emptyTables.Count == 0)
            { 
                return null;
            }
            PickRandomTableIndex();
            return GetEmptySeat();
        }
        return seat;
    }

    public void AddTable(Table table)
    {
        emptyTables.Add(table);
        emptyTables.Distinct().ToList();
    }
}
