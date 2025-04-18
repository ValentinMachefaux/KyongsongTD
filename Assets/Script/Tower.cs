// using UnityEngine;
//
// public class Tower : UnitBase
// {
//     public int cost = 50;
//     public float fireRate = 1f;
//     private float fireCooldown = 0f;
//
//     void Update()
//     {
//         fireCooldown -= Time.deltaTime;
//
//         if (fireCooldown <= 0f)
//         {
//             Transform target = FindNearestTarget("Enemy");
//             if (target != null)
//             {
//                 Shoot(target);
//                 fireCooldown = 1f / fireRate;
//             }
//         }
//     }
//
//     void OnDrawGizmosSelected()
//     {
//         Gizmos.color = Color.red;
//         Gizmos.DrawWireSphere(transform.position, range);
//     }
// }
