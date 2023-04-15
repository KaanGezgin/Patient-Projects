using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
public class BuildPanelUI : MonoBehaviour
{
    public SidePanel sidePanel;
    public static UnityAction<BuildingsData> ChosenBuilding;
    public BuildingsData[] buildings;
    public BuildingPartUI buildingButton;
    public GameObject buildingVariantPanel;
    public void OnClick(BuildingsData chosenBuildingsData)
    {
        ChosenBuilding?.Invoke(chosenBuildingsData);
        sidePanel.UpdateSideDisplay(chosenBuildingsData);
    }
    public void CreateButtons(BuildingsType chosenBuilding)
    {

    }
    public void ClearButtons()
    {

    }
    /*
    public void OnClickBuildingsButton(){}
    */
    public void OnClickBaseComputerButton()
    {
        CreateButtons(BuildingsType.BaseComputer);
    }
    public void OnClickTeslaCoilButton()
    {
        CreateButtons(BuildingsType.TeslaCoil);
    }
    public void OnClickUpgradeBenchButton()
    {
        CreateButtons(BuildingsType.UpgradeBench);
    }
}
