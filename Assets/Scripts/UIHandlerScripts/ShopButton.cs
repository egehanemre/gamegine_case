namespace UIHandlerScripts
{
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;
    using GameplayScripts;

    public class ShopButton : MonoBehaviour
    {
        [Header("Ally Settings")]
        public Sprite allySprite;
        public GameObject allyPrefab;
        public int price;
        public TextMeshProUGUI priceText;

        [Header("UI Elements")]
        public GameObject tiles;

        private Image _shopIconSpriteRenderer;
        private GameObject _currentAlly;
        private Camera _mainCamera;
        private void Awake()
        {
            _mainCamera = Camera.main;
            price = allyPrefab.GetComponent<Shooter>().cost;
            _shopIconSpriteRenderer = transform.Find("ShopIconSprite").GetComponent<Image>();
            _shopIconSpriteRenderer.sprite = allySprite;
            priceText.text = price.ToString();
        }

        private void OnValidate()
        {
            if (allySprite && _shopIconSpriteRenderer)
            {
                _shopIconSpriteRenderer.sprite = allySprite;
                priceText.text = price.ToString();
            }
        }

        private void Start()
        {
            InitializeButtons();
        }

        private void InitializeButtons()
        {
            GetComponent<Button>().onClick.AddListener(StartDraggingAlly);
        }
        
        private GameObject InstantiateAlly(Vector3 position)
        {
            return Instantiate(allyPrefab, position, Quaternion.identity);
        }

        public void StartDraggingAlly()
        {
            // Ensure any previous ally is no longer draggable
            if (_currentAlly != null && _currentAlly.GetComponent<Draggable>() != null)
            {
                _currentAlly.GetComponent<Draggable>().StopDragging();
            }

            // Spawn the selected ally prefab at the mouse position
            Vector3 spawnPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            spawnPosition.z = 0;

            _currentAlly = InstantiateAlly(spawnPosition);

            // Show placement tiles
            SetTileActivity(true);

            if (tiles != null)
            {
                foreach (Transform child in tiles.transform)
                {
                    SpriteRenderer childSpriteRenderer = child.GetComponent<SpriteRenderer>();
                    Tile tileComponent = child.GetComponent<Tile>();
                    
                    if (tileComponent == null)
                    {
                        Debug.LogWarning($"No Tile component found on {child.name}");
                        continue;
                    }

                    if (childSpriteRenderer != null && !tileComponent.hasAlly)
                    {
                        childSpriteRenderer.sprite = allySprite;
                    }
                }
            }
            else
            {
                Debug.LogError("Tiles GameObject is not assigned in the Inspector.");
            }

            Draggable draggableComponent = _currentAlly.GetComponent<Draggable>();
            if (draggableComponent == null)
            {
                draggableComponent = _currentAlly.AddComponent<Draggable>();
            }
            draggableComponent.isDraggable = true;
            draggableComponent.shopButton = this;
        }
        public void SetTileActivity(bool show)
        {
            if (tiles != null)
            {
                tiles.SetActive(show);
                if (show)
                {
                    foreach (Transform child in tiles.transform)
                    {
                        Tile tileComponent = child.GetComponent<Tile>();
                        if (tileComponent != null && !tileComponent.hasAlly)
                        {
                            tileComponent.enabled = true;
                        }
                    }
                }
            }
        }
    }
}
