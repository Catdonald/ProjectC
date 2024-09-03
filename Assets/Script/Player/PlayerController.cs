using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    GameObject characterRoot;
    [SerializeField]
    GameObject touchController;

    public playerStack playerStack;
    private PlayerData playerData;
    private Rigidbody playerRigidbody;
    private JoyStickController joystickController;

    private Vector3 mouseClickedPos;
    private bool isClicked = false;

    private int burgerStackCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        playerData = GetComponent<PlayerData>();
        playerRigidbody = GetComponent<Rigidbody>();
        joystickController = touchController.GetComponentInChildren<JoyStickController>();
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
                // touchController UI 활성화
                touchController.SetActive(true);
                Vector3 touchControllerPos = new Vector3(mouseClickedPos.x - Screen.width / 2, mouseClickedPos.y - Screen.height / 2, mouseClickedPos.z);
                touchController.GetComponent<RectTransform>().localPosition = touchControllerPos;
            }
            else
            {
                // 클릭된 위치와 현재 커서 위치를 통해 이동 방향 구하기
                Vector3 mousePos = Input.mousePosition;
                Vector3 mouseDelta = mousePos - mouseClickedPos;
                // max padSizeX = 35.0f
                Vector3 mouseDeltaNorm = new Vector3(mouseDelta.x / 35.0f, mouseDelta.y / 35.0f, mouseDelta.z);
                if (mouseDeltaNorm.magnitude > 1.0f)
                {
                    mouseDeltaNorm = mouseDelta.normalized;
                }
                Vector3 moveVec = new Vector3(mouseDeltaNorm.x, 0.0f, mouseDeltaNorm.y);
                playerRigidbody.MovePosition(transform.position + moveVec * Time.deltaTime * playerData.speed);
                // 이동하는 방향 바라보기
                if (moveVec.magnitude > 0.0f)
                {
                    characterRoot.transform.forward = moveVec;
                }

                // 마우스 위치 변화한 정도 JoyStickController에 넘겨주기
                joystickController.mouseDelta = mouseDelta;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isClicked = false;
            // touchController UI 비활성화
            touchController.SetActive(false);
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
            Debug.Log("Collide with Cooker!");
            playerStack.InteractWithSpawner();
        }
        else if (collision.gameObject.CompareTag("Storage"))
        {
            Debug.Log("Collide with Table!");
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
