using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform target;
    public float speed = 2f;
    public float rotationSpeed = 5f;

    void Update()
    {
        if (target == null) return;

        MoveTowardsTarget(target);
        RotateTowardsTarget(target);
    }

    // Initialise la cible à suivre
    public void Initialize(GameObject targetObject)
    {
        if (targetObject != null)
            target = targetObject.transform;
    }

    protected void MoveTowardsTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }

    protected void RotateTowardsTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0;
        if (direction == Vector3.zero) return;

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
    }
}