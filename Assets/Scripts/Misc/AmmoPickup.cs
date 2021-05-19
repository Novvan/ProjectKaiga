using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour, IPickUp
{
    [SerializeField] private int amount;

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            WeaponHandler _wp = other.transform.root.GetComponent<WeaponHandler>();
            _wp.PlayerLoadout[_wp.CurrentWeaponIndex].reserveAmmo += amount;


            Destroy(gameObject);
        }
    }
}
