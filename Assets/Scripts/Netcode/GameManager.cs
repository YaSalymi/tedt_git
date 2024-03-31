using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    //[SerializeField] private PlayerController _playerPrefab;
    [SerializeField] private GameObject _Prefab;
    [SerializeField] private Transform spawnZone1;
    [SerializeField] private Transform spawnZone2;

    public override void OnNetworkSpawn()
    {
        SpawnPlayerServerRpc(NetworkManager.Singleton.LocalClientId);

        FakeCursor.Instance.LockCursor();

        base.OnNetworkSpawn();
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnPlayerServerRpc(ulong playerId)
    {
        /*var spawn = Instantiate(_playerPrefab);
        spawn.NetworkObject.SpawnWithOwnership(playerId);*/
        var spawn = Instantiate(_Prefab);
        if (playerId % 2 == 0)
        {
            spawn.transform.position = spawnZone1.transform.position;
        }
        else
        {
            spawn.transform.position = spawnZone2.transform.position;
        }
        
        spawn.GetComponent<NetworkObject>().SpawnWithOwnership(playerId);
    }
}
