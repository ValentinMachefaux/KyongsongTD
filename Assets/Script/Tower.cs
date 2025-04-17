public class Tower : UnitBase
{
    public int cost = 50;
    public float fireRate = 1f;
    private float fireCooldown = 0f;

    if (Input.GetMouseButtonDown(0)) // ou touch pour mobile
    {
        Vector3 mousePos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Vérifie si le sol est un emplacement valide
            if (hit.collider.CompareTag("BuildZone")) // Tag de l'herbe par exemple
            {
                Tower towerComponent = towerPrefab.GetComponent<Tower>();
                int towerCost = towerComponent.cost;
                if (GameManager.Instance.playerMoney >= towerCost)
                {
                    GameManager.Instance.playerMoney -= towerCost;
                    Instantiate(towerPrefab, hit.point, Quaternion.identity);
                    Debug.Log("Tour placée !");
                }
                else
                {
                    Debug.Log("Pas assez d'argent !");
                }
            }
        }
    }

    void Update()
    {
        fireCooldown -= Time.deltaTime;
        if (fireCooldown <= 0f)
        {
            Transform target = FindNearestTarget("Enemy");
            if (target != null)
            {
                Shoot(target);
                fireCooldown = 1f / fireRate;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

}
