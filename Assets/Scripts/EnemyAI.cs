using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float attackDuration = 1f;
    [SerializeField] private float damage = 10f;

    private NavMeshAgent agent;
    private Animator animator;
    private Transform player;
    private MoveController moveController;
    private float attackRangeCoefficient = 2f;
    private bool canAttack = true;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        moveController = GetComponent<MoveController>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (!canAttack) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange) StartCoroutine(AttackRoutine());
        else if (distanceToPlayer <= detectionRange) MoveTowardsPlayer();
        else Idle();
    }

    private void MoveTowardsPlayer()
    {
        agent.SetDestination(player.position);
    }

    private void Idle()
    {
        agent.SetDestination(transform.position);
    }

    private IEnumerator AttackRoutine()
    {
        canAttack = false;

        Idle();
        animator.SetTrigger("Attack");

        Vector3 direction = (player.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

        yield return new WaitForSeconds(attackDuration);
        yield return new WaitForSeconds(attackCooldown);

        canAttack = true;
    }

    public void DealDamage()
    {
        Debug.Log("Enemy dealt damage to player");
        if (Vector3.Distance(transform.position, player.position) <= attackRange * attackRangeCoefficient) player.GetComponent<HealthController>().TakeDamage(damage);
    }
}