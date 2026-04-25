using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Arm
{
    public enum ArmState
    {
        Idle,
        Attacking,
        Stunned,
    }

    private Bat bat;
    public float attackDuration = 0.5f;
    public float stunDuration = 1f;
    public float shortStunDuration = 0.4f;
    public float currentStateTimer = 0f;
    public float timeBetweenAttacks = 1f;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public ArmState CurrentState { get; private set; } = ArmState.Idle;
    public Action OnDamaged;
    private string[] animStates = { "Idle", "Attack", "Block" };
    private int[] animStateHashes;
    public Arm(Bat bat, Animator animator, SpriteRenderer spriteRenderer)
    {
        this.bat = bat;
        this.animator = animator;
        this.spriteRenderer = spriteRenderer;
        InitAnims();
    }

    void InitAnims()
    {
        animStateHashes = new int[animStates.Length];
        foreach (string state in animStates)
        {
            animStateHashes[Array.IndexOf(animStates, state)] = Animator.StringToHash(state);
        }
    }

    public void Attack()
    {
        if (CurrentState == ArmState.Idle)
        {
            spriteRenderer.color = Color.white;
            // TODO : Play replace with actual arm
            CurrentState = ArmState.Attacking; 
            currentStateTimer = attackDuration;
            animator.Play(animStateHashes[1]);
        }
    }

    public void Stun(bool isParrySuccess, bool isDamage = false)
    {
        if (isParrySuccess)
        {
            CurrentState = ArmState.Stunned;
            currentStateTimer = stunDuration;
            // TODO : Play stun animation and sound here
            animator.Play(animStateHashes[0]);
        }
        else
        {
            CurrentState = ArmState.Stunned;
            currentStateTimer = shortStunDuration;
            // TODO : Play hit animation and sound here
            animator.Play(animStateHashes[0]);
        }
        if (isDamage)
        {
            OnDamaged?.Invoke();
        }
    }

    public void Reset()
    {
        CurrentState = ArmState.Idle;
        currentStateTimer = 0f;
        // Implement reset logic here
        animator.Play(animStateHashes[0]);
    }

    public void Update()
    {
        if (CurrentState == ArmState.Idle) { if (timeBetweenAttacks <= 0f) { Attack(); timeBetweenAttacks = 1f; } else { spriteRenderer.color = Color.red; timeBetweenAttacks -= Time.deltaTime; }}

        currentStateTimer -= Time.deltaTime;
        if (CurrentState == ArmState.Attacking && bat.IsInParryWindow())
        {
            Stun(true, true);
            bat.ParryReset();
        }
        if (bat.CurrentState == Bat.BatState.Attacking && CurrentState != ArmState.Attacking)
        {
            Stun(false, true);
            bat.Reset();
        }
        if (bat.CurrentState == Bat.BatState.Attacking && CurrentState == ArmState.Attacking)
        {
            Stun(false, false);
            bat.Stun(false);
            bat.Reset();
        }
        if (bat.CurrentState != Bat.BatState.Stunned && !bat.IsInParryWindow() && CurrentState == ArmState.Attacking)
        {
            bat.Stun(true);
        }
        if (currentStateTimer <= 0f)
        {
            Reset();
        }
    }
}