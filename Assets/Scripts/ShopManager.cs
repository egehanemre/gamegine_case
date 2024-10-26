// ShopManager.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public GameObject[] buttons; // Array of buttons
    public GameObject[] allyPrefabs; // Array of ally unit prefabs
    private GameObject currentAlly;

    private void Start()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i; // Capture the current index
            buttons[i].GetComponent<Button>().onClick.AddListener(() => StartDraggingAlly(index));
        }
    }

    private void StartDraggingAlly(int index)
    {
        if (index < allyPrefabs.Length)
        {
            // Spawn the selected ally prefab at the mouse position which is directly on buttons
            Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            spawnPosition.z = 0; // Ensure the z position is 0
            
            currentAlly = Instantiate(allyPrefabs[index], spawnPosition, Quaternion.identity);
            
            // Add Draggable component if not already present
            if (currentAlly.GetComponent<Draggable>() == null)
            {
                currentAlly.AddComponent<Draggable>();
            }
        }
        else
        {
            Debug.LogError("No corresponding ally prefab for button index: " + index);
        }
    }
}