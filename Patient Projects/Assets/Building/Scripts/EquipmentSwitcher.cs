using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSwitcher : MonoBehaviour
{
    public int selectedEquipment = 0;
    private int previousSelectedWeapon;

    void Start()
    {
        EquipmentSwitch();
    }

    void Update()
    {
        previousSelectedWeapon = selectedEquipment;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (selectedEquipment >= transform.childCount - 1)
            {
                selectedEquipment = 0;
            }
            else
            {
                selectedEquipment++;
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (selectedEquipment <= 0)
            {
                selectedEquipment = transform.childCount - 1;
            }
            else
            {
                selectedEquipment--;
            }
        }
        if (previousSelectedWeapon != selectedEquipment)
        {
            EquipmentSwitch();
        }
    }

    void EquipmentSwitch()
    {
        int counter = 0;
        foreach (Transform equipment in transform)
        {
            if (counter == selectedEquipment)
            {
                equipment.gameObject.SetActive(true);
            }
            else
            {
                equipment.gameObject.SetActive(false);
            }
            counter++;
        }
    }
}
