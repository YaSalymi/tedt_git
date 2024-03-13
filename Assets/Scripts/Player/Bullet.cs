using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    
    [SerializeField] private float speed = 30f;
    [SerializeField] private float lifeTime = 2f;
    [SerializeField] private float impactForce = 0f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private GameObject hitEfect;
    public Shooting parent;
    private Rigidbody2D bulletRB;


    private void Awake()
    {
        Destroy(gameObject, lifeTime);
        bulletRB = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float radian = (transform.eulerAngles.z + 90f) * Mathf.Deg2Rad;
        bulletRB.MovePosition(bulletRB.position + new Vector2(Mathf.Cos(radian), Mathf.Sin(radian)) * speed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Target target = collision.transform.GetComponent<Target>();
        if (target != null)
        {
            target.TakeDamage(damage);
        }

        Rigidbody2D rb = collision.transform.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForceAtPosition((transform.rotation * Vector2.up).normalized * impactForce, transform.position, ForceMode2D.Impulse);
        }

        //Instantiate(hitEfect, collision.collider.p);

        Destroy(gameObject);
    }
}
