using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    [SerializeField] private float talkDistance = 5f;

    [Space(5)]

    [SerializeField] private GameObject eIcon;
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private GameObject shopObjectsSpawnParent;
    [SerializeField] private GameObject shopObjectPrefab;
    [SerializeField] private Text moneyText;

    [Space(5)]

    [SerializeField] private ShopObjectClass[] shopObjects;

    bool isShopActive = false;

    Transform player;
    Transform _camera;

    List<GameObject> spawnedShopObjects = new List<GameObject>();

    private static ShopController instance;

    bool isPlayerNear = false;

    public enum ObjectType
    {
        Weapon
    }

    private void Awake()
    {
        instance = this;

        player = FindObjectOfType<PlayerMovement>().transform;
        _camera = Camera.main.transform;
    }

    private void Talk()
    {
        isShopActive = true;
        shopPanel.SetActive(true);

        SpawnShopObjects();
    
        moneyText.text = PlayerController.GetMoney.ToString();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void Update()
    {
        if (isShopActive && Input.GetKeyDown(KeyCode.Escape))
        {
            shopPanel.SetActive(false);
            isShopActive = false;

            eIcon.SetActive(false);
            ClearShopObjects();

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (Vector3.Distance(player.position, transform.position) < talkDistance)
        {
            eIcon.SetActive(true);
            isPlayerNear = true;

            if (Input.GetKeyDown(KeyCode.E))
                ReadRayCast();
        }
        else if (isPlayerNear)
        {
            isPlayerNear = false;
            eIcon.SetActive(false);
        }
    }

    private void ReadRayCast()
    {
        Ray ray = new Ray(_camera.position, _camera.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, talkDistance))
        {
            if (hit.collider.gameObject == gameObject)
                Talk();
        }
    }

    private void SpawnShopObjects()
    {
        foreach(ShopObjectClass settings in shopObjects)
        {
            GameObject obj = Instantiate(shopObjectPrefab, shopObjectsSpawnParent.transform);
            spawnedShopObjects.Add(obj);

            obj.GetComponent<ShopObject>().UpdateText(settings);
        }
    }

    private void ClearShopObjects()
    {
        foreach(GameObject obj in spawnedShopObjects)
        {
            Destroy(obj);
        }

        spawnedShopObjects.Clear();
    }

    public static bool BuyShopObject(ObjectType type, int index, int cost)
    {
        if(PlayerController.GetMoney < cost) return false;

        PlayerController.AddMoney(-cost);

        instance.moneyText.text = PlayerController.GetMoney.ToString();

        switch (type)
        {
            case ObjectType.Weapon:
                HandController.BuyWeapon(index);
                break;
        }

        return true;
    }

    public static bool IsShopActive()
    {
        if(instance == null)
        {
            return false;
        }
        else if(!instance.isShopActive)
        {
            return false;
        }

        return true;
    }
}

[System.Serializable]
public class ShopObjectClass
{
    public Sprite icon;
    public ShopController.ObjectType objectType;
    public int index;
    public int cost;
}
