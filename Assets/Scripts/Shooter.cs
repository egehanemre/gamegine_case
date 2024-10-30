// Shooter.cs
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Shooter : MonoBehaviour
{
    public bool isBomb;
    public enum AllyType
    {
        Soldier,
        Helicopter,
        Tank,
        Bomb
    }

    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;

    public float cooldown;
    public float range;

    public int health;
    public int cost;

    public Tile tile;
    public LayerMask shootMask;
    public AllyType allyType;

    private bool _canShoot = true;
    private GameObject _target;

    private Button _sellButton;

    private void Update()
    {
        if (isBomb)
        {
            return;
        }

        switch (allyType)
        {
            case AllyType.Soldier:
                DetectAndShootSoldier();
                break;
            case AllyType.Helicopter:
                DetectAndShootHelicopter();
                break;
            case AllyType.Tank:
                DetectAndShootTank();
                break;
        }
    }

    private void Start()
    {
        if (isBomb)
        {
            return;
        }

        _sellButton = GetComponentInChildren<Button>(true);
        _sellButton.onClick.AddListener(OnSellButtonClick);
        _sellButton.gameObject.SetActive(false);
    }

    public void TakeDamage(int damage)
    {
        if (isBomb)
        {
            return;
        }

        health -= damage;
        if (health <= 0)
        {
            tile.ResetTile();
            Destroy(gameObject);
        }
    }

    public void RefundAndRemoveAlly()
    {
        if (isBomb)
        {
            return;
        }

        CoinCounter.Instance.GetCoins(cost / 2);
        tile.ResetTile();
        RemoveAlly();
    }

    private void OnMouseEnter()
    {
        if (isBomb)
        {
            return;
        }

        _sellButton.gameObject.SetActive(true);
    }

    private void OnMouseExit()
    {
        if (isBomb)
        {
            return;
        }

        _sellButton.gameObject.SetActive(false);
    }

    private void OnSellButtonClick()
    {
        if (isBomb)
        {
            return;
        }

        RemoveAlly();
    }

    public void RemoveAlly()
    {
        if (isBomb)
        {
            return;
        }

        tile.ResetTile();
        Destroy(gameObject);
    }

    private void DetectAndShoot(Vector2 forwardDirection, Vector2 backwardDirection)
    {
        if (isBomb)
        {
            return;
        }

        RaycastHit2D forwardHit = Physics2D.Raycast(transform.position, forwardDirection, range, shootMask);
        RaycastHit2D backwardHit = Physics2D.Raycast(transform.position, backwardDirection, 0.5f, shootMask);

        if (forwardHit.collider != null && forwardHit.collider.CompareTag("Enemy"))
        {
            if (_canShoot)
            {
                _target = forwardHit.collider.gameObject;
                StartCoroutine(FireBullet());
            }
        }
        else if (backwardHit.collider != null && backwardHit.collider.CompareTag("Enemy"))
        {
            if (_canShoot)
            {
                _target = backwardHit.collider.gameObject;
                StartCoroutine(FireBullet());
            }
        }
    }

    private void DetectAndShootSoldier()
    {
        if (isBomb)
        {
            return;
        }

        if (GetComponent<Draggable>() != null)
        {
            return;
        }

        DetectAndShoot(Vector2.left, Vector2.right);
    }

    private void DetectAndShootTank()
    {
        if (isBomb)
        {
            return;
        }

        if (GetComponent<Draggable>() != null)
        {
            return;
        }

        DetectAndShoot(Vector2.left, Vector2.right);
    }

    private void DetectAndShootHelicopter()
    {
        if (isBomb)
        {
            return;
        }

        if (GetComponent<Draggable>() != null)
        {
            return;
        }

        Vector2 pivotPosition = bulletSpawnPoint.position;
        Vector2 leftDirection = Vector2.left;
        float detectionRange = range;

        RaycastHit2D[] hits = Physics2D.RaycastAll(pivotPosition, leftDirection, detectionRange, shootMask);
        float closestDistance = Mathf.Infinity;
        GameObject closestEnemy = null;

        foreach (var hit in hits)
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                float distance = Vector2.Distance(pivotPosition, hit.collider.transform.position);
                if (distance < closestDistance && hit.collider.transform.position.x < pivotPosition.x - 1)
                {
                    closestDistance = distance;
                    closestEnemy = hit.collider.gameObject;
                }
            }
        }

        if (closestEnemy != null && _canShoot)
        {
            _target = closestEnemy;
            StartCoroutine(FireBullet());
        }
    }

    private IEnumerator FireBullet()
    {
        if (isBomb)
        {
            yield break;
        }

        _canShoot = false;

        if (allyType == AllyType.Helicopter)
        {
            FireHelicopterBullets();
        }
        else if (allyType == AllyType.Soldier)
        {
            FireSoldierBullets();
        }
        else
        {
            FireTankBullet();
        }

        yield return new WaitForSeconds(cooldown);
        _canShoot = true;
    }

    private void FireHelicopterBullets()
    {
        Vector3 pivotPosition = bulletSpawnPoint.position;
        Vector3 targetOffset1 = new Vector3(0, 2, 0); // Target position pivot.y + 2
        Vector3 targetOffset2 = new Vector3(0, -2, 0); // Target position pivot.y - 2

        GameObject bullet1 = Instantiate(bulletPrefab, pivotPosition, Quaternion.identity);
        Bullet bulletScript1 = bullet1.GetComponent<Bullet>();
        bulletScript1.SetTarget(_target.transform);
        bulletScript1.speed = 15f;
        bulletScript1.moveInXAxisOnly = false; // Allow movement in both axes
        bulletScript1.isHelicopterBullet = true; // Mark as helicopter bullet

        GameObject bullet2 = Instantiate(bulletPrefab, pivotPosition, Quaternion.identity);
        Bullet bulletScript2 = bullet2.GetComponent<Bullet>();
        bulletScript2.SetTarget(_target.transform);
        bulletScript2.speed = 15f;
        bulletScript2.moveInXAxisOnly = false; // Allow movement in both axes
        bulletScript2.isHelicopterBullet = true; // Mark as helicopter bullet

        // Apply initial force to create an arc trajectory towards the target positions
        Rigidbody2D rb1 = bullet1.GetComponent<Rigidbody2D>();
        Rigidbody2D rb2 = bullet2.GetComponent<Rigidbody2D>();

        Vector2 force1 = (pivotPosition + targetOffset1 - pivotPosition).normalized * 5f; // Adjust the force as needed
        Vector2 force2 = (pivotPosition + targetOffset2 - pivotPosition).normalized * 5f; // Adjust the force as needed

        rb1.AddForce(force1, ForceMode2D.Impulse);
        rb2.AddForce(force2, ForceMode2D.Impulse);
    }

    private void FireSoldierBullets()
    {
        Vector3[] offsets = { Vector3.zero, new Vector3(0.5f, 0, 0), new Vector3(-0.5f, 0, 0) };
        foreach (var offset in offsets)
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position + offset, Quaternion.identity);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.SetTarget(_target.transform);
            bulletScript.speed = 10f;
            bulletScript.moveInXAxisOnly = true;
        }
    }

    private void FireTankBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.SetTarget(_target.transform);
        bulletScript.speed = 10f;
        bulletScript.moveInXAxisOnly = true;
    }
}