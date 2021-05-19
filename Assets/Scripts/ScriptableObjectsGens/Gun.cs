using UnityEngine;

[CreateAssetMenu(fileName = "New Gun", menuName = "Scriptable Objects/Weapons/Gun")]
public class Gun : ScriptableObject
{
    public string gunName;
    public GameObject prefab;
    public GameObject bulletPrefab;
    public float range = 100;
    public float rateOfFire = 24;
    public float recoil = 10;
    public float kickback = 0.1f;
    public float hitArea = 1;
    public float damage = 15;
    public float headshotDamage = 60;
    [HideInInspector] public int clipAmmo;
    [HideInInspector] public int reserveAmmo;
    public int magazineSize = 8;
    public int maxAmmo = 120;
    public float reloadTime = 2f;
    public float adsSpeed = 20;
}
