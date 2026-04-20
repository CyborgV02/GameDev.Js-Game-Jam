using UnityEngine;

public class Node
{
    public int x;
    public int y;
    public GameObject nodeObject;

    public Node(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public void Move((int,int) newPosition)
    {
        this.x += newPosition.Item1;
        this.y += newPosition.Item2;
    }
}