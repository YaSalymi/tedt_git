using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;

public class Target : NetworkBehaviour
{
    private float helth = 50;

    public void TakeDamage(float amount)
    {
        helth -= amount;
        if (helth <= 0f)
        {
            DieServerRpc();
            gameObject.SetActive(false);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void DieServerRpc()
    {
        gameObject.GetComponent<NetworkObject>().Despawn(false);
    }

    public override void OnNetworkDespawn()
    {
        gameObject.SetActive(false);
        base.OnNetworkDespawn();
    }
}
