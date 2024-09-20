using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OpenBagScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject inventory;
    private bool openBagSwitch;
    void Start()
    {
        openBagSwitch = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("tab")) {
            SetOpenBagSwitch();
        }
    }
    private void SetOpenBagSwitch() {
        if (openBagSwitch) {
            openBagSwitch = false;
        } else {
            openBagSwitch = true;
        }
        OpenBag();
    }
    private void OpenBag() {
        if(openBagSwitch) {
            inventory.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        } else {
            inventory.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
