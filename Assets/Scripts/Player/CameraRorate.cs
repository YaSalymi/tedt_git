using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CameraRorate : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        if (!IsOwner) Destroy(gameObject);
        base.OnNetworkSpawn();
    }
}
