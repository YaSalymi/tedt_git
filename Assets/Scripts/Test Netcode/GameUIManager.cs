using UnityEditor;
using UnityEngine;
using TMPro;

public class GameUIManager : MonoBehaviour
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

    [SerializeField] private GameObject pauseUIPlain;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseUIPlain.SetActive(!pauseUIPlain.activeSelf);
            
        }
    }

    [SerializeField] private string m_SceneName;
    public void ExitMainMenue()
    {
        RelayManager.Instance.LeaveServer(m_SceneName);
    }

    [SerializeField] private GameObject copyButton;
    [SerializeField] private TMP_Text joinCodeField;
    private void Awake()
    {
        joinCodeField.text = RelayManager.Instance.JoinCode;
        if (joinCodeField.text != null) copyButton.SetActive(true);
    }
    public void CopyJoinCode()
    {
        GUIUtility.systemCopyBuffer = joinCodeField.text;
    }
    


}
