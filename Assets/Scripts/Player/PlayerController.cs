using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int starterHealth = 100;

    [Space(6)]

    [SerializeField] private ProgressBar healthBar;
    [SerializeField] private WeaponController autogun, rocketLauncher, shotGun;
    [SerializeField] private Transform _player;

    [Space(5)]

    [SerializeField] private GameObject letter;

    List<int> keysFound = new List<int>();

    int hp;
    private static int money;

    private static PlayerController instance;

    private void Awake()
    {
        instance = this;

        hp = starterHealth;
        UpdateHealthBar();
    }

    public static void AddAmmo(int ammount, TakeAmmo.AmmoType ammoType)
    {
        switch(ammoType)
        {
            case TakeAmmo.AmmoType.Autogun:
                instance.autogun.AddAmmo(ammount);
                break;

            case TakeAmmo.AmmoType.Rocket_Launcher:
                instance.rocketLauncher.AddAmmo(ammount);
                break;

            case TakeAmmo.AmmoType.Shotgun:
                instance.shotGun.AddAmmo(ammount);
                break;

            case TakeAmmo.AmmoType.Key:
                instance.AddKey(ammount);
                break;

            case TakeAmmo.AmmoType.Health:
                instance.hp += ammount;
                if(instance.hp > instance.starterHealth)
                    instance.hp = instance.starterHealth;

                instance.UpdateHealthBar();

                break;

            case TakeAmmo.AmmoType.Money:
                AddMoney(ammount);
                break;
        }
    }

    private void UpdateHealthBar()
    {
        float value = Vector2.Lerp(Vector2.zero, new Vector2(1, 0), (float)hp / (float)starterHealth).x;

        healthBar.value = value;
    }

    private void Update()
    {
        if (isLerpAnimationActive)
            LerpTimeScale();
    }

    private void Die()
    {
        elapsedLerpTime = 0;
        isLerpAnimationActive = true;
    }

    private void DieAnimationOver()
    {
        Time.timeScale = 1;
        hp = starterHealth;

        if (autogun.GetAmmo.Item2 <= 0) autogun.AddAmmo(45);

        CheckpointSystem.MovePlayerToLastCheckpoint();
        UpdateHealthBar();
    }

    float elapsedLerpTime;
    float desiredLerpTime = .7f;
    float desiredAnimationTime = .85f;
    bool isLerpAnimationActive = false;

    private void LerpTimeScale()
    {
        elapsedLerpTime += Time.deltaTime;

        float percentage = elapsedLerpTime / desiredLerpTime;
        
        if(percentage >= 1)
        {
            float newPercentage = elapsedLerpTime / desiredAnimationTime;
            if (newPercentage >= 1)
            {
                isLerpAnimationActive = false;
                DieAnimationOver();
            }

            return;
        }

        Time.timeScale = Vector2.Lerp(new Vector2(1, 0), new Vector2(.2f, 0), percentage).x;
    }

    public void AddKey(int index)
    {
        keysFound.Add(index);
    }

    public static void GiveDamage(int damage)
    {
        instance.hp -= damage;

        instance.UpdateHealthBar();

        if (instance.hp <= 0)
            instance.Die();
    }

    public static bool ReadKey(int index)
    {
         return instance.keysFound.Contains(index);
    }

    public static Transform GetPlayer()
    {
        return instance._player;
    }

    public static int GetMoney => money;

    public static void AddMoney(int value)
    {
        money += value;
    }

    public static void ActivateLetter()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        instance.letter.SetActive(true);
    }

    public static void ActivateLetter(float time)
    {
        instance.StartCoroutine(instance.letterTimer(time));
    }

    IEnumerator letterTimer(float time)
    {
        yield return new WaitForSeconds(time);
        ActivateLetter();
    }
}
