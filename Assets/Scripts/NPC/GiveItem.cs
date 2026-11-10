using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveItem : MonoBehaviour
{
    [SerializeField] private bool giveWeapon;
    [SerializeField] private bool giveKey;
    [SerializeField] private bool giveMoney;

    [Space(5)]

    [SerializeField] private int weaponIndex;
    [SerializeField] private int keyIndex;
    [SerializeField] private int moneyAmount;

    public void Give()
    {
        if (giveWeapon)
            HandController.BuyWeapon(weaponIndex);

        if (giveKey)
            PlayerController.AddAmmo(keyIndex, TakeAmmo.AmmoType.Key);
    
        if(giveMoney)
            PlayerController.AddMoney(moneyAmount);
    }
}
