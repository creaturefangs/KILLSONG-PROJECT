using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    public enum WeaponType
    {
        OneShot,
        SingleFire,
        BurstFire,
        Automatic
    }
    public WeaponType weaponType;
    public float maxRange;
    public int startingMagazineAmmo;
    public int maxMagazineAmmo;
}
