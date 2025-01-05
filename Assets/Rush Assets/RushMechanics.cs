using System.Collections.Generic;
using UnityEngine;

public class RushPathFollower : MonoBehaviour
{
    public float speed = 10f;
    private Queue<Transform> pathQueue = new Queue<Transform>();
    private Transform currentTarget;

    public void InitializePath(List<Transform> path)
    {
        foreach (var anchor in path)
        {
            pathQueue.Enqueue(anchor);
        }

        SetNextTarget();
    }

    private void Update()
    {
        if (currentTarget != null)
        {
            MoveTowardsTarget(currentTarget);

            if (Vector3.Distance(transform.position, currentTarget.position) < 0.1f)
            {
                SetNextTarget();
            }
        }
    }

    private void MoveTowardsTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
    }

    private void SetNextTarget()
    {
        if (pathQueue.Count > 0)
        {
            currentTarget = pathQueue.Dequeue();
        }
        else
        {
            currentTarget = null;
            Debug.Log("Rush has completed the path!");
        }
    }
}
