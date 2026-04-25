using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Party
{
    public List<Character> allies;

    public Party(List<Character> allies)
    {
        this.allies = allies;
    }

    public void AddAlly(Character newAlly)
    {
        allies.Add(newAlly);
    }

    public void FollowLeader()
    {
        if (allies.Count < 2) return;

        Character leader = allies[0];
        Vector3 leaderPosition = leader.GameObject.transform.position;

        for (int i = 1; i < allies.Count; i++)
        {
            if (allies[i] == leader) continue; // Skip the leader
            Character ally = allies[i];
            Vector3 targetPosition = leaderPosition - new Vector3(i * 1.5f, 0, 0); // Position allies in a line behind the leader
            ally.GameObject.transform.position = Vector3.Lerp(ally.GameObject.transform.position, targetPosition, Time.deltaTime * 5f);
        }
    }
}