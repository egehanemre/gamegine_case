using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour
{
    public Sprite allySprite;
    public GameObject allyPrefab;
    public int price;
    public TextMeshProUGUI priceText;

    private Image _shopIconSpriteRenderer;
    private GameObject _currentAlly;
    
    public GameObject tiles;
    private void Awake()
    {
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
        GetComponent<Button>().onClick.AddListener(() => StartDraggingAlly());
    }

    public void StartDraggingAlly()
    {
        // Ensure any previous ally is no longer draggable
        if (_currentAlly != null && _currentAlly.GetComponent<Draggable>() != null)
        {
            _currentAlly.GetComponent<Draggable>().StopDragging();
        }

        // Spawn the selected ally prefab at the mouse position which is directly on buttons
        Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        spawnPosition.z = 0; // Ensure the z position is 0

        _currentAlly = InstantiateAlly(spawnPosition);
        
        // show placement tiles
        SetTileActivity(true);
        
        if (tiles != null)
        {
            foreach (Transform child in tiles.transform)
            {
                // Get the SpriteRenderer component of the child GameObject
                SpriteRenderer childSpriteRenderer = child.GetComponent<SpriteRenderer>();
        
                // Get the Tile component of the child GameObject
                Tile tileComponent = child.GetComponent<Tile>();
                if (tileComponent == null)
                {
                    Debug.LogWarning($"No Tile component found on {child.name}");
                    continue;
                }

                // If the child has a SpriteRenderer and no ally is present, set its sprite
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
        

        // Add Draggable component if not already present
        Draggable draggableComponent = _currentAlly.GetComponent<Draggable>();
        if (draggableComponent == null)
        {
            draggableComponent = _currentAlly.AddComponent<Draggable>();
        }
        draggableComponent.isDraggable = true; // Ensure the new object is draggable
        draggableComponent.shopButton = this; // Set the shopButton reference
    }

    public GameObject InstantiateAlly(Vector3 position)
    {
        return Instantiate(allyPrefab, position, Quaternion.identity);
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