using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float knockbackForce;
    private Enemy enemy;
    private int maxDamage;
    private int damage;
    private bool isCriticalHit;
    private Cannon canon;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.CompareTag("Bullet"))
        {
            if (collision.CompareTag("Enemy"))
            {
                if (collision.gameObject.name == "PfEnemyPunch")
                {
                    enemy = collision.gameObject.GetComponent<EnemyPunch>();
                }
                else if (collision.gameObject.name == "PfEnemyArcher")
                {
                    enemy = collision.gameObject.GetComponent<EnemyArcher>();
                }
                if (enemy != null && enemy.enabled == true)
                {
                    Vector3 collisionPoint = collision.bounds.ClosestPoint(transform.position);
                    collisionPoint.z = 0f;

                    if (GunController.instance.gunType == GunController.GunType.Pistol)
                    {
                        maxDamage = 150;
                        damage = Random.Range(120, 150);
                    }
                    else if (GunController.instance.gunType == GunController.GunType.Shotgun)
                    {
                        maxDamage = 80;
                        damage = Random.Range(60, 80);
                    }
                    else
                    {
                        maxDamage = 50;
                        damage = Random.Range(30, 50);
                    }
                    isCriticalHit = damage > maxDamage * 0.9f;
                    TestingDamage.Instance.CreateDamageTxt(damage, collisionPoint, isCriticalHit);
                    enemy.healthSystem.Damage(damage);
                    Transform playerTransform = FindObjectOfType<PlayerMoveMent>().transform;
                    Vector2 knockbackDirection = (enemy.transform.position - playerTransform.position).normalized;
                    FindObjectOfType<BloodSystem>().SpawnBlood(enemy.transform.position, knockbackDirection);
                    enemy.ApplyKnockback(knockbackDirection, knockbackForce);
                    gameObject.SetActive(false);
                }
            }
            else if (collision.CompareTag("MachineGun"))
            {
                canon = collision.gameObject.GetComponent<Cannon>();
                maxDamage = 100;
                damage = Random.Range(80, maxDamage);
                isCriticalHit = damage > maxDamage * 0.9f;
                TestingDamage.Instance.CreateDamageTxt(damage, collision.gameObject.transform.position, isCriticalHit);
                canon.healthSystem.Damage(damage);
                gameObject.SetActive(false);
            }
            else if(collision.CompareTag("Shield"))
            {
                gameObject.SetActive(false);

            }
        }
        
        if(gameObject.CompareTag("Bullet_2"))
        {
            if (collision.CompareTag("Player"))
            {
                maxDamage = 15;
                damage = Random.Range(7, maxDamage);
                isCriticalHit = damage > maxDamage * 0.9f;
                TestingDamage.Instance.CreateDamageTxt(damage, PlayerMoveMent.instance.gameObject.transform.position, isCriticalHit);
                PlayerMoveMent.instance.healthSystem.Damage(damage);
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
