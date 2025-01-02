using UnityEngine;
using UnityEngine.AI;

public class BossAnimation : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent navMeshAgent;

    void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // Check the velocity of the NavMeshAgent
        bool isMoving = navMeshAgent.velocity.sqrMagnitude > 0.1f;
        animator.SetBool("IsMoving", isMoving);
    }
}
