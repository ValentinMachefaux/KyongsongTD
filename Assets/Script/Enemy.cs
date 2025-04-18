using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Déplacement")]
    public float speed = 2f;
    public float rotationSpeed = 5f;
    private Vector3 initialDirection;

    [Header("Cibles")]
    public Transform target; // cible principale (ex: base)
    private Transform attackTarget; // cible détectée dans le champ
    private readonly List<Transform> targetsInRange = new();

    [Header("Attaque")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float damage = 10f;
    public float shootCooldown = 2f;

    private float shootTimer = 0f;

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
        Debug.Log("Tir déclenché sur : " + attackTarget.name);

        GameObject projectileGO = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
    
        // Empêche la collision immédiate avec soi-même
        Collider myCollider = GetComponent<Collider>();
        Collider projectileCollider = projectileGO.GetComponent<Collider>();
        if (myCollider != null && projectileCollider != null)
        {
            Physics.IgnoreCollision(projectileCollider, myCollider);
        }

        // Si nécessaire : passe des infos au projectile
        Projectile projectile = projectileGO.GetComponent<Projectile>();
        if (projectile != null)
        {
            projectile.target = attackTarget;
            projectile.damage = damage;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Base"))
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
        if (other.CompareTag("Base"))
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
