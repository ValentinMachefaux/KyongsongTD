using System.Collections;
using System.Collections.Generic;
using Script;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public Vector3 targetPosition;  // Position de la cible
    public float damage = 2f;       // Dégâts de la boule de feu
    public float speed = 0.5f;        // Vitesse de la boule de feu
    public string shooterTag;        // Tag du tireur (qui peut être "Tower" ou "Enemy")
    public float lifetime = 1f;      // Durée de vie de la boule de feu avant qu'elle disparaisse
    public float effectDamageRadius = 1f;    

    public GameObject explosionEffectPrefab;  // Effet d'explosion
    public AudioClip impactSound;             // Son d'impact
    public AudioSource audioSourcePrefab;     // Source audio pour l'impact
    public GameObject trailEffectPrefab;      // Effet de traînée

    private bool hasExploded = false;         // Si la boule a explosé ou non
    private Vector3 direction;                // Direction vers la cible
    private float trailDuration = 0.2f;         // Durée de la traînée de la boule de feu
    private float offsetY = 0.5f;         // Durée de la traînée de la boule de feu

    void Start()
    {
        // Positionner la Fireball 5 unités au-dessus de la cible
        transform.position = targetPosition + new Vector3(0, offsetY, 0);

        // Calculer la direction vers la cible
        direction = (targetPosition - transform.position).normalized;

        // Ajouter un effet de traînée
        if (trailEffectPrefab != null)
        {
            Instantiate(trailEffectPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject, lifetime);  // Détruire la Fireball après un certain temps
    }

    void Update()
    {
        if (!hasExploded)
        {
            // Déplacement de la Fireball vers la cible
            transform.position += direction * speed * Time.deltaTime;

            // Vérifier si la Fireball est arrivée à la position cible
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                Explode();
            }
        }
    }

    void Explode()
    {
        if (hasExploded) return;
        hasExploded = true;

        // Créer l'effet d'explosion
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }

        // Appliquer les dégâts de zone
        ApplyAreaOfEffectDamage();

        // Jouer le son d'impact
        PlayImpactSound();

        // Détruire la Fireball après l'explosion
        Destroy(gameObject);
    }

    void ApplyAreaOfEffectDamage()
    {
        // Zone d'effet : Détecter les objets dans une certaine portée de l'impact
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, effectDamageRadius);
        List<Enemy> touchedEnemies = new();

        foreach (var hitCollider in hitColliders)
        {
            // Si l'objet est un ennemi et que la Fireball a été lancée par une tour
            if (hitCollider.CompareTag("Enemy") && shooterTag == "Tower")
            {
                var enemy = hitCollider.GetComponent<Enemy>();
                if (enemy != null && !enemy.isAlreadyHit)
                {
                    enemy.TakeDamage(damage);
                    enemy.isAlreadyHit = true;
                    touchedEnemies.Add(enemy);
                }
            }
            // Si la Fireball est lancée par un ennemi, elle touche les tours ou bases
            else if ((hitCollider.CompareTag("Tower") || hitCollider.CompareTag("Base")) && shooterTag == "Enemy")
            {
                var tower = hitCollider.GetComponent<Tower>();
                var baseObject = hitCollider.GetComponent<Base>();
                if (tower != null) tower.TakeDamage(damage);
                if (baseObject != null) baseObject.TakeDamage(damage);
            }
        }

        StartCoroutine(ResetEnemiesHit(touchedEnemies, 0.2f)); // délai en secondes
    }

    IEnumerator ResetEnemiesHit(List<Enemy> enemies, float delay)
    {
        yield return new WaitForSeconds(delay);
        foreach (var enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.isAlreadyHit = false;
            }
        }
    }

    void PlayImpactSound()
    {
        if (impactSound != null && audioSourcePrefab != null)
        {
            AudioSource tempAudio = Instantiate(audioSourcePrefab, transform.position, Quaternion.identity);
            tempAudio.clip = impactSound;
            tempAudio.Play();
            Destroy(tempAudio.gameObject, impactSound.length); // Supprimer le son après qu'il a joué
        }
    }
}
