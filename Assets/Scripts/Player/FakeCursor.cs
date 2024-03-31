using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeCursor : MonoBehaviour
{
    public static FakeCursor Instance { get; private set; }

    [SerializeField] private InputReader inputReader;
    public float sensitivity = 0.01f;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        Instance = this;
    }

    private void DeltaCursor(Vector2 deltaMousePosition)
    {
        transform.position += transform.rotation * new Vector3(deltaMousePosition.x, deltaMousePosition.y, 0) * sensitivity;
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        inputReader.MousePositionEvent += DeltaCursor;
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        inputReader.MousePositionEvent -= DeltaCursor;
    }
}
