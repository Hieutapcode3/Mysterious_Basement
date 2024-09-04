using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool isKnockedBack = false;
    public Transform pfhealthBar;
    public Transform healthBarPos;
    public HealthSysytem healthSystem;
    [SerializeField] protected int health;

    [SerializeField] protected Sprite headTop;
    [SerializeField] protected Sprite bodyTop;
    [SerializeField] protected Sprite headRight;
    [SerializeField] protected Sprite bodyRight;
    [SerializeField] protected Sprite headDown;
    [SerializeField] protected Sprite bodyDown;

    [SerializeField] protected GameObject headObject;
    [SerializeField] protected GameObject bodyObject;
    [SerializeField] protected Transform pfEnemyDeadBody;
    protected Rigidbody2D rb;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }
    protected virtual void Start()
    {
        healthSystem = new HealthSysytem(health);
        Transform healthBarTransform = Instantiate(pfhealthBar, healthBarPos.transform.position, Quaternion.identity);
        healthBarTransform.SetParent(healthBarPos);
        HealthBar healthBar = healthBarTransform.GetComponent<HealthBar>();
        healthBar.SetUp(healthSystem);
        healthSystem.OnDeath += HealthSystem_OnDeath;
    }
    protected virtual void Update()
    {
        health = healthSystem.GetHealth();

    }
    protected virtual void HealthSystem_OnDeath(object sender, EventArgs e)
    {
        Vector2 flyDirection = (transform.position - PlayerMoveMent.instance.transform.position).normalized;
        FlyingBody.Create(pfEnemyDeadBody, transform.position, flyDirection);
        Destroy(gameObject);
    }
    public virtual void DamageAttack()
    {
        int damage = UnityEngine.Random.Range(5, 10); 
        bool isCriticalHit = damage > 7;
        TestingDamage.Instance.CreateDamageTxt(damage, PlayerMoveMent.instance.gameObject.transform.position, isCriticalHit);
        PlayerMoveMent.instance.healthSystem.Damage(damage);
    }
    public virtual void ApplyKnockback(Vector2 direction, float force)
    {
        float knockbackDuration = 0.07f;
        isKnockedBack = true;
        rb.velocity = Vector2.zero;
        rb.AddForce(direction * force * 10f, ForceMode2D.Impulse);
        Invoke("ResetKnockback", knockbackDuration);
    }


    public virtual void ResetKnockback()
    {
        isKnockedBack = false;
        rb.velocity = Vector2.zero;
    }

}
