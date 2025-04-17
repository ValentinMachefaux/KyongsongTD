public class Enemy : UnitBase
{
    public float moveSpeed = 2f;
    private Transform[] pathPoints;
    private int currentPoint = 0;
    public int reward = 10;

    void Start()
    {
        // Assume path points set externally or via a manager
        pathPoints = WaypointManager.Instance.GetPath();
    }

    void Update()
    {
        MoveAlongPath();

        // Exemple : tirer sur une tour s’il y en a une à portée
        Transform target = FindNearestTarget("Tower");
        if (target != null)
        {
            Shoot(target);
        }
    }

    void MoveAlongPath()
    {
        if (currentPoint >= pathPoints.Length) return;

        Transform targetPoint = pathPoints[currentPoint];
        Vector3 dir = (targetPoint.position - transform.position).normalized;
        transform.Translate(dir * moveSpeed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            currentPoint++;
        }
    }

    protected override void Die()
{
    GameManager.Instance.AddMoney(reward); // Exemple : 10 par ennemi
    base.Die(); // appelle le Die() de UnitBase → détruit l’objet
}

}
