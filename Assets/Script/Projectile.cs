using Script;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Transform target;
    public float speed = 10f;
    public float damage = 10f;
    public float lifetime = 5f; // Durée avant auto-destruction
    public string shooterTag;
    
    void Start()
    {
        Destroy(gameObject, lifetime); // Auto-destruction après quelques secondes
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        // Se dirige vers la cible
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // Optionnel : regarde vers la cible
        transform.LookAt(target);
    }

    void OnTriggerEnter(Collider other)
    {
        // Évite les collisions avec le tireur lui-même ou ses alliés
        if (other.CompareTag(shooterTag))
            return;

        bool hit = false;

        if (other.CompareTag("Base"))
        {
            Base baseObject = other.GetComponent<Base>();
            if (baseObject != null)
            {
                baseObject.TakeDamage(damage);
                hit = true;
            }
        }
        else if (other.CompareTag("Enemy"))
        {
            Enemy enemyObject = other.GetComponent<Enemy>();
            if (enemyObject != null)
            {
                enemyObject.TakeDamage(damage);
                hit = true;
            }
        }
        else if (other.CompareTag("Tower"))
        {
            Tower towerObject = other.GetComponent<Tower>();
            if (towerObject != null)
            {
                towerObject.TakeDamage(damage);
                hit = true;
            }
        }

        if (hit)
        {
            Destroy(gameObject);
        }
    }
}