using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopObject : MonoBehaviour
{
    [SerializeField] private Text costText;
    [SerializeField] private Image icon;

    private int cost;
    private int index;
    ShopController.ObjectType type;

    public void UpdateText(ShopObjectClass objectSettings)
    {
        cost = objectSettings.cost;
        costText.text = objectSettings.cost.ToString();
        icon.sprite = objectSettings.icon;
        index = objectSettings.index;
        type = objectSettings.objectType;
    }

    public void Buy()
    {
        if(ShopController.BuyShopObject(type, index, cost))
            Destroy(gameObject);
    }
}
