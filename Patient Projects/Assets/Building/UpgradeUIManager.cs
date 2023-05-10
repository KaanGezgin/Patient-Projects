using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UpgradeUIManager : MonoBehaviour
{
    //[Header("Referances")]

    [Header("Common Variables")]
    [SerializeField] private Image itemImage;

    [Header("Upgrade Costs")]
    [SerializeField] private float weaponUpgradeCosts;
    [SerializeField] private float miningUpgradeCosts;

    [Header("Weapon Upgrade Ratios")]
    [SerializeField] private float weaponFireRateUpgradeRatio;
    [SerializeField] private float weaponFirePowerUpgradeRatio;
    [SerializeField] private float weaponRecoilUpgradeRatio;
    private float oldFireRate;
    private float oldFirePower;
    private float oldRecoil;

    [Header("Collector Upgrade Ratios")]
    [SerializeField] private float miningRateUpgradeRatio;
    [SerializeField] private float miningSpeedUpgradeRatio;
    [SerializeField] private float miningefficiencyUpgradeRatio;
    private float oldminingRate;
    private float oldminingSpeed;
    private float oldminingefficiency;

    [Header("Text variables")]
    [SerializeField] private TMP_Text firstRatio;
    [SerializeField] private TMP_Text secondRatio;
    [SerializeField] private TMP_Text thirdRatio;
    [SerializeField] private TMP_Text costsText;

    [Header("UI")]
    [SerializeField] private GameObject upgradePanel;

    private void Start()
    {
        upgradePanel.SetActive(false);
    }
    public void WeaponUpgrade()
    {
        //If condition control for wanted item check in inventory
        upgradePanel.SetActive(true);
        //Take references of weapon in inventory and set old values
        //Set new values and send to texts
        firstRatio.text = oldFireRate + " -> ";
        secondRatio.text = oldFirePower + " -> ";
        thirdRatio.text = oldRecoil + " -> ";
        //If condition for enough resources
    }

    public void MiningUpgrade()
    {
        upgradePanel.SetActive(true);
        firstRatio.text = oldminingRate + " -> ";
        secondRatio.text = oldminingSpeed + " -> ";
        thirdRatio.text = oldminingefficiency + " -> ";
    }

    public void WhenClosing()
    {
        //After closing set null every changes
        upgradePanel.SetActive(false);
        firstRatio.text = "";
        secondRatio.text = "";
        thirdRatio.text = "";
    }
}
