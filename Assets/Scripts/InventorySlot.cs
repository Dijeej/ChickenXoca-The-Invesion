using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public UnityEngine.UI.Image image;
    public Color SelectedColor, notSelectedColor;

    private void Awake() {
        Deselect();
    }
    public void Select() {
        image.color = SelectedColor;
    }
    public void Deselect() {
        image.color = notSelectedColor;
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0) {
            InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
            inventoryItem.parentAfterDrag = transform;
        }
    }
}
