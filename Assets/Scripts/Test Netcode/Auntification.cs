using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Auntification : MonoBehaviour
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

    private async void Awake()
    {
        await Login();
        SceneManager.LoadSceneAsync(m_SceneName);
    }

    private static async Task Login()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }
}
