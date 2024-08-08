using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyMoveMent : MonoBehaviour
{
    public float moveSpeed;
    public Seeker seeker;
    public float nextWP = 2f;
    private int currentWP = 0;
    private bool hasReachedTarget = false;
    Path path;
    public Transform Target;

    [SerializeField] private Sprite headTop;
    [SerializeField] private Sprite bodyTop;
    [SerializeField] private Sprite headRight;
    [SerializeField] private Sprite bodyRight;
    [SerializeField] private Sprite headDown;
    [SerializeField] private Sprite bodyDown;

    [SerializeField] private GameObject headObject;
    [SerializeField] private GameObject bodyObject;

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
        Target = FindObjectOfType<PlayerMoveMent>().gameObject.transform;
        headSpriteRenderer = headObject.GetComponent<SpriteRenderer>();
        bodySpriteRenderer = bodyObject.GetComponent<SpriteRenderer>();
        InvokeRepeating("CalculatePath", 0, 0.5f);
    }

    void Update()
    {
        if (path != null && !hasReachedTarget && currentWP < path.vectorPath.Count)
        {
            MoveToTarget();
            Debug.Log("moving");
        }

        float distanceToTarget = Vector2.Distance(transform.position, Target.position);
        if (hasReachedTarget && distanceToTarget > nextWP)
        {
            hasReachedTarget = false;
        }
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
}
