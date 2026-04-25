using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Bat
{
    public enum BatState
    {
        Idle,
        Attacking,
        Parrying,
        Stunned,
    }

    public float attackDuration = 0.5f;
    public float parryDuration = 1f;
    public float parryWindow = 0.3f;
    public float currentStateTimer = 0f;
    public float stunDuration = 0.3f;
    public Animator animator;
    public Action OnDamaged;
    public BatState CurrentState { get; private set; } = BatState.Idle;
    private string[] animStates = { "Idle", "Attack", "Block" };
    private int[] animStateHashes;

    public Bat(Animator animator)
    {
        this.animator = animator;
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
        if (CurrentState == BatState.Idle)
        {
            CurrentState = BatState.Attacking;
            currentStateTimer = attackDuration;
            animator.Play(animStateHashes[1]);
        }
    }
    public void Parry()
    {
        if (CurrentState == BatState.Idle)
        {
            CurrentState = BatState.Parrying;
            currentStateTimer = parryDuration;
            animator.Play(animStateHashes[2]);
        }
    }
    public void Reset()
    {
        CurrentState = BatState.Idle;
        currentStateTimer = 0f;
        // Implement reset logic here
        animator.Play(animStateHashes[0]);
    }
    public bool IsInParryWindow()
    {
        return CurrentState == BatState.Parrying && currentStateTimer <= parryWindow;
    }

    public void ParryReset()
    {
        if (CurrentState == BatState.Parrying)
        {
            // TODO : Play parry success animation and sound here
            // ---wait--- //
            Reset();
        }
    }

    public void Stun(bool isDamage = false)
    {
        CurrentState = BatState.Stunned;
        currentStateTimer = stunDuration;
        // TODO : Play stun animation and sound here
        if (isDamage) OnDamaged?.Invoke();
    }

    public void Update()
    {
        if (CurrentState == BatState.Idle) return;
        
        currentStateTimer -= Time.deltaTime;
        if (currentStateTimer <= 0f && CurrentState != BatState.Idle)
        {
            Reset();
        }
    }
}