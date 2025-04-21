using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 10f;
    private Animator animator;
    [Header("Déplacement")]
    public float speed = 0.05f;
    public float rotationSpeed = 5f;
    private Vector3 initialDirection;

    [Header("Cibles")]
    public Transform target; // cible principale (ex: base)
    private Transform attackTarget; // cible détectée dans le champ
    private readonly List<Transform> targetsInRange = new();

    [Header("Attaque")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float damage = 1f;
    public float shootCooldown = 2f;

    private float shootTimer = 0f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        // Direction initiale aléatoire sur le plan XZ
        Vector2 randomXZ = Random.insideUnitCircle.normalized;
        initialDirection = new Vector3(randomXZ.x, 0, randomXZ.y);

        // Positionne l'ennemi en train de se déplacer dès le départ
        if (target == null)
            transform.rotation = Quaternion.LookRotation(initialDirection);
    }

    void Update()
    {
        // Se déplace vers la cible principale
        if (target != null)
        {
            MoveTowardsTarget(target);
            RotateTowardsTarget(target);
        }
        else
        {
            // Si aucune cible, continue dans la direction initiale
            transform.position += initialDirection * speed * Time.deltaTime;
        }

        // Gère les tirs si une cible est détectée
        if (attackTarget != null)
        {
            shootTimer += Time.deltaTime;
            if (shootTimer >= shootCooldown)
            {
                Shoot();
                shootTimer = 0f;
            }
        }
    }

    public void Initialize(GameObject targetObject)
    {
        if (targetObject != null)
            target = targetObject.transform;
    }
    
    public bool isAlreadyHit = false;  // Nouveau drapeau pour savoir si l'ennemi a déjà été touché
    public float hitCooldown = 1f;     // Délai avant que l'ennemi puisse être touché à nouveau (en secondes)

    public void TakeDamage(float amount)
    {
        Debug.Log($"Enemy taking damage {amount}, {health} left");
        if (!isAlreadyHit)  // Si l'ennemi n'a pas encore été touché
        {
            health -= amount;
            isAlreadyHit = true;  // L'ennemi est maintenant marqué comme touché

            // Démarre une coroutine pour réinitialiser le drapeau après un délai
            StartCoroutine(ResetHitCooldown());
        }

        if (health <= 0f)
        {
            Die();
        }
    }

    IEnumerator ResetHitCooldown()
    {
        // Attendre la durée du cooldown avant de permettre à l'ennemi d'être touché à nouveau
        yield return new WaitForSeconds(hitCooldown);
        isAlreadyHit = false;  // Réinitialiser le drapeau après le délai
    }
        
    private void Die()
    {
        if (animator != null)
        {
            GameManager.Instance.AddScore(10);
            animator.SetTrigger("Die");
            Destroy(gameObject, 2f); // Laisse le temps à l'animation
        }
        else
        {
            Debug.LogWarning("Animator non trouvé !");
            Destroy(gameObject);
        }
    }
    

    protected void MoveTowardsTarget(Transform tgt)
    {
        Vector3 direction = (tgt.position - transform.position).normalized;
        transform.position = Vector3.MoveTowards(transform.position, tgt.position, speed * Time.deltaTime);
    }

    protected void RotateTowardsTarget(Transform tgt)
    {
        Vector3 direction = (tgt.position - transform.position).normalized;
        direction.y = 0;
        if (direction == Vector3.zero) return;

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
    }

    protected void Shoot()
    {
        if (attackTarget == null) return;

        GameObject projectileGO = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        Collider myCollider = GetComponent<Collider>();
        Collider projectileCollider = projectileGO.GetComponent<Collider>();
        if (myCollider != null && projectileCollider != null)
        {
            Physics.IgnoreCollision(projectileCollider, myCollider);
        }

        Projectile projectile = projectileGO.GetComponent<Projectile>();
        if (projectile != null)
        {
            projectile.target = attackTarget;
            projectile.damage = damage;
            projectile.shooterTag = gameObject.tag; // Définir le tag du tireur
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Base") || other.CompareTag("Tower"))
        {
            Transform detected = other.transform;
            if (!targetsInRange.Contains(detected))
            {
                targetsInRange.Add(detected);

                if (attackTarget == null)
                    attackTarget = detected;
            }
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Base") || other.CompareTag("Tower"))
        {
            Transform detected = other.transform;
            targetsInRange.Remove(detected);

            if (attackTarget == detected)
            {
                attackTarget = targetsInRange.Count > 0 ? targetsInRange[0] : null;
            }
        }
    }
}
