using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EnemyType
{
    Wilson,
}
public class EnemyController : MonoBehaviour
{
    private enum EnemyAnimationState
    {
        Idle,
        Walking,
    }

    public Enemy enemy;
    public EnemyType enemyType;
    public Transform pointA;
    public Transform pointB;
    public Transform playerTransform;
    public float ChaseRange = 5f;
    public float ChaseSpeed = 2f;
    public Transform currentTarget;
    public Animator anim;

    private EnemyAnimationState currentAnimationState = EnemyAnimationState.Idle;
    private static readonly int IdleHash = Animator.StringToHash("Idle");
    private static readonly int WalkingHash = Animator.StringToHash("Walk");
    private const float AnimationBlendTime = 0.1f;
    private Vector3 startingLocalScale;


    void Chase()
    {
        if (playerTransform == null)
        {
            Debug.LogError("Player Transform is not assigned in EnemyController.");
            SetAnimationState(EnemyAnimationState.Idle);
            return;
        }
        if (enemy == null)
        {
            Debug.LogError("Enemy is not initialized in EnemyController.");
            SetAnimationState(EnemyAnimationState.Idle);
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        if (distanceToPlayer < ChaseRange)
        {
            currentTarget = playerTransform;
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            UpdateFacing(direction);
            transform.position += (Vector3)(direction * ChaseSpeed * Time.deltaTime);
            SetAnimationState(EnemyAnimationState.Walking);
            return;
        }

        if (pointA == null || pointB == null)
        {
            SetAnimationState(EnemyAnimationState.Idle);
            return;
        }

        if (currentTarget == null || currentTarget == playerTransform)
        {
            currentTarget = pointA;
        }

        Vector2 directionToTarget = (currentTarget.position - transform.position).normalized;
        UpdateFacing(directionToTarget);
        transform.position += (Vector3)(directionToTarget * ChaseSpeed * Time.deltaTime);
        SetAnimationState(EnemyAnimationState.Walking);

        if (Vector2.Distance(transform.position, currentTarget.position) < 0.1f)
        {
            currentTarget = (currentTarget == pointA) ? pointB : pointA;
        }
    }

    void Awake()
    {
        anim = GetComponent<Animator>();
        startingLocalScale = transform.localScale;

        switch (enemyType)
        {
            case EnemyType.Wilson:
                enemy = new Wilson();
                break;
            default:
                Debug.LogError("Unsupported enemy type: " + enemyType);
                break;
        }

        enemy.OnDefeated += () =>
        {
            Debug.Log($"Enemy {enemy.Name} has been defeated.");
            Destroy(gameObject);
        };
    }

    void UpdateFacing(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) < 0.01f)
        {
            return;
        }

        float facing = direction.x > 0f ? 1f : -1f;
        transform.localScale = new Vector3(Mathf.Abs(startingLocalScale.x) * facing, startingLocalScale.y, startingLocalScale.z);
    }

    void WalkAround()
    {
        if (currentTarget == null)
        {
            SetAnimationState(EnemyAnimationState.Idle);
            return;
        }
        if (currentTarget == pointA || currentTarget == pointB)
        {
            if (Vector2.Distance(transform.position, currentTarget.position) < 0.1f)
            {
                currentTarget = (currentTarget == pointA) ? pointB : pointA;
            }
        }
    }

    void SetAnimationState(EnemyAnimationState newState)
    {
        if (currentAnimationState == newState)
        {
            return;
        }

        currentAnimationState = newState;

        if (anim != null)
        {
            int stateHash = newState == EnemyAnimationState.Walking ? WalkingHash : IdleHash;
            anim.CrossFadeInFixedTime(stateHash, AnimationBlendTime, 0);
        }
    }

    void Update()
    {
        Chase();
        WalkAround();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"Enemy {enemy.Name} collided with player");
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController == null || playerController.Character == null)
            {
                Debug.LogError("PlayerController or Character is missing on the colliding player.");
                return;
            }

            if (BattleController.Instance == null)
            {
                Debug.LogError("BattleController.Instance is null. Cannot start battle.");
                return;
            }

            Enemy[] enemies = new Enemy[] { enemy };
            Character[] allies = new Character[] { playerController.Character };
            BattleController.Instance.RequestBattleStart(allies, enemies);
        }
    }

}
