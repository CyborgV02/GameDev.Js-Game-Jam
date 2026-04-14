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
    public MainCharacter Character { get { return playerCharacter; } }
    

    void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerCharacter = new MainCharacter();
        InputController.OnMove += Move;
    }

    void Update()
    {
        playerRb.velocity = moveInput * moveSpeed;
    }
    public void Move(Vector2 context)
    {
        moveInput = context;
    }
}