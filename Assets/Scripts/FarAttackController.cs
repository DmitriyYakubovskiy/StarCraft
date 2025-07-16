using System.Collections;
using UnityEngine;

public class FarAttackController : MonoBehaviour
{
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform[] points;
    [SerializeField] private float volume = 0.5f;
    [SerializeField] private float fireRate = 1f;

    private AudioSource audioSource;
    private Animator animator;
    private bool canShoot = true;

    public bool CanShoot { get => canShoot; }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    public void TryShoot()
    {
        if (!canShoot) return;
        StartCoroutine(ShootRoutine());
    }

    private IEnumerator ShootRoutine()
    {
        canShoot = false;
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(1f / fireRate);

        canShoot = true;
    }

    private void ShootFromPoint()
    {
        for (int i = 0; i < 2; i++)
        {
            Transform firePoint = points[i];

            if (firePoint == null || projectilePrefab == null) return;

            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            audioSource.PlayOneShot(attackSound, volume);
            projectile.gameObject.GetComponent<Projectile>().Owner = gameObject;
        }
    }
}
