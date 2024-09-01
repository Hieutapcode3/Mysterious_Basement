using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed;
    private float originalMoveSpeed;
    public Seeker seeker;
    public float nextWP = 2f;
    private int currentWP = 0;
    public bool hasReachedTarget = false;
    public bool isKnockedBack = false;
    Path path;
    public Transform Target;
    public Transform pfhealthBar;
    public Transform healthBarPos;
    public HealthSysytem healthSystem;
    [SerializeField] private int health;

    [SerializeField] private Sprite headTop;
    [SerializeField] private Sprite bodyTop;
    [SerializeField] private Sprite headRight;
    [SerializeField] private Sprite bodyRight;
    [SerializeField] private Sprite headDown;
    [SerializeField] private Sprite bodyDown;

    [SerializeField] private GameObject headObject;
    [SerializeField] private GameObject bodyObject;
    [SerializeField] private Transform pfEnemyDeadBody;
    [SerializeField] private Transform rightHand;

    private SpriteRenderer headSpriteRenderer;
    private SpriteRenderer bodySpriteRenderer;
    private Animator anim;
    private Rigidbody2D rb;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        originalMoveSpeed = moveSpeed;
        Target = FindObjectOfType<PlayerMoveMent>().gameObject.transform;
        headSpriteRenderer = headObject.GetComponent<SpriteRenderer>();
        bodySpriteRenderer = bodyObject.GetComponent<SpriteRenderer>();
        InvokeRepeating("CalculatePath", 0, 0.5f);
        healthSystem = new HealthSysytem(health);
        Transform healthBarTransform = Instantiate(pfhealthBar, healthBarPos.transform.position, Quaternion.identity);
        healthBarTransform.SetParent(healthBarPos);
        HealthBar healthBar = healthBarTransform.GetComponent<HealthBar>();
        healthBar.SetUp(healthSystem);
        healthSystem.OnDeath += HealthSystem_OnDeath;
    }

    void Update()
    {
        health = healthSystem.GetHealth();
        if (isKnockedBack) return;

        if (path != null && !hasReachedTarget && currentWP < path.vectorPath.Count)
        {
            MoveToTarget();
        }

        float distanceToTarget = Vector2.Distance(transform.position, Target.position);
        if (hasReachedTarget && distanceToTarget > nextWP)
        {
            hasReachedTarget = false;
        }
        Attacking();
    }

    void CalculatePath()
    {
        if (!hasReachedTarget && seeker.IsDone())
        {
            seeker.StartPath(transform.position, Target.position, OnPathCallback);
        }
    }

    void OnPathCallback(Path p)
    {
        if (p.error) return;
        path = p;
        currentWP = 0;
    }

    void MoveToTarget()
    {
        if (path == null || path.vectorPath.Count == 0) return;

        Vector2 currentPos = rb.position;
        Vector2 targetPos = (Vector2)path.vectorPath[currentWP];
        Vector2 direction = (targetPos - currentPos).normalized;

        rb.velocity = direction * moveSpeed;

        UpdateSpriteBasedOnDirection(direction);

        float distance = Vector2.Distance(currentPos, targetPos);

        if (distance < nextWP)
        {
            currentWP++;
            if (currentWP >= path.vectorPath.Count)
            {
                rb.velocity = Vector2.zero;
                anim.SetInteger("State", 0);
                hasReachedTarget = true;
            }
        }
    }

    void UpdateSpriteBasedOnDirection(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (angle >= 30 && angle <= 150)
        {
            anim.SetInteger("State", 1);
            headSpriteRenderer.sprite = headTop;
            bodySpriteRenderer.sprite = bodyTop;
        }
        else if (angle >= -150 && angle <= -30)
        {
            anim.SetInteger("State", 1);
            headSpriteRenderer.sprite = headDown;
            bodySpriteRenderer.sprite = bodyDown;
        }
        else if ((angle > 150f && angle <= 180) || (angle < -150f && angle >= -180))
        {
            anim.SetInteger("State", 2);
            headSpriteRenderer.sprite = headRight;
            bodySpriteRenderer.sprite = bodyRight;
            headSpriteRenderer.flipX = false;
            bodySpriteRenderer.flipX = false;
        }
        else
        {
            anim.SetInteger("State", 3);
            headSpriteRenderer.sprite = headRight;
            bodySpriteRenderer.sprite = bodyRight;
            headSpriteRenderer.flipX = true;
            bodySpriteRenderer.flipX = true;
        }
    }

    public void ApplyKnockback(Vector2 direction, float force)
    {
        float knockbackDuration = 0.07f;
        isKnockedBack = true;
        rb.velocity = Vector2.zero;
        rb.AddForce(direction * force * 10f, ForceMode2D.Impulse);
        Invoke("ResetKnockback", knockbackDuration);
    }


    private void ResetKnockback()
    {
        isKnockedBack = false;
        rb.velocity = Vector2.zero;
        moveSpeed = originalMoveSpeed;
    }

    private void HealthSystem_OnDeath(object sender, EventArgs e)
    {
        Vector2 flyDirection = (transform.position - PlayerMoveMent.instance.transform.position).normalized;
        FlyingBody.Create(pfEnemyDeadBody, transform.position, flyDirection);
        Destroy(gameObject);
    }
    private void Attacking()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, PlayerMoveMent.instance.transform.position);
        if (distanceToPlayer < 1f)
        {
            anim.SetTrigger("Attack");
            Vector2 directionToPlayer = (PlayerMoveMent.instance.transform.position - rightHand.position).normalized;
            rightHand.position = Vector2.MoveTowards(rightHand.position, PlayerMoveMent.instance.transform.position, moveSpeed * Time.deltaTime);
            Debug.Log("Attack");

        }
    }
}
