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
    private bool playingFootsteps = false;
    public float footstepsSpeed = 0.5f;
    private bool CanMove => !PauseManager.IsGamePaused && (BattleController.CurrentBattle == null);



    void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerCharacter = new MainCharacter(sprites); // Pass the sprites array here
        InputController.OnMove += Move;
        anim = GetComponent<Animator>();
        playerCharacter.GameObject = gameObject; // Link the Character to this GameObject
        playerCharacter.OnDefeated += () =>
        {
            Debug.Log("Player has been defeated!");
            // GAME OVER HERE

        };
    }

    void Update()
    {
        playerRb.velocity = moveInput * moveSpeed;

        if (PauseManager.IsGamePaused)
        {
            playerRb.velocity = Vector2.zero;
            StopFootsteps();
        }
        if (playerRb.velocity.magnitude > 0 && !playingFootsteps)
        {
            StartFootsteps();
        }
        else if (playerRb.velocity.magnitude == 0)
        {
            StopFootsteps();
        }

    }
    public void Move(Vector2 context)
    {
        if (!CanMove) {
            moveInput = Vector2.zero;
            anim.SetBool("Iswalking", false);
            anim.SetFloat("InputX", 0);
            anim.SetFloat("InputY", 0);
            return;
        };
        moveInput = context;
        anim.SetBool("Iswalking", moveInput != Vector2.zero);
        anim.SetFloat("InputX", moveInput.x);
        anim.SetFloat("InputY", moveInput.y);
    }

    void StartFootsteps()
    {
        playingFootsteps = true;
        SFXManager.play("Footsteps");
        InvokeRepeating(nameof(PlayFootsteps), 0f, footstepsSpeed);
    }

    void StopFootsteps()
    {
        playingFootsteps = false;
        CancelInvoke(nameof(PlayFootsteps));
    }

    void PlayFootsteps()
    {
        SFXManager.play("Footsteps");
    }
}