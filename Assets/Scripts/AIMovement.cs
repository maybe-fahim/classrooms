using UnityEngine.AI;
using UnityEngine;

public class AIMovement : MonoBehaviour
{
    [SerializeField] private Transform player; // No need for SerializeField if assigning in code
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
        // Optional: Check if the player was found
        if (player == null)
        {
            Debug.LogError("Player not found! Make sure the player GameObject is tagged as 'Player'.");
        }
    }

    void Update()
    {
        // Make sure the player is assigned before using it
        if (player != null)
        {
            agent.destination = player.position;
        }
    }
}
