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
        var building = buildings.Where(p => p.buildingType == chosenBuilding).ToArray();
        SpawnButtons(building);
    }
    public void SpawnButtons(BuildingsData[] buttonData)
    {
        ClearButtons();
        foreach (var data in buttonData)
        {
            var spawnedButton = Instantiate(buildingButton, buildingVariantPanel.transform);
            spawnedButton.Init(data, this);
        }
    }
    public void ClearButtons()
    {
        foreach(var button in buildingVariantPanel.transform.Cast<Transform>())
        {
            Destroy(button.gameObject);
        }
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
