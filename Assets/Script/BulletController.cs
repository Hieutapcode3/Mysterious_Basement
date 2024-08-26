using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float knockbackForce;
    private EnemyController enemy;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            enemy = collision.gameObject.GetComponent<EnemyController>();
            enemy.hasReachedTarget = true;
            if (enemy != null && enemy.enabled == true)
            {
                Vector3 collisionPoint = collision.bounds.ClosestPoint(transform.position);
                collisionPoint.z = 0f;
                int damage = Random.Range(120, 150);
                bool isCriticalhit = damage > 140;
                TestingDamage.Instance.CreateDamageTxt(damage, collisionPoint, isCriticalhit);
                enemy.healthSystem.Damage(damage);
                Transform playerTransform = FindObjectOfType<PlayerMoveMent>().transform;
                Vector2 knockbackDirection = (enemy.transform.position - playerTransform.position).normalized;
                FindObjectOfType<BloodSystem>().SpawnBlood(enemy.transform.position, knockbackDirection);
                enemy.ApplyKnockback(knockbackDirection, knockbackForce);
                gameObject.SetActive(false);
            }
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
