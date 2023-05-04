using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthStation : MonoBehaviour
{
    public Animator healthPlus;
    private void Start()
    {
        healthPlus.enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Tesla Coil Body")
        {
            healthPlus.enabled = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Tesla Coil Body")
        {
            healthPlus.enabled = false;
        }
    }
}
