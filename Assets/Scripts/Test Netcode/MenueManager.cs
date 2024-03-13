using TMPro;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;

public class MenueUIManager : MonoBehaviour
{
#if UNITY_EDITOR
    public SceneAsset SceneAsset;

    private void OnValidate()
    {
        if (SceneAsset != null)
        {
            m_SceneName = SceneAsset.name;
        }
    }
#endif
    [SerializeField] private string m_SceneName;
    
    [SerializeField] private GameObject mainMenuePlain;
    [SerializeField] private GameObject serverBrowserPlain;
    [SerializeField] private GameObject settingsPlain;

    [SerializeField] private TMP_InputField _joinInput;

    public async void StartHost()
    {
        serverBrowserPlain.SetActive(false);
        await RelayManager.Instance.StartHost(m_SceneName);
        if (!RelayManager.Instance.IsStarted) serverBrowserPlain.SetActive(true);
    }

    public async void StartClient()
    {
        //serverBrowserPlain.SetActive(false);
        await RelayManager.Instance.StartClient(_joinInput.text);
        //if (!RelayManager.Instance.IsStarted) serverBrowserPlain.SetActive(true);
    }

    public void OpenSettings()
    {
        mainMenuePlain.SetActive(false);
        serverBrowserPlain.SetActive(false);
        settingsPlain.SetActive(true);
    }

    public void OpenMainMenue()
    {
        mainMenuePlain.SetActive(true);
        serverBrowserPlain.SetActive(false);
        settingsPlain.SetActive(false);
    }

    public void OpenServerBrowser()
    {
        mainMenuePlain.SetActive(false);
        serverBrowserPlain.SetActive(true);
        settingsPlain.SetActive(false);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
