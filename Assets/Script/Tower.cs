using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public float health = 10f;

    [Header("Déplacement")]
    private Vector3 initialDirection;

    [Header("Cibles")]
    private Transform attackTarget; // cible détectée dans le champ
    private readonly List<Transform> targetsInRange = new();

    [Header("Attaque")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float damage = 2f;
    public float shootCooldown = 1f;

    private float shootTimer = 1f;

    void Update()
    {
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

    public void TakeDamage(float amount)
    {
        if (health <= 0f)
        {
            Die();
        }
        health -= amount;
    }
    
    private void Die()
    {
        Destroy(gameObject);
    }
    
    protected void Shoot()
    {
        if (attackTarget == null) return;

        Debug.Log("Tir déclenché sur : " + attackTarget.name);

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
        if (other.CompareTag("Enemy"))
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
        if (other.CompareTag("Enemy"))
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
