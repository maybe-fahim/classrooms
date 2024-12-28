using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BlindBoss : MonoBehaviour
{
    private NavMeshAgent agent;
    private Vector3 lastHeardPosition;
    private bool hasHeardSound;
    private bool isRoaming;

    // State duration
    private float idleTimer;
    private float idleTimeThreshold = 3f; // Time before returning to roaming after hearing no sound

    // Roaming variables
    public float roamingAreaRadius = 20f; // Roaming area radius
    private Vector3 currentRoamingDestination;

    // Speed variables
    public float aggressiveSpeed = 6f; // Default aggressive speed
    private float roamingSpeed; // Roaming speed is half of aggressive speed

    // State machine states
    private enum State { Roaming, Aggressive }
    private State currentState;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        PlayerFootstep.FootstepEvent += OnPlayerFootstep;
        currentState = State.Roaming;
        isRoaming = true;

        // Set speeds
        roamingSpeed = aggressiveSpeed / 2f; // Roaming speed is half of aggressive speed
        agent.speed = roamingSpeed;

        SetNewRoamingDestination();
    }

    void OnDestroy()
    {
        PlayerFootstep.FootstepEvent -= OnPlayerFootstep;
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Roaming:
                HandleRoaming();
                break;

            case State.Aggressive:
                HandleAggressive();
                break;
        }
    }

    void HandleRoaming()
    {
        // If the boss is roaming, it will wait until it reaches the roaming destination
        if (isRoaming)
        {
            if (!agent.pathPending && agent.remainingDistance <= 1f)
            {
                // The boss has reached its destination, now it can pick a new one
                SetNewRoamingDestination();
            }

            // If the boss hasn't heard any sound for a while, it stays in roaming
            if (!hasHeardSound)
            {
                return;
            }
        }

        // Switch to aggressive if the boss hears a footstep
        if (hasHeardSound)
        {
            currentState = State.Aggressive;
            isRoaming = false;
            idleTimer = 0f;

            // Switch to aggressive speed
            agent.speed = aggressiveSpeed;
        }
    }

    void HandleAggressive()
    {
        // Move towards the last heard position
        if (hasHeardSound)
        {
            agent.SetDestination(lastHeardPosition);
        }
        else
        {
            // If no new footstep sound is heard, we wait for some time before returning to roaming
            idleTimer += Time.deltaTime;

            if (idleTimer >= idleTimeThreshold)
            {
                // The boss has arrived at the position and has waited for 3 seconds without hearing anything
                currentState = State.Roaming;
                isRoaming = true;
                SetNewRoamingDestination();
                idleTimer = 0f;

                // Switch back to roaming speed
                agent.speed = roamingSpeed;
            }
        }
    }

    void OnPlayerFootstep(Vector3 footstepPosition, bool isCrouching)
    {
        // Store the last footstep position and set the boss to aggressive state
        lastHeardPosition = footstepPosition;
        hasHeardSound = true;

        // Reset the idle timer to zero whenever a new footstep is heard
        idleTimer = 0f;
    }

    void SetNewRoamingDestination()
    {
        // Pick a new random destination within the roaming area
        Vector3 randomDirection = Random.insideUnitSphere * roamingAreaRadius;
        randomDirection += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, roamingAreaRadius, 1))
        {
            currentRoamingDestination = hit.position;
            agent.SetDestination(currentRoamingDestination);
        }
    }
}
