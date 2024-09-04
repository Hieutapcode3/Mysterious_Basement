using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArcher : Enemy
{
    public float angle;
    public Transform firePoint;
    public float startingAngle = 15f;
    public float convergenceTime = 2f;
    public LayerMask playerLayer;
    public Transform Hand;
    private float currentAngle;
    private bool isPlayerInRange = false;
    public LineRenderer lineRenderer1; 
    public LineRenderer lineRenderer2;
    [SerializeField] private BoxCollider2D boxHit;


    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
        currentAngle = startingAngle;
    }

    protected override void Update()
    {
        base.Update();
        UpdateSprite();
        if (isPlayerInRange)
        {
            RotateToPlayer();
            DrawRaycasts();
        }
    }

    private void RotateToPlayer()
    {
        Vector3 playerPos = PlayerMoveMent.instance.transform.position;
        Vector2 lookDir = playerPos - Hand.position;
        angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        Hand.rotation = rotation;

        if (Hand.eulerAngles.z > 90 && Hand.eulerAngles.z < 270)
        {
            Hand.localScale = new Vector3(Hand.localScale.x, -Mathf.Abs(Hand.localScale.y), Hand.localScale.z);
        }
        else
        {
            Hand.localScale = new Vector3(Hand.localScale.x, Mathf.Abs(Hand.localScale.y), Hand.localScale.z);
        }
    }

    private IEnumerator CastRaycastsRoutine()
    {
        while (isPlayerInRange)
        {
            float elapsedTime = 0f;
            while (elapsedTime < convergenceTime)
            {
                float t = elapsedTime / convergenceTime;
                currentAngle = Mathf.Lerp(startingAngle, 0f, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            base.DamageAttack();
            yield return new WaitForSeconds(1f);

            elapsedTime = 0f;
            while (elapsedTime < convergenceTime)
            {
                float t = elapsedTime / convergenceTime;
                currentAngle = Mathf.Lerp(0f, startingAngle, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
    private void DrawRaycasts()
    {
        Vector3 playerPos = PlayerMoveMent.instance.transform.position;
        float distanceToPlayer = Vector2.Distance(firePoint.position, playerPos);
        Vector2 direction1 = Quaternion.Euler(0, 0, currentAngle) * Hand.right;
        Vector2 direction2 = Quaternion.Euler(0, 0, -currentAngle) * Hand.right;
        lineRenderer1.SetPosition(0, firePoint.position);
        lineRenderer1.SetPosition(1, (Vector2)firePoint.position + direction1 * distanceToPlayer);

        lineRenderer2.SetPosition(0, firePoint.position);
        lineRenderer2.SetPosition(1, (Vector2)firePoint.position + direction2 * distanceToPlayer);
    }

    private void UpdateSprite()
    {
        if (angle >= 30f && angle <= 150f)
        {
            bodyObject.GetComponent<SpriteRenderer>().sprite = bodyTop;
            headObject.GetComponent<SpriteRenderer>().sprite = headTop;
        }
        else if (angle >= -150f && angle <= -30f)
        {
            bodyObject.GetComponent<SpriteRenderer>().sprite = bodyDown;
            headObject.GetComponent<SpriteRenderer>().sprite = headDown;
        }
        else if ((angle > 150f && angle <= 180) || (angle < -150f && angle >= -180))
        {
            bodyObject.GetComponent<SpriteRenderer>().sprite = bodyRight;
            headObject.GetComponent<SpriteRenderer>().sprite = headRight;
            bodyObject.GetComponent<SpriteRenderer>().flipX = true;
            headObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            bodyObject.GetComponent<SpriteRenderer>().sprite = bodyRight;
            headObject.GetComponent<SpriteRenderer>().sprite = headRight;
            bodyObject.GetComponent<SpriteRenderer>().flipX = false;
            headObject.GetComponent<SpriteRenderer>().flipX = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player")){
            Debug.Log("Player in side");
            isPlayerInRange = true;
            StartCoroutine(CastRaycastsRoutine());
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }


}
