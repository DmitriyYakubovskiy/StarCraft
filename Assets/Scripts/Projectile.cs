using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private GameObject impactEffectPrefab;
    [SerializeField] private GameObject muzzleFlashPrefab;
    [SerializeField] private AudioClip hitSound;

    [SerializeField] private float speed = 20f;
    [SerializeField] private float damage = 10f;               
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private float volume = 0.1f;

    private Rigidbody rb;

    public GameObject Owner { get; set; }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.linearVelocity = transform.forward * speed;

        if (muzzleFlashPrefab)
        {
            var muzzleFlash = Instantiate(muzzleFlashPrefab, transform.position, transform.rotation);
            Destroy(muzzleFlash, 1f);
        }

        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Owner) return;

        var health = other.GetComponent<HealthController>();
        if (health != null) health.TakeDamage(damage);

        GameObject effect = Instantiate(impactEffectPrefab, transform);
        AudioSource.PlayClipAtPoint(hitSound, transform.position, volume);

        Destroy(gameObject);
    }
}
