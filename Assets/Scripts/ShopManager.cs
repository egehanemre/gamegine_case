using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public GameObject[] buttons; // Array of buttons
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
        ShopButton shopButton = buttons[index].GetComponent<ShopButton>();
        if (shopButton != null)
        {
            // Spawn the selected ally prefab at the mouse position which is directly on buttons
            Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            spawnPosition.z = 0; // Ensure the z position is 0

            currentAlly = shopButton.InstantiateAlly(spawnPosition);

            // Add Draggable component if not already present
            if (currentAlly.GetComponent<Draggable>() == null)
            {
                currentAlly.AddComponent<Draggable>();
            }
        }
        else
        {
            Debug.LogError("No ShopButton component found on button index: " + index);
        }
    }
}