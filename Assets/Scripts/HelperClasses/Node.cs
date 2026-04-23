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
    public void SnapToPosition((int, int) newPosition)
    {
        if (nodeObject != null)
        {
            nodeObject.transform.localPosition = new Vector3(newPosition.Item1 * travelDistance, newPosition.Item2 * travelDistance, nodeObject.transform.localPosition.z);
        }
    }
    public void Move((int, int) newPosition)
    {
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
        Vector3 xTarget = new Vector3(targetPosition.x, startPosition.y, startPosition.z);
        Vector3 yTarget = new Vector3(targetPosition.x, targetPosition.y, startPosition.z);

        // Keep total move time close to moveDuration, split by distance per axis
        float xDist = Mathf.Abs(xTarget.x - startPosition.x);
        float yDist = Mathf.Abs(yTarget.y - xTarget.y);
        float total = xDist + yDist;


        if (xDist > 0f)
            yield return MoveSegment(startPosition, xTarget, moveDuration);

        if (yDist > 0f)
            yield return MoveSegment(xTarget, yTarget, moveDuration);

        nodeObject.transform.localPosition = targetPosition;
        isMoving = false;
    }

    private IEnumerator MoveSegment(Vector3 from, Vector3 to, float duration)
    {
        if (duration <= 0f)
        {
            nodeObject.transform.localPosition = to;
            yield break;
        }

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            nodeObject.transform.localPosition = Vector3.Lerp(from, to, t);
            yield return null;
        }

        nodeObject.transform.localPosition = to;
    }
}