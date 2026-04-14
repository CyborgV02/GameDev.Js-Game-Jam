using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRb;
    private Vector2 moveInput;
    private float moveSpeed = 5.0f;
    private MainCharacter playerCharacter;
    void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerCharacter = new MainCharacter();
    }

    void Update()
    {
        playerRb.velocity=moveInput*moveSpeed;
    }
    public void Move(InputAction.CallbackContext context)
    {
        moveInput=context.ReadValue<Vector2>();
    }
}