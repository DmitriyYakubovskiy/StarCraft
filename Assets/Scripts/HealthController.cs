using UnityEngine;
using System.Collections;

public class HealthController : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100;
    private float currentHealth;

    private Renderer objectRenderer;
    private Color originalColor;
    [SerializeField] private Color damageColor = Color.red;
    [SerializeField] private float flashDuration = 0.2f;

    private void Start()
    {
        currentHealth = maxHealth;

        objectRenderer = GetComponentInChildren<Renderer>();
        if (objectRenderer != null) originalColor = objectRenderer.material.color;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"Player health: {currentHealth}");

        if (objectRenderer != null) StartCoroutine(DamageFlash());

        if (currentHealth <= 0) Die();
    }

    private IEnumerator DamageFlash()
    {
        objectRenderer.material.color = damageColor;
        yield return new WaitForSeconds(flashDuration);
        objectRenderer.material.color = originalColor;
    }

    private void Die()
    {
        Debug.Log("Player died!");
        Destroy(gameObject);
    }
}