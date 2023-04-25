using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SidePanel : MonoBehaviour
{
    public Image buildingImage;
    public TMP_Text buildingText;

    public void Start()
    {
        buildingImage.color = Color.clear;
        buildingText.text = "";
    }

    public void UpdateSideDisplay(BuildingsData data)
    {
        buildingImage.color = data.color;
        buildingText.text = data.DisplayName;
    }
}
