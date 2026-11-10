using UnityEngine;

[CreateAssetMenu(menuName = "Weapon", fileName = "Weapon")]
public class WeaponScriptableObject : ScriptableObject
{
    public int maxAmmoInMagazine = 30;
    public int maxAmmo = 120;
    public int damage = 5;
    public float shootCooldown = .1f;
    public float reloadTime = 2.5f;
    public bool castRay = true;
    public float spreadKof = .3f;
    public int bulletsInSpread = 1;
    public float bulletDistance = 25;
}
