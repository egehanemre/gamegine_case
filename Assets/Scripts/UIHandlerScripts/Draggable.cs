namespace UIHandlerScripts
{
    using UnityEngine;
    using ManagementScripts;
    using GameplayScripts;

    public class Draggable : MonoBehaviour
    {
        [Header("Draggable Settings")]
        public bool isDraggable = true;
        public GameObject allyObject;
        public float slowTimeScale = 0.1f;
        public ShopButton shopButton;

        private float _currentTimeScale;
        private CoinManager _coinCounter;
        private Camera _mainCamera;
        private static bool _isDragging;

        private void Start()
        {
            _mainCamera = Camera.main;
            _coinCounter = FindObjectOfType<CoinManager>();
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
                if (Input.GetMouseButtonDown(1)) // Right-click to cancel
                {
                    Destroy(gameObject);
                    StopDragging();
                    shopButton?.SetTileActivity(false);
                    return;
                }

                Vector2 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
                transform.position = mousePosition;

                if (Input.GetMouseButtonDown(0)) // Left-click to place
                {
                    if (_coinCounter.coinCount >= shopButton.price)
                    {
                        PlaceObject();
                        shopButton?.SetTileActivity(false);
                    }
                    else
                    {
                        Destroy(gameObject);
                        StopDragging();
                        shopButton?.SetTileActivity(false);
                    }
                }
            }
        }

        private void StartDragging()
        {
            _isDragging = true;
            _currentTimeScale = Time.timeScale;
            Time.timeScale = slowTimeScale;
        }

        public void StopDragging()
        {
            _isDragging = false;
            Time.timeScale = _currentTimeScale;
        }

        private void PlaceObject()
        {
            Collider2D hitCollider = Physics2D.OverlapPoint(transform.position);
            if (hitCollider != null)
            {
                Tile tile = hitCollider.GetComponent<Tile>();
                if (tile != null && !tile.hasAlly)
                {
                    tile.hasAlly = true;
                    tile.spriteRenderer.sprite = null;
                    tile.allyObject = gameObject;

                    _coinCounter.SpendCoins(shopButton.price);

                    Shooter shooter = gameObject.GetComponent<Shooter>();
                    shooter.tile = tile;

                    transform.position = tile.transform.position;
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
    }
}
