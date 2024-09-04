using System.Collections;
using System.Collections.Generic;
using UnityEngine;  
using Pathfinding;
using System;


public class EnemyPunch : Enemy
{
    public float moveSpeed;
    private float originalMoveSpeed;
    public Seeker seeker;
    public float nextWP = 2f;
    private int currentWP = 0;
    public bool hasReachedTarget = false;
    Path path;
    public Transform Target;
    [SerializeField] private Transform rightHand;
    private Animator anim;

    protected override void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
    }

    protected override void Start()
    {
        originalMoveSpeed = moveSpeed;
        Target = FindObjectOfType<PlayerMoveMent>().gameObject.transform;
        InvokeRepeating("CalculatePath", 0, 0.5f);
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
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
            bodyObject.GetComponent<SpriteRenderer>().sprite = bodyTop;
            headObject.GetComponent<SpriteRenderer>().sprite = headTop;
        }
        else if (angle >= -150 && angle <= -30)
        {
            anim.SetInteger("State", 1);
            bodyObject.GetComponent<SpriteRenderer>().sprite = bodyDown;
            headObject.GetComponent<SpriteRenderer>().sprite = headDown;
        }
        else if ((angle > 150f && angle <= 180) || (angle < -150f && angle >= -180))
        {
            anim.SetInteger("State", 2);
            bodyObject.GetComponent<SpriteRenderer>().sprite = bodyRight;
            headObject.GetComponent<SpriteRenderer>().sprite = headRight;
            bodyObject.GetComponent<SpriteRenderer>().flipX = false;
            headObject.GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            anim.SetInteger("State", 3);
            bodyObject.GetComponent<SpriteRenderer>().sprite = bodyRight;
            headObject.GetComponent<SpriteRenderer>().sprite = headRight;
            bodyObject.GetComponent<SpriteRenderer>().flipX = true;
            headObject.GetComponent<SpriteRenderer>().flipX = true;
        }
    }
    private void Attacking()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, PlayerMoveMent.instance.transform.position);
        if (distanceToPlayer < 1f)
        {
            anim.SetTrigger("Attack");
        }
    }






}
