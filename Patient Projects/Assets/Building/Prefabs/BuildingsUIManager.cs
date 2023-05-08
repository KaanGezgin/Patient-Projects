using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BuildingsUIManager : MonoBehaviour
{
    public GameObject background;
    public GameObject energyStationUI;
    public GameObject beconUI;
    public GameObject uprafUI;

    private void Start()
    {
        background.SetActive(false);
        energyStationUI.SetActive(false);
        beconUI.SetActive(false);
        uprafUI.SetActive(false);
        SetMouseCursorState(background.activeInHierarchy);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            CloseAllUIs();
            SetMouseCursorState(background.activeInHierarchy);
        }
    }

    private void SetMouseCursorState(bool newState)
    {
        Cursor.visible = newState;
        Cursor.lockState = newState ? CursorLockMode.Confined : CursorLockMode.Locked;
    }

    public void OpenBeconUI()
    {
        background.SetActive(!background.activeInHierarchy);
        beconUI.SetActive(true);
        SetMouseCursorState(background.activeInHierarchy);
    }
    public void OpenTeslaCoilUI()
    {
        background.SetActive(!background.activeInHierarchy);
        energyStationUI.SetActive(true);
        SetMouseCursorState(background.activeInHierarchy);
    }
    public void OpenUprafUI()
    {
        background.SetActive(!background.activeInHierarchy);
        uprafUI.SetActive(true);
        SetMouseCursorState(background.activeInHierarchy);
    }

    public void CloseAllUIs()
    {
        background.SetActive(false);
        energyStationUI.SetActive(false);
        beconUI.SetActive(false);
        uprafUI.SetActive(false);
    }
}
