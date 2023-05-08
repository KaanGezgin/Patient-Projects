using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class TeslaCoilUIManager : MonoBehaviour
{
    [Header("References")]
    public EnergyStation energyStation;

    [Header("UI")]
    [SerializeField] private Button addFuel;
    [SerializeField] private TMP_Text currentEnergy;

    private float fuelUpRate;
    private float currentFuel;

    private void Start()
    {
        energyStation = GameObject.FindGameObjectWithTag("Tesla Coil").GetComponent<EnergyStation>();
        fuelUpRate = 20000.0f;
    }
    private void Update()
    {
        currentFuel = energyStation.currentFuel;
    }
    private void FixedUpdate()
    {
        currentEnergy.text = currentFuel.ToString();
    }
    public void AddFuel()
    {
        currentFuel += fuelUpRate;
        energyStation.currentFuel = currentFuel;
        //currentEnergy.text = currentFuel.ToString();
    }

} 
