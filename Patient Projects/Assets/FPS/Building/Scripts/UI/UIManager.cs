using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public BuildPanelUI buildPanel;

    public void Start()
    {
        buildPanel.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            buildPanel.gameObject.SetActive(!buildPanel.gameObject.activeInHierarchy);
        }
    }
}
