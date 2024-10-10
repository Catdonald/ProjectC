using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject playerRoot;
    [SerializeField] private GameObject moveController;

    [SerializeField] private float baseSpeed = 50.0f;
    [SerializeField] private int baseCapacity = 5;

    private playerStack playerStack;
    private Animator animator;
    private Rigidbody playerRigidbody;
    private JoyStickController joystickController;  

    private Vector3 rayStartPoint;
    private Vector3 mouseClickedPos;
    private bool isClicked = false;
    private float moveSpeed;

    public Vector3 mouseDelta;
    public playerStack Stack => playerStack;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerRigidbody.sleepThreshold = 0.0f;
        joystickController = moveController.GetComponentInChildren<JoyStickController>();
        playerStack = GetComponentInChildren<playerStack>();
        GameManager.instance.OnUpgrade += UpdateStats;
        UpdateStats();
    }

    // Update is called once per frame
    void Update()
    {
        mouseDelta = Vector2.zero;
        if (GameManager.instance.IsUpgradableCamMoving == false)
        {
            if (Input.GetMouseButtonDown(0))
            {
                // UI�� Ŭ������ �ʴ´�.
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (EventSystem.current.IsPointerOverGameObject() == false)
                    {
                        isClicked = true;
                        mouseClickedPos = Input.mousePosition;
                        // moveController UI Ȱ��ȭ
                        moveController.SetActive(true);
                        Vector3 touchControllerPos = new Vector3(mouseClickedPos.x - Screen.width / 2, mouseClickedPos.y - Screen.height / 2, mouseClickedPos.z);
                        moveController.GetComponent<RectTransform>().localPosition = touchControllerPos;
                    }
                }
            }
            if (Input.GetMouseButton(0))
            {
                if (isClicked)
                {
                    // Ŭ���� ��ġ�� ���� Ŀ�� ��ġ�� ���� �̵� ���� ���ϱ�
                    Vector3 mousePos = Input.mousePosition;
                    mouseDelta = mousePos - mouseClickedPos;
                    // max padSizeX = 35.0f
                    Vector3 mouseDeltaNorm = new Vector3(mouseDelta.x / 35.0f, mouseDelta.y / 35.0f, mouseDelta.z);
                    float mouseDeltaMagnitude = mouseDeltaNorm.magnitude;
                    if (mouseDeltaMagnitude > 1.0f)
                    {
                        mouseDeltaNorm = mouseDelta.normalized;
                    }
                    animator.SetBool("isMove", mouseDeltaMagnitude > 0.0f);

                    Vector3 moveVec = new Vector3(mouseDeltaNorm.x, 0.0f, mouseDeltaNorm.y);
                    rayStartPoint = new Vector3(transform.position.x, 0.5f, transform.position.z);
                    Debug.DrawRay(rayStartPoint, moveVec * 1.0f, Color.red);
                    RaycastHit hit;
                    if (Physics.Raycast(rayStartPoint, moveVec, out hit, 1.0f))
                    {
                        // �����̳� �մ� ������Ʈ�� �浹���� �ʾҴٸ�
                        // �ǹ��̳� ���, ī���Ϳ� ���� ������Ʈ�� ���̰� �浹�� ���̴�.
                        if (!hit.collider.CompareTag("Building"))
                        {
                            playerRigidbody.MovePosition(transform.position + moveVec * Time.deltaTime * moveSpeed);
                        }
                    }
                    else
                    {
                        playerRigidbody.MovePosition(transform.position + moveVec * Time.deltaTime * moveSpeed);
                    }
                    // �̵��ϴ� ���� �ٶ󺸱�
                    if (moveVec.magnitude > 0.0f)
                    {
                        playerRoot.transform.forward = moveVec;
                    }

                    // ���콺 ��ġ ��ȭ�� ���� JoyStickController�� �Ѱ��ֱ�
                    joystickController.mouseDelta = mouseDelta;
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            isClicked = false;
            animator.SetBool("isMove", false);
            moveController.SetActive(false);
        }
    }

    private void UpdateStats()
    {
        int speedLevel = GameManager.instance.GetUpgradeLevel(UpgradeType.PlayerSpeed);
        moveSpeed = baseSpeed + (speedLevel * 0.2f);
        int capacityLevel = GameManager.instance.GetUpgradeLevel(UpgradeType.PlayerCapacity);
        Stack.Capacity = baseCapacity + (capacityLevel * 3);
    }
}
