namespace GameplayScripts
{
    using UnityEngine;
    using ManagementScripts;

    public class Bullet : MonoBehaviour
    {
        public int damage;
        public float speed;
        public float range;
        public bool isHelicopterBullet;

        private Transform _target;
        private Vector3 _lastDirection;
        private Vector3 _lastTargetPosition;

        public bool moveInXAxisOnly; 
        
        // Area damage parameters
        public float areaDamageRadius; 
        public int areaDamageAmount; 
        
        public GameObject explosionVisualPrefab; 

        private void Update()
        {
            BulletLogic();
        }

        public void SetTarget(Transform target)
        {
            _target = target;
            if (_target != null)
            {
                _lastTargetPosition = _target.position; // Update last known position when target is set
            }
        }

        private void BulletLogic()
        {
            if (isHelicopterBullet)
            {
                if (_target != null)
                {
                    Vector3 direction = (_target.position - transform.position).normalized;

                    transform.position += direction * (speed * Time.deltaTime);

                    // Check if the bullet has reached the target
                    if (Vector3.Distance(transform.position, _target.position) < 0.1f)
                    {
                        DealAreaDamage(); 
                        ShowExplosionVisual(); 
                        Destroy(gameObject);
                    }
                }
                else
                {
                    // Move towards the last known position
                    Vector3 direction = (_lastTargetPosition - transform.position).normalized;
                    transform.position += direction * (speed * Time.deltaTime);

                    // Check if the bullet has reached the last known position
                    if (Vector3.Distance(transform.position, _lastTargetPosition) < 0.1f)
                    {
                        DealAreaDamage(); // Apply area damage when reaching the last known position
                        ShowExplosionVisual(); // Show explosion visual
                        Destroy(gameObject);
                    }
                }
            }
            else
            {
                // Old logic for non-helicopter bullets
                if (_target != null)
                {
                    Vector3 direction = (_target.position - transform.position).normalized;
                    _lastDirection = direction;

                    if (moveInXAxisOnly)
                    {
                        direction.y = 0; // Only move in x-axis
                    }

                    transform.position += direction * (speed * Time.deltaTime);

                    // Check if the bullet has reached the target
                    if (Vector3.Distance(transform.position, _target.position) < 0.1f)
                    {
                        _target.GetComponent<EnemyManager>().TakeDamage(damage);
                        Destroy(gameObject);
                    }
                }
                else
                {
                    // Continue moving in the last direction if no target is found
                    if (moveInXAxisOnly)
                    {
                        _lastDirection.y = 0; // Only move in x-axis
                    }

                    transform.position += _lastDirection * (speed * Time.deltaTime);
                    Destroy(gameObject, 2f); // Destroy the bullet after 2 seconds if no target is set
                }
            }

            // Optional: Destroy if the bullet goes out of bounds
            if (transform.position == Vector3.zero)
            {
                Destroy(gameObject);
            }

            // Destroy bullet after 5 seconds
            Destroy(gameObject, 5f);
        }

        private void DealAreaDamage()
        {
            // Get all colliders within the area damage radius
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, areaDamageRadius);

            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    collider.GetComponent<EnemyManager>().TakeDamage(areaDamageAmount);
                }
            }
        }

        private void ShowExplosionVisual()
        {
            if (explosionVisualPrefab != null)
            {
                // Instantiate the explosion visual prefab at the bullet's position
                GameObject explosionVisual = Instantiate(explosionVisualPrefab, transform.position, Quaternion.identity);
                Destroy(explosionVisual, 1f); // Destroy the visual after 1 second to clean up
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
            {
                other.GetComponent<EnemyManager>().TakeDamage(damage);
                ShowExplosionVisual(); // Show explosion visual on direct hit
                Destroy(gameObject);
            }
        }

        // Visualization of the area damage radius in the Scene view
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red; // Set the color for the gizmo
            Gizmos.DrawWireSphere(transform.position, areaDamageRadius); // Draw the wire sphere at the bullet position
        }
    }
}
