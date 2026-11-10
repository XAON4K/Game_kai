using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoVisualizer : MonoBehaviour
{
    private Text ammoText;

    private static AmmoVisualizer instance;

    private void Awake()
    {
        instance = this;
        ammoText = GetComponent<Text>();
    }

    public static void UpdateText(int ammoInMagazine, int fullAmmo)
    {
        instance.ammoText.text = $"{ammoInMagazine}/{fullAmmo}";
    }

    public static void UpdateText()
    {
        instance.ammoText.text = " ";
    }
}
