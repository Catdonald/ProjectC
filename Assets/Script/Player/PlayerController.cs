using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public GameObject playerRoot;
    [SerializeField] private GameObject moveController;
    [SerializeField] private ParticleSystem particle;

    [SerializeField] private float baseSpeed = 10.0f;
    [SerializeField] private int baseCapacity = 5;
    [SerializeField] private GameObject maxImg;

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
    public Animator Animator => animator;
    public bool IsUnlockEntranceTriggerOn { get; set; } = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerRigidbody.sleepThreshold = 0.0f;
        joystickController = moveController.GetComponentInChildren<JoyStickController>();
        playerStack = GetComponentInChildren<playerStack>();
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.OnUpgrade += UpdateStats;
        UpdateStats();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerStack.IsFull 
            && playerStack.StackType != eObjectType.TRASH 
            && playerStack.StackType != eObjectType.EMPTYCUP 
            && !maxImg.activeSelf)
        {
            maxImg.SetActive(true);
        }
        else if (!playerStack.IsFull && maxImg.activeSelf)
        {
            maxImg.SetActive(false);
        }


        mouseDelta = Vector2.zero;
        if (GameManager.instance.IsUpgradableCamMoving == false && IsUnlockEntranceTriggerOn == false)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (!EventSystem.current.IsPointerOverGameObject())
                    {
                        isClicked = true;
                        mouseClickedPos = Input.mousePosition;
                        moveController.GetComponent<RectTransform>().position = mouseClickedPos;
                        moveController.SetActive(true);
                        Camera.main.transform.localPosition = new Vector3(-10.7f, 17.0f, -10.7f);
                        Camera.main.transform.localRotation = Quaternion.Euler(45, 45, 0);
                    }
                }
            }
            if (Input.GetMouseButton(0))
            {
                if (isClicked)
                {
                    Vector3 mousePos = Input.mousePosition;
                    mouseDelta = mousePos - mouseClickedPos;
                    
                    Vector3 mouseDeltaNorm = new Vector3(mouseDelta.x / 100.0f, mouseDelta.y / 100.0f, mouseDelta.z);
                    float mouseDeltaMagnitude = mouseDeltaNorm.magnitude;
                    if (mouseDeltaMagnitude > 1.0f)
                    {
                        mouseDeltaNorm = mouseDelta.normalized;
                    }
                    animator.SetBool("isMove", mouseDeltaMagnitude > 0.0f);

                    Vector3 moveVec = new Vector3(mouseDeltaNorm.x, 0.0f, mouseDeltaNorm.y);
                    Vector3 moveDir = Quaternion.Euler(0, 45, 0) * moveVec;
                    rayStartPoint = new Vector3(transform.position.x, 0.15f, transform.position.z);
#if UNITY_EDITOR
                    Debug.DrawRay(rayStartPoint, moveDir * 1.0f, Color.red);
#endif
                    RaycastHit hit;
                    if (Physics.Raycast(rayStartPoint, moveDir, out hit, 1.0f))
                    {
                        if (!hit.collider.CompareTag("Building"))
                        {
                            playerRigidbody.MovePosition(transform.position + moveDir * Time.deltaTime * moveSpeed);
                        }
                    }
                    else
                    {
                        playerRigidbody.MovePosition(transform.position + moveDir * Time.deltaTime * moveSpeed);
                    }

                    if (moveDir.magnitude > 0.0f)
                    {
                        playerRoot.transform.forward = moveDir;
                    }

                    joystickController.mouseDelta = mouseDelta;
                }
            }

            particle.transform.position = gameObject.transform.position + new Vector3(0, 0.3f, 0);
        }
        if (Input.GetMouseButtonUp(0) || GameManager.instance.IsUpgradableCamMoving)
        {
            isClicked = false;
            animator.SetBool("isMove", false);
            moveController.SetActive(false);
        }
        maxImg.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 5f, 0));
    }

    private void UpdateStats()
    {
        int speedLevel = GameManager.instance.GetUpgradeLevel(UpgradeType.PlayerSpeed);
        moveSpeed = baseSpeed + (speedLevel * 0.2f);
        int capacityLevel = GameManager.instance.GetUpgradeLevel(UpgradeType.PlayerCapacity);
        Stack.Capacity = baseCapacity + (capacityLevel * 3);
    }
}
