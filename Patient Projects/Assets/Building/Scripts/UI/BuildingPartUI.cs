using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class BuildingPartUI : MonoBehaviour
{
    private Button button;
    private BuildingsData buildingData;
    private BuildPanelUI buildPanelUI;

    public void Init(BuildingsData chosenBuildingData, BuildPanelUI panelUI)
    {
        buildingData = chosenBuildingData;
        buildPanelUI = panelUI;
        button = GetComponentInChildren<Button>();
        button.onClick.AddListener(OnButtonClick);
        button.GetComponent<Image>().color = buildingData.color;

    }
    private void OnButtonClick()
    {
        buildPanelUI.OnClick(buildingData);
    }
}
