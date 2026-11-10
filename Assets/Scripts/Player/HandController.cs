using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HandController : MonoBehaviour
{
    [SerializeField] private GameObject[] weapons;
    [SerializeField] private GameObject[] weaponIcons;

    private static HandController instance;

    private static List<bool> weaponsBought = new List<bool>() { false, false, false, true };

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        weapons[3].SetActive(true);

        UpdateIcons();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && weaponsBought[0])
            ChangeWeapon(0);

        if(Input.GetKeyDown(KeyCode.Alpha2) && weaponsBought[1])
            ChangeWeapon(1);

        if( Input.GetKeyDown(KeyCode.Alpha3) && weaponsBought[2])
            ChangeWeapon(2);

        if(Input.GetKey(KeyCode.Alpha4) && weaponsBought[3])
            ChangeWeapon(3);
    }

    private void ChangeWeapon(int index)
    {
        for(int i = 0; i < weapons.Length; i++)
            weapons[i].SetActive(i == index);
    }

    private static void UpdateIcons()
    {
        for(int i = 0; i < instance.weaponIcons.Length; i++)
        {
            instance.weaponIcons[i].SetActive(weaponsBought[i]);
        }
    }

    public static void BuyWeapon(int index)
    {
        weaponsBought[index] = true;

        UpdateIcons();
    }
}
