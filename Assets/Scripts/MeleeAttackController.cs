using UnityEngine;
using System.Collections;

public class MeleeAttackController : MonoBehaviour
{
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float attackDuration = 1f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float attackRangeCoefficient = 2f;
    [SerializeField] private float volume = 0.5f;

    private Transform player;
    private AudioSource audioSource;
    private Animator animator;
    private bool canAttack = true;

    public bool CanAttack => canAttack;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public bool IsInRange()
    {
        return Vector3.Distance(transform.position, player.position) <= attackRange;
    }

    public IEnumerator AttackRoutine(System.Action OnAttackStarted = null, System.Action OnAttackFinished = null)
    {
        if (!canAttack) yield break;

        canAttack = false;
        if (OnAttackStarted != null) OnAttackStarted();

        animator.SetTrigger("Attack");
        Vector3 direction = (player.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        if (attackSound != null) audioSource.PlayOneShot(attackSound, volume);
        yield return new WaitForSeconds(attackDuration);
        if (OnAttackFinished != null) OnAttackFinished();
        yield return new WaitForSeconds(attackCooldown);

        canAttack = true;
    }

    private void DealDamage()
    {
        if (Vector3.Distance(transform.position, player.position) <= attackRange * attackRangeCoefficient)
        {
            player.GetComponent<HealthController>().TakeDamage(damage);
            Debug.Log("Enemy dealt damage to player");
        }
    }
}
