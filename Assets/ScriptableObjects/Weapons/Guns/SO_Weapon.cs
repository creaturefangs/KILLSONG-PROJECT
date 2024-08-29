using UnityEngine;

public enum WeaponType
{
    OneShot,
    SingleFire,
    BurstFire,
    Automatic
}

[CreateAssetMenu(menuName = "Weapon", order = 4)]
public class SO_Weapon : ScriptableObject
{
    [Header("General Weapon Settings")] 
    public SO_Bullet bullet;

    [Tooltip("Name of the weapon")]
    public string weaponName;  

    [Tooltip("Type of weapon (OneShot, SingleFire, BurstFire, Automatic)")]
    public WeaponType weaponType; 

    [Tooltip("Maximum effective range of the weapon")]
    public float maxRange = 50.0f;

    [Header("Ammo Settings")]

    [Tooltip("Maximum ammo the magazine can hold")]
    public int maxMagazineAmmo = 30;

    [Tooltip("Starting ammo in the magazine")]
    public int startingMagazineAmmo = 30;

    [Tooltip("Maximum amount of reserve ammo")]
    public int maxReserveAmmo = 90;

    [Header("Fire Rate Settings")]

    [Tooltip("Time between shots for automatic weapons")]
    public float fireRate = 0.1f;

    [Tooltip("Time between bursts for burst fire weapons")]
    public float burstDelay = 0.2f;

    [Tooltip("Number of shots per burst")]
    public int burstCount = 3;  

    [Header("Visual and Audio")]

    [Tooltip("Sound that plays when the weapon fires")]
    public AudioClip fireSound;

    [Tooltip("Sound that plays when the weapon reloads")]
    public AudioClip reloadSound; 

    [Header("AI and Vision Settings")]

    [Tooltip("Time it takes for the AI to lock onto a target")]
    public float lockOnTime = 10.0f; 

    [Tooltip("Speed at which the weapon rotates towards the target")]
    public float rotationSpeed = 2.0f;

    [Tooltip("Angle of the vision cone in degrees")]
    public float visionAngle = 45.0f; 

    [Header("Other Settings")]

    [Tooltip("Prefab for the muzzle flash effect")]
    public GameObject muzzleFlashPrefab;
    
    [Tooltip("Icon to represent the weapon in UI")]
    public Sprite weaponIcon;  

    [Tooltip("Time it takes to reload the weapon")]
    public float reloadTime = 2.0f;
}