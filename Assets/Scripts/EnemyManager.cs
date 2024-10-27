using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public float speed;

    private void Start()
    {
        speed = Random.Range(1.0f, 3.0f);
    }

    void Update()
    {
        transform.Translate(Vector2.right * (speed * Time.deltaTime));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        BaseManager baseManager = collision.GetComponent<BaseManager>();
        if (baseManager != null)
        {
            baseManager.healthManager.TakeDamage(10);
            Destroy(gameObject);
            CoinCounter.Instance.GetCoins(10);
        }
    }
}