using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using Unity.Services.Relay.Models;
using Unity.Services.Relay;
using TMPro;
using Unity.Netcode.Transports.UTP;
using System.Data;
using System.Threading.Tasks;

public class RelayManager : NetworkBehaviour
{
    public static RelayManager Instance { get; private set; }
    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        Instance = this;

        _transport = FindObjectOfType<UnityTransport>();
    }

    #region �������� �����, ��������� ������� � ��������� �������
    private UnityTransport _transport;

    private string joinCode;
    public string JoinCode
    {
        get => joinCode;
    }
    private bool isStarted = false;
    public bool IsStarted
    {
        get => isStarted;
    }

    public async Task StartHost(string loadingSceneName)
    {
        if (!isStarted)
        {
            try
            {
                Allocation a = await RelayService.Instance.CreateAllocationAsync(5);
                joinCode = await RelayService.Instance.GetJoinCodeAsync(a.AllocationId);
                _transport.SetHostRelayData(a.RelayServer.IpV4, (ushort)a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData);

                sceneName = loadingSceneName;
                NetworkManager.Singleton.StartHost();

                isStarted = true;
            }
            catch (ArgumentException e)
            {
                Debug.LogError("�� ������� ������� ����");
                Debug.LogError(e);

                NetworkManager.Singleton.Shutdown();

                isStarted = false;
            }
        }
        else
        {
            Debug.LogError("NetworkManager ��� �������");
        }

    }

    public async Task StartClient(string enteredJoinCode)
    {
        if (!isStarted)
        {
            try
            {
                JoinAllocation a = await RelayService.Instance.JoinAllocationAsync(enteredJoinCode);
                _transport.SetClientRelayData(a.RelayServer.IpV4, (ushort)a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData, a.HostConnectionData);
                NetworkManager.Singleton.StartClient();
                
                isStarted = true;
            }
            catch (ArgumentException e)
            {
                Debug.LogError("��� ����������� �� ������");
                Debug.LogError(e);

                NetworkManager.Singleton.Shutdown();

                isStarted = false;
            }
        }
        else
        {
            Debug.LogError("NetworkManager ��� �������");
        }
    }

    private const string m_SceneName = "Main Manue";
    public void LeaveServer(string loadingSceneName = m_SceneName)
    {
        if (isStarted)
        {
            try
            {
                sceneName = loadingSceneName;
                NetworkManager.Singleton.Shutdown();
                SceneManager.LoadScene(sceneName, LoadSceneMode.Single);

                isStarted = false;
            }
            catch (ArgumentException e)
            {
                Debug.LogError(e);

                isStarted = true;
            }
        }
        else
        {
            Debug.LogError("NetworkManager �� �������");
        }
    }
    #endregion

    #region �������� � ������� �� �������

    private string sceneName;
    public override void OnNetworkSpawn()
    {
        if (IsServer && !string.IsNullOrEmpty(sceneName))
        {
            NetworkManager.SceneManager.OnSceneEvent += SceneManager_OnSceneEvent;
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnectCallback;

            var status = NetworkManager.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
            CheckStatus(status);
        }

        base.OnNetworkSpawn();
    }

    public override void OnNetworkDespawn()
    {
        if (IsServer && !string.IsNullOrEmpty(sceneName))
        {
            NetworkManager.SceneManager.OnSceneEvent -= SceneManager_OnSceneEvent;
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnectedCallback;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnectCallback;
        }

        base.OnNetworkDespawn();
    }
    #endregion

    #region ����������

    private void OnClientConnectedCallback(ulong clientId)
    {
        Debug.LogError(NetworkManager.Singleton.LocalClientId);
        Debug.LogError(NetworkManager.ConnectedClients.Count);
    }

    private void OnClientDisconnectCallback(ulong clientId)
    {
        Debug.LogError(NetworkManager.ConnectedClients.Count);
    }

    private void CheckStatus(SceneEventProgressStatus status, bool isLoading = true)
    {
        var sceneEventAction = isLoading ? "���������" : "���������";
        if (status != SceneEventProgressStatus.Started)
        {
            Debug.LogWarning($"�� ������� {sceneEventAction} {m_SceneName} � ������� {nameof(SceneEventProgressStatus)}: {status}");
        }
    }

    private void SceneManager_OnSceneEvent(SceneEvent sceneEvent)
    {
        var clientOrServer = sceneEvent.ClientId == NetworkManager.ServerClientId ? "������" : "������";
        switch (sceneEvent.SceneEventType)
        {
            case SceneEventType.LoadComplete:
                {                 
                    if (sceneEvent.ClientId == NetworkManager.ServerClientId)
                    {
                        //m_LoadedScene = sceneEvent.Scene;
                    }
                    Debug.Log($"�������� {sceneEvent.SceneName} ����� �� {clientOrServer}-({sceneEvent.ClientId}).");
                    break;
                }
            case SceneEventType.UnloadComplete:
                {
                    Debug.Log($"�������� {sceneEvent.SceneName} ����� �� {clientOrServer}-({sceneEvent.ClientId}).");
                    break;
                }
            case SceneEventType.LoadEventCompleted:
            case SceneEventType.UnloadEventCompleted:
                {
                    var loadUnload = sceneEvent.SceneEventType == SceneEventType.LoadEventCompleted ? "��������" : "��������";

                    Debug.Log($"������� {loadUnload} ��������� ��� ���������� �������������� �������:({sceneEvent.ClientsThatCompleted})");
                    if (sceneEvent.ClientsThatTimedOut.Count > 0)
                    {
                        Debug.LogWarning($"����� �������� ������� {loadUnload} ������� ��� ��������� ��������������� �������:({sceneEvent.ClientsThatTimedOut})");
                    }
                    break;
                }
        }
    }
    #endregion
}

#region �������� �����������
/*

public void LeaveLobby()
{

}

public void UnloadScene()
{
    if (!IsServer || !IsSpawned || !m_LoadedScene.IsValid() || !m_LoadedScene.isLoaded)
    {
        return;
    }

    var status = NetworkManager.SceneManager.UnloadScene(m_LoadedScene);
    CheckStatus(status, false);
}

*/
#endregion