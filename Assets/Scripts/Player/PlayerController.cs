using Unity.Netcode;
using UnityEngine;
using Cinemachine;

public class PlayerController : NetworkBehaviour
{
    [Header("Refs")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private CinemachineVirtualCamera vc;
    [SerializeField] private AudioListener listener;
    [SerializeField] private new Camera camera;
    

    private Vector2 previosMovementInput;

    private float deltaAngle = 0f;

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            listener.enabled = true;
            vc.Priority = 1;

            inputReader.MoveEvent += PlayerMove;
        }
        else
        {
            vc.Priority = 0;
        }
    }

    private void PlayerMove(Vector2 movementInput)
    {
        previosMovementInput = movementInput;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner) return;

        inputReader.MoveEvent -= PlayerMove;
    }

    private void Update()
    {
        if (!IsOwner) return;

        GetRotation();

        GetMove();
    }

    [Header("Settings")]
    [SerializeField] private float movingSpeed = 5f;
    private void GetMove()
    {
        rb.MovePosition(rb.position + (Vector2)(transform.rotation * previosMovementInput * movingSpeed * Time.fixedDeltaTime));
    }

    [SerializeField] private float maxRotationSpeed = 360f;
    [SerializeField] private float timeRotation = 0.1f;
    private void GetRotation()
    {
        Vector2 mousePos = FakeCursor.Instance.transform.position;

        Vector2 loocDir = Quaternion.Euler(0, 0, -90f) * (mousePos - rb.position); // тут надо повернуть вектор на 90 градусов вправо из-за того что градусный отчёт начинается справа а не спереди
        float angle = Mathf.Atan2(loocDir.y, loocDir.x) * Mathf.Rad2Deg;

        // Фиксем неэфективный поворот через большую сторону на -90 градусах
        if (rb.rotation <= -90 && angle >= 90)
        {
            rb.rotation += 360;
            rb.rotation = Mathf.SmoothDamp(rb.rotation, angle, ref deltaAngle, timeRotation, maxRotationSpeed);
        }
        else if (rb.rotation >= 90 && angle <= -90)
        {
            rb.rotation -= 360;
            rb.rotation = Mathf.SmoothDamp(rb.rotation, angle, ref deltaAngle, timeRotation, maxRotationSpeed);
        }
        else
        {
            rb.rotation = Mathf.SmoothDamp(rb.rotation, angle, ref deltaAngle, timeRotation, maxRotationSpeed);
        }

        FakeCursor.Instance.transform.rotation = transform.rotation;
    }
}
