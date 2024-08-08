using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 2f;
    private Vector2 moveDirection;

    void Update()
    {
        transform.position += (Vector3)moveDirection * speed * Time.deltaTime;
    }

    public void SetMoveDirection(Vector2 direction)
    {
        moveDirection = direction.normalized;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            Debug.Log("va cham");
            bool isCriticalhit = Random.Range(0f, 100f) < 30;
            Vector3 collisionPoint = collision.bounds.ClosestPoint(transform.position);
            collisionPoint.z = 0f;
            TestingDamage.Instance.CreateDamageTxt(collisionPoint,isCriticalhit);
            gameObject.SetActive(false );
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            gameObject.SetActive(false);
        }
    }
}
