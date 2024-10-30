// Bomb.cs
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public int damage;
    public float explosionRadius;
    public LayerMask enemyLayer;
    public GameObject explosionVisualPrefab; // Prefab for the explosion visual

    private Shooter shooter; // Reference to the Shooter component

    private void Start()
    {
        shooter = GetComponent<Shooter>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Explode();
        }
    }

    public void Explode()
    {
        ShowExplosionVisual();

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, explosionRadius, enemyLayer);

        foreach (var enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyManager>().TakeDamage(damage);
        }

        if (shooter != null && shooter.tile != null)
        {
            shooter.tile.ResetTile(); // Free the tile
        }

        Destroy(gameObject);
    }

    private void ShowExplosionVisual()
    {
        GameObject explosionVisual = Instantiate(explosionVisualPrefab, transform.position, Quaternion.identity);
        explosionVisual.transform.localScale = new Vector3(explosionRadius * 2, explosionRadius * 2, 1); // Adjust scale based on radius
        Destroy(explosionVisual, 0.5f); // Destroy visual after 0.5 seconds
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}