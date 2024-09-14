using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// n * m 형태로 정리해서 오브젝트를 쌓아올린다.
/// 쌓인 오브젝트를 플레이어나 직원이 가져갈 수 있다.
/// </summary>
/// 
public class ObjectPile : Interactable
{
    [SerializeField] private StackType stackType;
    [SerializeField] private int length = 2; // 세로 칸 수
    [SerializeField] private int width = 2; // 가로 칸 수
    [SerializeField] private Vector3 spacing = new Vector3(0.5f, 0.1f, 0.5f);
    [SerializeField] private float dropInterval = 0.03f;

    public StackType StackType => stackType;
    public int Count => objects.Count;

    protected Stack<GameObject> objects = new Stack<GameObject>();

    private float dropTimer = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        spacing.y = GameManager.instance.GetStackOffset(stackType);
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
        if (player.playerStack.StackType == StackType.NONE || player.playerStack.StackType == stackType)
        {
            // TODO
            objects.Pop();
        }
    }

    public void AddObject(GameObject obj)
    {
        objects.Push(obj);
        // ArrangeAddedObejct()
    }

    public void RemoveAndStackToReceiver(Receiver receiver)
    {
        var removeObj = objects.Pop();
        // TODO
    }

    // 임시
    public void RemoveAll()
    {
        objects.Clear();
    }
}
