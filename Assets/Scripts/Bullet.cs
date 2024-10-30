// Bullet.cs
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    public float speed;
    public float range; // Add range for searching new targets
    public bool isHelicopterBullet; // Add a flag to identify helicopter bullets

    private Transform _target; // Target enemy
    private Vector3 _lastDirection; // Last direction of the bullet

    public bool moveInXAxisOnly; // Move the bullet only in x-axis

    private void Update()
    {
        BulletLogic();
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    private void BulletLogic()
    {
        if (_target != null)
        {
            Vector3 direction = (_target.position - transform.position).normalized;
            _lastDirection = direction;

            if (moveInXAxisOnly)
            {
                direction.y = 0; // Only move in x-axis
            }

            transform.position += direction * (speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, _target.position) < 0.1f)
            {
                _target.GetComponent<EnemyManager>().TakeDamage(damage);
                Destroy(gameObject);
            }
        }
        else
        {
            // Search for a new target if the current target is destroyed
            Transform newTarget = FindNewTarget();
            if (newTarget != null)
            {
                SetTarget(newTarget);
            }

            // Continue moving in the last direction if no new target is found
            if (moveInXAxisOnly)
            {
                _lastDirection.y = 0; // Only move in x-axis
            }

            transform.position += _lastDirection * (speed * Time.deltaTime);
            Destroy(gameObject, 2f); // Destroy the bullet after 2 seconds if no target is set
        }
    }

    private Transform FindNewTarget()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, range);
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                float distance = Vector2.Distance(transform.position, hitCollider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = hitCollider.transform;
                }
            }
        }

        return closestEnemy;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyManager>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}