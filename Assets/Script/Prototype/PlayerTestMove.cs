using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 240822 ������
/// �÷��̾ �ӽ÷� �����̱� ���� ��ũ��Ʈ
/// </summary>

public class PlayerTestMove : MonoBehaviour
{
    public float speed = 6.0f;        // �÷��̾� �̵� �ӵ�
    public float gravity = -9.81f;    // �߷�
    public CharacterController controller;
    public playerStack stack;

    private Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        stack = GetComponentInChildren<playerStack>();
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        controller.Move(move * speed * Time.deltaTime);

        // �߷� ����
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;  // ���鿡 ���� �� Y�� �ӵ� �ʱ�ȭ
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
       stack.OnEnterInteraction(other);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("kitchen"))
        {
            stack.InteractWithSpawner();
        }
        else if (other.CompareTag("desk"))
        {
            stack.InteractWithReceiver();
        }
    }

    void OnTriggerExit(Collider other)
    {
        stack.OnExitInteraction();
    }
}
