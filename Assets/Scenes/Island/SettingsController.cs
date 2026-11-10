using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private Slider sensetivitySlider;

    [SerializeField] private MouseLook[] cameras;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (ShopController.IsShopActive()) return;

            if(settingsPanel.activeSelf)
                CloseSettings();
            else
                OpenSettings();
        }
    }

    public void CloseSettings()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        PlayerPrefs.SetFloat("Sensetivity", sensetivitySlider.value);
        PlayerPrefs.Save();
        settingsPanel.SetActive(false);

        cameras[0].UpdateSens();
        cameras[1].UpdateSens();
    }

    private void OpenSettings()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        sensetivitySlider.value = PlayerPrefs.GetFloat("Sensetivity", .6f);
        settingsPanel.SetActive(true);
    }

    public void QuitGame()
    {
        PlayerPrefs.Save();
        Application.Quit();
    }
}
