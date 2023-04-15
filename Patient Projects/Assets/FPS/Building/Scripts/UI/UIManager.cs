using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public BuildPanelUI buildPanel;

    public void Start()
    {
        buildPanel.gameObject.SetActive(false);
        SetMouseCursorState(buildPanel.gameObject.activeInHierarchy);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            buildPanel.gameObject.SetActive(!buildPanel.gameObject.activeInHierarchy);
            SetMouseCursorState(buildPanel.gameObject.activeInHierarchy);
        }
    }
    private void SetMouseCursorState(bool newState)
    {
        Cursor.visible = newState;
        Cursor.lockState = newState ? CursorLockMode.Confined : CursorLockMode.Locked;
    }
}
