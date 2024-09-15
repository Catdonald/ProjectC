using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject playerRoot;
    [SerializeField]
    GameObject moveController;

    public playerStack playerStack;

    private Animator animator;
    private PlayerData playerData;
    private Rigidbody playerRigidbody;
    private JoyStickController joystickController;  

    private Vector3 rayStartPoint;
    private Vector3 mouseClickedPos;
    private bool isClicked = false;

    private int burgerStackCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        playerData = GetComponent<PlayerData>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerRigidbody.sleepThreshold = 0.0f;
        joystickController = moveController.GetComponentInChildren<JoyStickController>();
        playerStack = GetComponentInChildren<playerStack>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (!isClicked)
            {
                isClicked = true;
                mouseClickedPos = Input.mousePosition;
                // moveController UI Ȱ��ȭ
                moveController.SetActive(true);
                Vector3 touchControllerPos = new Vector3(mouseClickedPos.x - Screen.width / 2, mouseClickedPos.y - Screen.height / 2, mouseClickedPos.z);
                moveController.GetComponent<RectTransform>().localPosition = touchControllerPos;
            }
            else
            {
                // Ŭ���� ��ġ�� ���� Ŀ�� ��ġ�� ���� �̵� ���� ���ϱ�
                Vector3 mousePos = Input.mousePosition;
                Vector3 mouseDelta = mousePos - mouseClickedPos;
                // max padSizeX = 35.0f
                Vector3 mouseDeltaNorm = new Vector3(mouseDelta.x / 35.0f, mouseDelta.y / 35.0f, mouseDelta.z);
                float mouseDeltaMagnitude = mouseDeltaNorm.magnitude;
                if (mouseDeltaMagnitude > 1.0f)
                {
                    mouseDeltaNorm = mouseDelta.normalized;
                }
                if(mouseDeltaMagnitude > 0.0f)
                {
                    animator.SetBool("isMove", true);
                }
                else
                {
                    animator.SetBool("isMove", false);
                }
                Vector3 moveVec = new Vector3(mouseDeltaNorm.x, 0.0f, mouseDeltaNorm.y);
                rayStartPoint = new Vector3(transform.position.x, 0.2f, transform.position.z);
                Debug.DrawRay(rayStartPoint, moveVec * 1.0f, Color.red);
                RaycastHit hit;
                if (Physics.Raycast(rayStartPoint, moveVec, out hit, 1.0f))
                {
                    // �����̳� �մ� ������Ʈ�� �浹���� �ʾҴٸ�
                    // �ǹ��̳� ���, ī���Ϳ� ���� ������Ʈ�� ���̰� �浹�� ���̴�.
                    if (!hit.collider.CompareTag("Building"))
                    {
                        playerRigidbody.MovePosition(transform.position + moveVec * Time.deltaTime * playerData.moveSpeed);
                    }
                }
                else
                {
                    playerRigidbody.MovePosition(transform.position + moveVec * Time.deltaTime * playerData.moveSpeed);
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

        if (Input.GetMouseButtonUp(0))
        {
            isClicked = false;
            animator.SetBool("isMove", false);
            // touchController UI ��Ȱ��ȭ
            moveController.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Cooker") ||
            collision.gameObject.CompareTag("Storage"))
        {
            Debug.Log("Collision Enter");
            playerStack.OnEnterInteraction(collision);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Cooker"))
        {
            playerStack.InteractWithSpawner();
        }
        else if (collision.gameObject.CompareTag("Storage"))
        {
            playerStack.InteractWithReceiver();
        }
        else if (collision.gameObject.CompareTag("Upgrade"))
        {
            //Debug.Log("Collide with UpgradeButton!");
            UpgradeBox box = collision.gameObject.GetComponent<UpgradeBox>();
            if (box != null)
            {
                box.isPushed = true;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Cooker") ||
            collision.gameObject.CompareTag("Storage"))
        {
            playerStack.OnExitInteraction();
        }

        if (collision.gameObject.CompareTag("Upgrade"))
        {
            UpgradeBox box = collision.gameObject.GetComponent<UpgradeBox>();
            if (box != null)
            {
                box.isPushed = false;
            }
        }
    }
}
