// using UnityEngine;
// public class Projectile : MonoBehaviour
// {
//     public float speed = 10f;
//     public float damage = 25f;
//     public GameObject target;
//
//     void Update()
//     {
//         if (target == null) { Destroy(gameObject); return; }
//
//         Vector3 dir = (target.transform.position - transform.position).normalized;
//         transform.Translate(dir * speed * Time.deltaTime, Space.World);
//
//         if (Vector3.Distance(transform.position, target.transform.position) < 0.2f)
//         {
//             target.GetComponent<Enemy>().TakeDamage(damage);
//             Destroy(gameObject);
//         }
//     }
// }
