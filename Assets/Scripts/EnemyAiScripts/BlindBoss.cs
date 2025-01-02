using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossFunctionality : MonoBehaviour
{
    private NavMeshAgent agent;
    private Vector3 lastHeardPosition;
    private bool hasHeardSound;
    private bool isRoaming;

    // State duration
    private float idleTimer;
    private float idleTimeThreshold = 3f;

    // Roaming variables
    public float roamingAreaRadius = 20f;
    private Vector3 currentRoamingDestination;

    // Speed variables
    public float aggressiveSpeed = 6f;
    private float roamingSpeed;

    // State machine states
    private enum State { Roaming, Aggressive }
    private State currentState;

    public delegate void SpeedChanged(float speed);
    public event SpeedChanged OnSpeedChanged;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        PlayerFootstep.FootstepEvent += OnPlayerFootstep;
        currentState = State.Roaming;
        isRoaming = true;

        roamingSpeed = aggressiveSpeed / 2f;
        agent.speed = roamingSpeed;

        SetNewRoamingDestination();
        OnSpeedChanged?.Invoke(roamingSpeed);
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
        if (isRoaming && !agent.pathPending && agent.remainingDistance <= 1f)
        {
            SetNewRoamingDestination();
        }

        if (hasHeardSound)
        {
            currentState = State.Aggressive;
            isRoaming = false;
            idleTimer = 0f;

            agent.speed = aggressiveSpeed;
            OnSpeedChanged?.Invoke(aggressiveSpeed);
        }
    }

    void HandleAggressive()
    {
        if (hasHeardSound)
        {
            agent.SetDestination(lastHeardPosition);

            if (!agent.pathPending && agent.remainingDistance <= 1f)
            {
                hasHeardSound = false;
            }
        }
        else
        {
            idleTimer += Time.deltaTime;

            if (idleTimer >= idleTimeThreshold)
            {
                currentState = State.Roaming;
                isRoaming = true;
                SetNewRoamingDestination();
                idleTimer = 0f;

                agent.speed = roamingSpeed;
                OnSpeedChanged?.Invoke(roamingSpeed);
            }
        }
    }

    void OnPlayerFootstep(Vector3 footstepPosition, bool isCrouching)
    {
        lastHeardPosition = footstepPosition;
        hasHeardSound = true;
        idleTimer = 0f;
    }

    void SetNewRoamingDestination()
    {
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
