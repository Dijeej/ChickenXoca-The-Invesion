using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManagerScript : MonoBehaviour
{
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;

    int selectedSlot = -1;

    private void Start() {
        ChangeSelectedSlot(0);
    }

    private void Update() {
        if (Input.inputString != null) {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if (isNumber && number > 0 && number < 7) {
                ChangeSelectedSlot(number - 1);
            }
        }
    }

    void ChangeSelectedSlot(int newValue) {
        if (selectedSlot >= 0)
            inventorySlots[selectedSlot].Deselect();

        inventorySlots[newValue].Select();
        selectedSlot = newValue;
        Debug.Log("Trocou para: " + newValue);
    }

   public bool AddNewItem(ItemScript item) {
    for (int i = 0; i < inventorySlots.Length; i++) {
        InventorySlot inventorySlot = inventorySlots[i];
        InventoryItem inventoryItem = inventorySlot.GetComponentInChildren<InventoryItem>();
        if (inventoryItem == null) {
            SpawnNewItem(item, inventorySlot);
            return true;
        }
    }
    return false; // Inventário cheio
}

    void SpawnNewItem(ItemScript item, InventorySlot inventorySlot) {
        GameObject newItemGO = Instantiate(inventoryItemPrefab, inventorySlot.transform);
        InventoryItem inventoryItem = newItemGO.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
    }
}
