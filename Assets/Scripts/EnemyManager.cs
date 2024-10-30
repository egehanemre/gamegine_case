using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour
{
    public float speed;
    public int enemyHealth;
    public int damageOverTime = 10; // Damage dealt to ally per second
    public float damageInterval = 0.3f; // Time between each damage dealt to ally
    public int coinOnDeath;

    private bool _isAttacking;
    private GameObject _currentAlly;

    // Event to notify when the enemy is defeated
    public event System.Action OnEnemyDefeated;

    private void Start()
    {
        speed = Random.Range(1.0f, 3.0f);
    }

    void Update()
    {
        if (!_isAttacking)
        {
            transform.Translate(Vector2.right * (speed * Time.deltaTime));
        }
        if(enemyHealth <= 0)
        {
            Destroy(gameObject);
            CoinCounter.Instance.GetCoins(coinOnDeath);
            OnEnemyDefeated?.Invoke(); // Trigger the event
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ignore collisions with other enemies
        if (collision.CompareTag("Enemy"))
        {
            return;
        }

        if (collision.CompareTag("Ally"))
        {
            _currentAlly = collision.gameObject;
            StartCoroutine(DamageAllyOverTime(_currentAlly)); // Start dealing damage to ally
        }

        BaseManager baseManager = collision.GetComponent<BaseManager>();
        if (baseManager != null)
        {
            baseManager.healthManager.TakeDamage(damageOverTime);
            Destroy(gameObject);
            OnEnemyDefeated?.Invoke(); // Trigger the event
        }
    }

    private IEnumerator DamageAllyOverTime(GameObject ally)
    {
        _isAttacking = true;
        Shooter shootScript = ally.GetComponent<Shooter>();
        while (ally != null && enemyHealth > 0 && shootScript != null && shootScript.health > 0)
        {
            shootScript.TakeDamage(damageOverTime);
            yield return new WaitForSeconds(damageInterval); // Damage dealt every 0.3 seconds
        }
        _isAttacking = false;
    }

    public void TakeDamage(int dmg)
    {
        if (enemyHealth <= 0)
        {
            Destroy(gameObject);
            CoinCounter.Instance.GetCoins(10);
            OnEnemyDefeated?.Invoke(); // Trigger the event
        }
        else
        {
            enemyHealth -= dmg;
        }
    }
}