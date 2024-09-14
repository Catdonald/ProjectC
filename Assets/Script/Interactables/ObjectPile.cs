using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// n * m ���·� �����ؼ� ������Ʈ�� �׾ƿø���.
/// ���� ������Ʈ�� �÷��̾ ������ ������ �� �ִ�.
/// </summary>
/// 
public class ObjectPile : Interactable
{
    [SerializeField] private StackType stackType;
    [SerializeField] private int length = 2; // ���� ĭ ��
    [SerializeField] private int width = 2; // ���� ĭ ��
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

    // �ӽ�
    public void RemoveAll()
    {
        objects.Clear();
    }
}
