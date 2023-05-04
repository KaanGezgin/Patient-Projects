using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UprafBench : MonoBehaviour
{
    public Animator startSide1, startSide2;
    public ParticleSystem computerSoul;
    private void Start()
    {
        startSide1.enabled = false;
        startSide2.enabled = false;
        computerSoul.Stop();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Tesla Coil Body")
        {
            startSide1.enabled = true;
            startSide2.enabled = true;
            computerSoul.Play();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Tesla Coil Body")
        {
            startSide1.enabled = false;
            startSide2.enabled = false;
            computerSoul.Stop();
        }
    }
}
