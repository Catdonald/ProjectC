using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 240822 오수안
/// 플레이어를 임시로 움직이기 위한 스크립트
/// </summary>

public class PlayerTestMove : MonoBehaviour
{
    public float speed = 6.0f;        // 플레이어 이동 속도
    public float gravity = -9.81f;    // 중력
    public CharacterController controller;
    public burgerStack stack;

    private Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        stack = GetComponentInChildren<burgerStack>();
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        controller.Move(move * speed * Time.deltaTime);

        // 중력 적용
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;  // 지면에 있을 때 Y축 속도 초기화
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        switch (other.transform.tag)
        {
            case "kitchen":
                {
                    stack.OnEnterKitchen(0, other);
                }
                break;
            case "desk":
                {
                    stack.OnEnterTable(0, other);
                }
                break;
        }

    }

    void OnTriggerStay(Collider other)
    {
        switch (other.transform.tag)
        {
            case "kitchen":
                {
                    stack.TryGetBurger();
                }
                break;
            case "desk":
                {
                    stack.TrySetBurger();
                }
                break;
        }

    }

    void OnTriggerExit(Collider other)
    {
        switch (other.transform.tag)
        {
            case "kitchen":
                {

                    stack.OnExitKitchen();

                }
                break;
            case "desk":
                {
                    stack.OnExitTable();
                }
                break;
        }
    }
}
