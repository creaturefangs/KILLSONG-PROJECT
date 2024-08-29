using UnityEngine;

[CreateAssetMenu(menuName = "Bullet", order = 5)]
public class SO_Bullet : ScriptableObject
{
    [Tooltip("Prefab for the bullet object")]
    public GameObject bulletPrefab;

    [Tooltip("Name of the bullet, could be useful later")]
    public string bulletName;

    [Tooltip("Amount of damage this bullet inflicts")]
    public float bulletDamage;

    [Tooltip("Speed at which this bullet travels")]
    public float bulletSpeed;
}