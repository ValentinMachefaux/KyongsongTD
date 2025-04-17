using UnityEngine;

public abstract class UnitBase : MonoBehaviour
{
    public float health = 100f;
    public float damage = 10f;
    public float range = 5f;
    public GameObject projectilePrefab;
    public Transform firePoint;

    public virtual void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
            Die();
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    protected void Shoot(Transform target)
    {
        if (target == null) return;

        GameObject projGO = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Projectile projectile = projGO.GetComponent<Projectile>();
        if (projectile != null)
        {
            projectile.target = target;
            projectile.damage = damage;
        }
    }

    protected Transform FindNearestTarget(string targetTag)
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);
        Transform nearest = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject t in targets)
        {
            float distance = Vector3.Distance(transform.position, t.transform.position);
            if (distance < shortestDistance && distance <= range)
            {
                shortestDistance = distance;
                nearest = t.transform;
            }
        }

        return nearest;
    }
}
