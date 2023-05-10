using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visor : MonoBehaviour
{
    public BuildingsUIManager uiManager;
    public GameObject visor;
    RaycastHit visorRay;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            uiManager.CloseAllUIs();
            if (Physics.Raycast(visor.transform.position, visor.transform.TransformDirection(Vector3.forward), out visorRay))
            {
                
                if(visorRay.collider.name == "HealthStation")
                {
                    Debug.Log("ee");
                    uiManager.OpenBeconUI();
                }
                
                if(visorRay.collider.name == "Energy station")
                {
                    uiManager.OpenTeslaCoilUI();
                }
                
                if (visorRay.collider.name == "Upgrade and Crafting Bench")
                {
                    uiManager.OpenUprafUI();
                }

            }
        }
    }
}

