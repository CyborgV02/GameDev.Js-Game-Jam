using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    private Rigidbody2D playerRb;
    private Vector2 moveInput;
    private float moveSpeed = 2.5f;
    [SerializeField] private MainCharacter playerCharacter;
    public MainCharacter Character { get { return playerCharacter; } }
    public Animator anim;
    

    void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerCharacter = new MainCharacter(sprites); // Pass the sprites array here
        InputController.OnMove += Move;
        anim=GetComponent<Animator>();
    }

    void Update()
    {
        playerRb.velocity = moveInput * moveSpeed;

    }
   public void Move(Vector2 context)
   {
    moveInput = context;
    anim.SetBool("Iswalking", moveInput != Vector2.zero);
    anim.SetFloat("InputX", moveInput.x);
    anim.SetFloat("InputY", moveInput.y);
   }
}