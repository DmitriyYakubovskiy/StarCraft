using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private List<Transform> patrolPoints; 
    [SerializeField] private float patrolPointThreshold = 1f;

    private NavMeshAgent agent;
    private MeleeAttackController meleeAttackController;
    private Transform player;

    private int currentPatrolIndex = 0;
    private bool isPlayerDetected = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        meleeAttackController = GetComponent<MeleeAttackController>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (patrolPoints == null || patrolPoints.Count == 0) Debug.LogWarning("Ќужно добавить точки дл€ патрулировани€.");
        else agent.SetDestination(patrolPoints[currentPatrolIndex].position);
    }

    private void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange) isPlayerDetected = true;
        else isPlayerDetected = false;

        if (isPlayerDetected) HandleChaseAndAttack(distanceToPlayer);
        else Patrol();
    }


    private void HandleChaseAndAttack(float distanceToPlayer)
    {
        if (!meleeAttackController.CanAttack) return;

        if (meleeAttackController.IsInRange())
        {
            Idle();
            StartCoroutine(meleeAttackController.AttackRoutine(Idle));
        }
        else if (distanceToPlayer <= detectionRange)
        {
            MoveTowardsPlayer();
        }
        else
        {
            Idle();
        }
    }

    private void Patrol()
    {
        if (patrolPoints == null || patrolPoints.Count == 0) return;

        if (!agent.pathPending && agent.remainingDistance <= patrolPointThreshold)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }
    }

    private void MoveTowardsPlayer()
    {
        agent.SetDestination(player.position);
    }

    private void Idle()
    {
        agent.SetDestination(transform.position);
    }
}
