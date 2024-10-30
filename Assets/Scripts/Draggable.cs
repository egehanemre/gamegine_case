using UnityEngine;

public class Draggable : MonoBehaviour
{
    public bool isDraggable = true;
    public GameObject allyObject;
    public float slowTimeScale = 0.1f;
    public ShopButton shopButton;
    private float _currentTimeScale;
    
    private CoinCounter _coinCounter;
    private Camera _mainCamera;

    private static bool _isDragging;

    private void Start()
    {
        _mainCamera = Camera.main;
        _coinCounter = FindObjectOfType<CoinCounter>();
    }

    private void OnEnable()
    {
        if (isDraggable)
        {
            StartDragging();
        }
    }

    private void OnMouseDown()
    {
        if (!_isDragging && isDraggable)
        {
            StartDragging();
        }
    }

    private void OnMouseUp()
    {
        if (_isDragging)
        {
            StopDragging();
        }
    }

    private void OnDestroy()
    {
        StopDragging();
    }

    private void Update()
    {
        ManageInput();
    }

    private void ManageInput()
    {
        if (_isDragging)
        {
            if (Input.GetMouseButtonDown(1)) // Right click
            {
                Destroy(gameObject);
                StopDragging();
                shopButton?.SetTileActivity(false);
                return;
            }

            Vector2 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePosition;

            if (Input.GetMouseButtonDown(0)) // Left click
            {
                if (_coinCounter.coinCount >= shopButton.price)
                {
                    PlaceObject(); 
                    shopButton?.SetTileActivity(false); // Disable the tile
                }
                else
                {
                    Destroy(gameObject);
                    StopDragging();
                    shopButton?.SetTileActivity(false); // Disable the tile 
                }
            }
        }
    }

    public void StopDragging()
    {
        _isDragging = false;
        Time.timeScale = _currentTimeScale; // Restore the time scale
    }

    private void PlaceObject()
    {
        Collider2D hitCollider = Physics2D.OverlapPoint(transform.position);
        if (hitCollider != null)
        {
            Tile tile = hitCollider.GetComponent<Tile>();
            if (tile != null && !tile.hasAlly)
            {
                // Place the object on the tile
                tile.hasAlly = true;
                tile.spriteRenderer.sprite = null;
                tile.allyObject = gameObject;
                
                _coinCounter.SpendCoins(shopButton.price); // Spend coins

                Shooter shooter = gameObject.GetComponent<Shooter>();
                shooter.tile = tile;

                gameObject.transform.position = tile.transform.position;
                Destroy(gameObject.GetComponent<Draggable>());

                BoxCollider2D boxCollider = gameObject.GetComponent<BoxCollider2D>();
                if (boxCollider != null)
                {
                    boxCollider.enabled = true;
                }
                return;
            }
        }
        Destroy(gameObject);
    }

    private void StartDragging()
    {
        _isDragging = true;
        _currentTimeScale = Time.timeScale;
        Time.timeScale = slowTimeScale; // Slow down the time while dragging
    }
}