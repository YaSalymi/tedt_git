using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private Camera cam;

    [SerializeField] private float movingSpeed = 5f;

    private Vector2 inputVector;
    private Vector2 mousePos;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = FindObjectOfType<Camera>();
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;


        /*Vector2 inputVector = GameInput.Instance.GetMovementVector();*/

        inputVector = Vector2.zero;
        if (Input.GetKey(KeyCode.W)) inputVector.y += 1f;
        if (Input.GetKey(KeyCode.S)) inputVector.y -= 1f;
        if (Input.GetKey(KeyCode.D)) inputVector.x += 1f;
        if (Input.GetKey(KeyCode.A)) inputVector.x -= 1f;

        inputVector.Normalize();
        rb.MovePosition(rb.position + inputVector * (movingSpeed * Time.fixedDeltaTime));


        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        Vector2 loocDir = mousePos - rb.position;
        float angle = Mathf.Atan2(loocDir.y, loocDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }

/*    public override void OnNetworkSpawn()
    {
        if (!IsOwner) Destroy(this);
    }*/
}
