using Script;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    [Header("Cible et mouvement")]
    public Transform target;
    public float spawnHeight = 10f;
    public float descentSpeed = 10f;

    [Header("Explosion")]
    public float explosionRadius = 5f;
    public float damage = 20f;

    [Header("Effets")]
    public GameObject impactEffect;
    public AudioClip explosionSound;
    public AudioSource audioSource;

    [Header("Configuration")]
    public float lifetime = 10f;
    public LayerMask affectedLayers;

    private bool isDescending = true;

    void Start()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        // Position initiale au-dessus de la cible
        Vector3 spawnPos = target.position + Vector3.up * spawnHeight;
        transform.position = spawnPos;

        // Auto-destruction si jamais elle ne touche rien
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        if (isDescending && target != null)
        {
            // Descente verticale uniquement sur l'axe Y
            Vector3 groundPosition = new Vector3(target.position.x, target.position.y, target.position.z);
            transform.position = Vector3.MoveTowards(transform.position, groundPosition, descentSpeed * Time.deltaTime);

            // Rotation vers la cible pour effet visuel
            transform.LookAt(groundPosition);

            // Vérifie si on a atteint le sol
            if (Vector3.Distance(transform.position, groundPosition) < 0.1f)
            {
                isDescending = false;
                Explode();
            }
        }
    }

    void Explode()
    {
        // Effet visuel
        if (impactEffect != null)
            Instantiate(impactEffect, transform.position, Quaternion.identity);

        // Effet sonore
        if (audioSource != null && explosionSound != null)
            audioSource.PlayOneShot(explosionSound);

        // Dégâts de zone
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius, affectedLayers);

        foreach (Collider hit in hitColliders)
        {
            // Appliquer les dégâts aux bons types d'objets
            if (hit.CompareTag("Enemy"))
            {
                Enemy enemy = hit.GetComponent<Enemy>();
                if (enemy != null)
                    enemy.TakeDamage(damage);
            }
            else if (hit.CompareTag("Base"))
            {
                Base baseObj = hit.GetComponent<Base>();
                if (baseObj != null)
                    baseObj.TakeDamage(damage);
            }
            else if (hit.CompareTag("Tower"))
            {
                Tower tower = hit.GetComponent<Tower>();
                if (tower != null)
                    tower.TakeDamage(damage);
            }
        }

        // Détruire la fireball après l'explosion
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        // Pour visualiser la zone d'explosion dans l'éditeur
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}

