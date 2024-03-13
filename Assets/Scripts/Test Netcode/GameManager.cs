using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    [SerializeField] private PlayerController _playerPrefab;
    [SerializeField] private GameObject _Prefab;

    public override void OnNetworkSpawn()
    {
        SpawnPlayerServerRpc(NetworkManager.Singleton.LocalClientId);
        base.OnNetworkSpawn();
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnPlayerServerRpc(ulong playerId)
    {
        /*var spawn = Instantiate(_playerPrefab);
        spawn.NetworkObject.SpawnWithOwnership(playerId);*/
        var spawn = Instantiate(_Prefab);
        spawn.GetComponent<NetworkObject>().SpawnWithOwnership(playerId);
    }
}
