using UnityEngine;
using System.Collections;
using Unity.Mathematics;

public class Node
{
    public int x;
    public int y;
    public GameObject nodeObject;
    private MonoBehaviour coroutineRunner;
    private float travelDistance = 3.19f;
    private bool isMoving = false;
    private float moveDuration = 0.25f;

    public Node(int x, int y, GameObject nodeObject = null, MonoBehaviour coroutineRunner = null)
    {
        this.x = x;
        this.y = y;
        this.nodeObject = nodeObject;
        this.coroutineRunner = coroutineRunner;
        if (nodeObject != null && coroutineRunner)
        {
            nodeObject.transform.localPosition = new Vector3(x * travelDistance, y * travelDistance, nodeObject.transform.localPosition.z);
        }
    }

    public void Move((int, int) newPosition)
    {
        if (math.abs(newPosition.Item1) == 1 && math.abs(newPosition.Item2) == 1)
            {
                int _movement = UnityEngine.Random.Range(0, 2);
                if (_movement == 0)
                {
                    newPosition.Item1 = 0;
                }
                else
                {
                    newPosition.Item2 = 0;
                }
            }
        this.x += newPosition.Item1;
        this.y += newPosition.Item2;
        _TransformMove(newPosition);
    }

    private void _TransformMove((int, int) newPosition)
    {
        if (nodeObject != null && coroutineRunner != null && !isMoving)
        {
            Vector3 targetPosition = new Vector3(nodeObject.transform.localPosition.x + newPosition.Item1 * travelDistance, nodeObject.transform.localPosition.y + newPosition.Item2 * travelDistance, nodeObject.transform.localPosition.z);
            coroutineRunner.StartCoroutine(SmoothMove(targetPosition));
        }
    }

    private IEnumerator SmoothMove(Vector3 targetPosition)
    {
        if (isMoving)
            yield break;

        isMoving = true;
        Vector3 startPosition = nodeObject.transform.localPosition;
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            nodeObject.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);
            yield return null;
        }

        nodeObject.transform.localPosition = targetPosition;
        isMoving = false;
    }
}