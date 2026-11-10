using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject menu;
    [SerializeField] private Slider sensetivitySlider;

    public void Play()
    {
        SceneManager.LoadScene("Island");
    }

    public void Settings()
    {
        float s = PlayerPrefs.GetFloat("Sensetivity");
        sensetivitySlider.value = s > 0 ? s : .7f;

        menu.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        menu.SetActive(true);
        settingsPanel.SetActive(false);

        PlayerPrefs.SetFloat("Sensetivity", sensetivitySlider.value);
        PlayerPrefs.Save();
    }

    public void Quit()
    {
        PlayerPrefs.Save();
        Application.Quit();
    }
}
