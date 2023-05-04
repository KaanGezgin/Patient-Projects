using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visor : MonoBehaviour
{
    public GameObject visor;
    RaycastHit visorRay;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Physics.Raycast(visor.transform.position, visor.transform.TransformDirection(Vector3.forward), out visorRay))
            {
                
                if(visorRay.collider.name == "HealthStation")
                {
                    Debug.Log("HealthStation");
                }
                
                if(visorRay.collider.name == "TeslaCoil")
                {
                        Debug.Log("coil");
                }
                
                if (visorRay.collider.name == "Upgrade and Crafting Bench")
                {
                    Debug.Log("upraf");
                }

            }
        }
    }
}

