using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStackObject
{

    float actingTime { get; set; }
    bool isActing { get; set; }
    Stack<GameObject> stack { get; set; }
    float objectHeight { get; set; }

    public void Enter(Collision other);
    public void Exit();
    public void Interaction(Collision other);
    public void ReceiveObject(GameObject obj, eObjectType objType, float objHegiht);
    public GameObject RequestObject();
    IEnumerator SpawnObject(int type);
    IEnumerator DestryObject();
    IEnumerator UpdateObjectPos(GameObject obj, Vector3 targetPos, GameObject targetObject);
    IEnumerator UpdateObjectPos(GameObject obj, Vector3 targetPos);
}
