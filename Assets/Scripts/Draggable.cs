// Draggable.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    public static bool isDragging = false;
    public float slowTimeScale = 0.1f;

    private void OnEnable()
    {
        StartDragging();
    }

    private void OnMouseDown()
    {
        StartDragging();
    }

    private void OnMouseUp()
    {
        StopDragging();
    }

    private void OnDestroy()
    {
        StopDragging();
    }

    private void Update()
    {
        if (isDragging)
        {
            // Cancel drag
            if (Input.GetMouseButtonDown(1)) // Right-click
            {
                Destroy(gameObject);
                StopDragging();
                return;
            }

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0; // Ensure the z position is 0
            transform.position = mousePosition;
        }
    }

    private void StartDragging()
    {
        isDragging = true;
        Time.timeScale = slowTimeScale;
    }

    private void StopDragging()
    {
        isDragging = false;
        Time.timeScale = 1.0f;
    }
}