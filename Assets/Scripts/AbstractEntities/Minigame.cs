using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMinigame
{
    public string name { get; }
    public float duration { get; }
    public int allyDamage { get; }
    public int enemyDamage { get; }

    public void StartMinigame();
    public void EndMinigame();
    public void UpdateMinigame(float deltaTime);
    public void HandleInput();
    public void DamageAllies(Character[] allies);
    public void DamageEnemies(Enemy[] enemies);
}
