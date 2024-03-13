using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Netcode;
using UnityEngine;

public class Shooting : NetworkBehaviour
{
    private float fireRatePerMinute = 900f;

    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    //private List<GameObject> spawnedBullets = new List<GameObject>();

    private float nextTimeToFier = 0f;

    private void Update()
    {
        if (!IsOwner) return;

        if (Input.GetMouseButton(0) && Time.time >= nextTimeToFier)
        {
            nextTimeToFier = Time.time + 60f / fireRatePerMinute;

            ShootServerRpc(firePoint.position, firePoint.rotation);
            Shoot(firePoint.position, firePoint.rotation);
        }
    }

    [ServerRpc]
    private void ShootServerRpc(Vector3 v, Quaternion q)
    {
        ShootClientRpc(v, q);
    }

    [ClientRpc]
    private void ShootClientRpc(Vector3 v, Quaternion q)
    {
        if (!IsOwner) Shoot(v, q);
    }

    private void Shoot(Vector3 v, Quaternion q)
    {
        GameObject bullet = Instantiate(bulletPrefab, v, q);
        bullet.GetComponent<Bullet>().parent = this;
        //spawnedBullets.Add(bullet);
        //bullet.GetComponent<NetworkObject>().Spawn();
    }
}
