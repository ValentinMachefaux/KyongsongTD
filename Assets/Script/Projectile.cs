using Script;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Transform target;
    public float speed = 10f;
    public float damage = 1f;
    public float lifetime = 5f; // Durée avant auto-destruction

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
        if (other.CompareTag("Base"))
        {
            Base baseObject = other.GetComponent<Base>();
            if (baseObject != null)
            {
                baseObject.TakeDamage(damage);  // Infliger des dégâts à la base
            }

            Destroy(gameObject);  // Détruire le projectile après avoir infligé des dégâts à la base
        }
    }
}