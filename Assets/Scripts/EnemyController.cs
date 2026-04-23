using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EnemyType
{
    Wilson,
}
public class EnemyController : MonoBehaviour
{
    public Enemy enemy;
    public EnemyType enemyType;

    void Awake()
    {
        switch (enemyType)
        {
            case EnemyType.Wilson:
                enemy = new Wilson();
                break;
            default:
                Debug.LogError("Unsupported enemy type: " + enemyType);
                break;
        }
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
