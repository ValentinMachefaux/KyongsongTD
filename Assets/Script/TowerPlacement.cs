// using UnityEngine;
//
// public class TowerPlacer : MonoBehaviour
// {
//     public GameObject towerPrefab;
//
//     void Update()
//     {
//         if (Input.GetMouseButtonDown(0)) // ou touch pour mobile
//         {
//             Vector3 mousePos = Input.mousePosition;
//             Ray ray = Camera.main.ScreenPointToRay(mousePos);
//             RaycastHit hit;
//
//             if (Physics.Raycast(ray, out hit))
//             {
//                 if (hit.collider.CompareTag("BuildZone"))
//                 {
//                     Tower towerComponent = towerPrefab.GetComponent<Tower>();
//                     int towerCost = towerComponent.cost;
//
//                     if (GameManager.Instance.playerMoney >= towerCost)
//                     {
//                         GameManager.Instance.playerMoney -= towerCost;
//                         Instantiate(towerPrefab, hit.point, Quaternion.identity);
//                         Debug.Log("Tour placée !");
//                     }
//                     else
//                     {
//                         Debug.Log("Pas assez d'argent !");
//                     }
//                 }
//             }
//         }
//     }
// }
