using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    // Static variable to track if any object is currently being dragged
    private static bool _isDragging;

    // Public variables to control dragging behavior
    public bool isDraggable = true;
    public GameObject allyObject;
    public float slowTimeScale = 0.1f;

    public ShopButton shopButton;

    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main;
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
        if (_isDragging)
        {
            // Cancel drag on right-click
            if (Input.GetMouseButtonDown(1))
            {
                Destroy(gameObject);
                StopDragging();
                if (shopButton != null)
                {
                    shopButton.SetTileActivity(false);
                }
                return;
            }

            // Update object position to follow the mouse
            Vector2 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePosition;

            // Place object on left-click
            if (Input.GetMouseButtonDown(0))
            {
                PlaceObject();
                if (shopButton != null)
                {
                    shopButton.SetTileActivity(false);
                }
            }
        }
    }

    // Method to place the object on a tile
    private void PlaceObject()
    {
        Collider2D hitCollider = Physics2D.OverlapPoint(transform.position);
        if (hitCollider != null)
        {
            Tile tile = hitCollider.GetComponent<Tile>();
            if (tile != null && !tile.hasAlly)
            {
                tile.hasAlly = true;
                tile.spriteRenderer.sprite = null; // Remove the sprite from the tile
                tile.allyObject = gameObject; // Set the ally object reference on the tile

                gameObject.transform.position = tile.transform.position; // Snap the draggable object to the tile
                Destroy(gameObject.GetComponent<Draggable>()); // Remove the draggable component

                //TODO Manage money mechanics

                return;
            }
        }
        Destroy(gameObject); // Destroy if not placed on a valid tile
    }
    private void StartDragging()
    {
        _isDragging = true;
        Time.timeScale = slowTimeScale;
    }
    public void StopDragging()
    {
        _isDragging = false;
        Time.timeScale = 1.0f;
    }
}