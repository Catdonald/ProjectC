using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPile : Interactable
{
    [SerializeField] private eObjectType stackType;
    [SerializeField] private int length = 2;
    [SerializeField] private int width = 2;
    [SerializeField] private Vector3 spacing = new Vector3(0.5f, 0.1f, 0.5f);
    [SerializeField] private float dropInterval = 0.03f;

    public eObjectType StackType => stackType;
    public int Count => objects.Count;
    public Vector3 PeakPoint => transform.position + new Vector3(0, spacing.y * Count, 0);

    protected Stack<GameObject> objects = new Stack<GameObject>();

    private Vector3 pileCenter;
    private float dropTimer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        pileCenter = new Vector3((length - 1) * spacing.x * 0.5f, 0, (width - 1) * spacing.z * 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null || objects.Count == 0)
        {
            return;
        }

        dropTimer += Time.deltaTime;
        if (dropTimer >= dropInterval)
        {
            dropTimer = 0.0f;
            Drop();
        }
    }

    protected virtual void Drop()
    {
        if (player.Stack.StackType == eObjectType.LAST || player.Stack.StackType == stackType)
        {
            var removedObj = objects.Pop();
            player.Stack.AddToStack(removedObj, stackType);
            // TODO) Sound
        }
    }

    public void AddObject(GameObject obj)
    {
        objects.Push(obj);
        ArrangeAddedObject();
    }

    private void ArrangeAddedObject()
    {
        int lastIndex = objects.Count - 1;

        int row = (lastIndex / length) % width;
        int col = lastIndex % length;

        float xPos = col * spacing.x - pileCenter.x;
        float yPos = Mathf.FloorToInt(lastIndex / (length * width)) * spacing.y;
        float zPos = row * spacing.z - pileCenter.z;

        var latestObject = objects.Peek();
        latestObject.transform.position = transform.position + new Vector3(xPos, yPos, zPos);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        pileCenter = new Vector3((length - 1) * spacing.x * 0.5f, 0, (width - 1) * spacing.z * 0.5f);
        Gizmos.color = Color.yellow;
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < width; j++)
            {
                Vector3 position = transform.position + new Vector3(i * spacing.x - pileCenter.x, spacing.y / 2.0f, j * spacing.z - pileCenter.z);
                Gizmos.DrawWireCube(position, spacing);
            }
        }
    }
#endif
}
