// Draggable.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    private bool isDragging = false;

    private void OnEnable()
    {
        isDragging = true;
    }

    private void OnMouseDown()
    {
        isDragging = true;
    }

    private void OnMouseUp()
    {
        isDragging = false;
    }

    private void Update()
    {
        if (isDragging)
        {
            //cancel drag
            if (Input.GetMouseButtonDown(1)) // Right-click
            {
                Destroy(gameObject);
                isDragging = false;
                return;
            }

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0; // Ensure the z position is 0
            transform.position = mousePosition;
        }
    }
}