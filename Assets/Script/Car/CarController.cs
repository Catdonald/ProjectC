using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float turnSpeed = 15.0f;
    [SerializeField] private int maxOrderCount = 5;

    public bool HasOrder { get; private set; }
    public int OrderCount { get; private set; }

    private OrderInfo orderInfo;
    private List<Vector3> linePositions = new List<Vector3>();
    private Vector3 exitPoint;
    private int queueNumber;
    private int currentLineIndex;

    void Start()
    {
        orderInfo = GameManager.instance.GetOrderInfo(2);
    }

    public void Init(Line line, Vector3 exitPoint, int queueNumber)
    {
        linePositions = line.LinePositions;
        this.exitPoint = exitPoint;
        this.queueNumber = queueNumber;
        currentLineIndex = linePositions.Count - 1;
        MoveToNextLinePosition();
    }

    public void UpdateQueue()
    {
        queueNumber--;
        currentLineIndex = queueNumber;
        MoveToNextLinePosition();
    }

    private void MoveToNextLinePosition()
    {
        Vector3 targetPos = linePositions[currentLineIndex];
        System.Action onComplete = () =>
        {
            if (currentLineIndex > queueNumber)
            {
                currentLineIndex--;
                MoveToNextLinePosition();
                GameManager.instance.SoundManager.PlaySFX("SFX_carHorn");
            }
            else if (queueNumber == 0)
            {
                PlaceOrder();
            }
        };
        Move(targetPos, onComplete);
    }

    private void Move(Vector3 targetPos, System.Action onComplete)
    {
        if (DOTween.IsTweening(transform))
            return;
        float distance = Vector3.Distance(transform.position, targetPos);
        Vector3 direction = targetPos - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        float movingDuration = distance / moveSpeed;
        float turningDuration = distance / turnSpeed;

        var sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(targetPos, movingDuration).SetEase(Ease.Linear));
        sequence.Join(transform.DORotateQuaternion(targetRotation, turningDuration).SetEase(Ease.Linear));
        sequence.OnComplete(() => onComplete.Invoke());
    }

    public void PlaceOrder()
    {
        OrderCount = Random.Range(1, maxOrderCount);
        HasOrder = true;
        orderInfo.ShowInfo(OrderCount);
    }

    public void ReceiveFood(Transform package)
    {
        package.DOJump(transform.position + Vector3.up * 3, 5.0f, 1, 0.5f)
            .OnComplete(() => GameManager.instance.PoolManager.Return(package.gameObject));

        OrderCount--;
        if (OrderCount == 0)
        {
            orderInfo.HideInfo();
        }
        else
        {
            orderInfo.ShowInfo(OrderCount);
        }
    }

    public void Exit()
    {
        Move(exitPoint, () => Destroy(gameObject));
    }
}
