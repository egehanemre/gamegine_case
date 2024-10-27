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

    private Image shopIconSpriteRenderer;
    private GameObject currentAlly;
    private void Awake()
    {
        shopIconSpriteRenderer = transform.Find("ShopIconSprite").GetComponent<Image>();
        shopIconSpriteRenderer.sprite = allySprite;
        priceText.text = price.ToString();
    }

    private void OnValidate()
    {
        if (allySprite && shopIconSpriteRenderer)
        {
            shopIconSpriteRenderer.sprite = allySprite;
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
        if (currentAlly != null && currentAlly.GetComponent<Draggable>() != null)
        {
            currentAlly.GetComponent<Draggable>().StopDragging();
        }

        // Spawn the selected ally prefab at the mouse position which is directly on buttons
        Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        spawnPosition.z = 0; // Ensure the z position is 0

        currentAlly = InstantiateAlly(spawnPosition);

        // Add Draggable component if not already present
        Draggable draggableComponent = currentAlly.GetComponent<Draggable>();
        if (draggableComponent == null)
        {
            draggableComponent = currentAlly.AddComponent<Draggable>();
        }
        draggableComponent.isDraggable = true; // Ensure the new object is draggable
    }

    public GameObject InstantiateAlly(Vector3 position)
    {
        return Instantiate(allyPrefab, position, Quaternion.identity);
    }
}