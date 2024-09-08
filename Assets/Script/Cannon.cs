using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Cannon : MonoBehaviour
{
    private bool isPlayerinside;
    [SerializeField] private bool isShow;
    [SerializeField] private Transform posSpawnBullet;
    [SerializeField] private float bulletSpeed;
    private bool isShooting = false;
    private ObjectPool pool;
    public Transform pfhealthBar;
    public Transform healthBarPos;
    public HealthSysytem healthSystem;
    [SerializeField] protected int health;
    private bool isDestroy;

    public Transform Racks;
    [SerializeField] private BoxCollider2D boxHit;
    [SerializeField] private BoxCollider2D col;
    private Animator anim;
    [SerializeField] private GameObject mainCam;
    [SerializeField] private GameObject zoomCam; 

    private void Awake()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        col.enabled = false;
        healthBarPos.gameObject.SetActive(false);
        pool = ObjectPool.instance;
        healthSystem = new HealthSysytem(health);
        Transform healthBarTransform = Instantiate(pfhealthBar, healthBarPos.transform.position, Quaternion.identity);
        healthBarTransform.SetParent(healthBarPos);
        HealthBar healthBar = healthBarTransform.GetComponent<HealthBar>();
        healthBar.SetUp(healthSystem);
        anim.SetBool("CanShoot", true);
    }

    private void Update()
    {
        health = healthSystem.GetHealth();
        if (health <= 0 && !isDestroy)
            DestroyGameObject();
        if (isPlayerinside && !isDestroy )
        {
            RotateToPlayer();
            if(!isShooting )
                StartCoroutine(Shooting());
        }
    }

    private void RotateToPlayer()
    {
        float angle;
        Vector3 playerPos = PlayerMoveMent.instance.transform.position;
        Vector2 lookDir = playerPos - Racks.position;
        angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

        Quaternion rotation = Quaternion.Euler(0, 0, angle + 90f);
        Racks.rotation = rotation;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!isShow)
            {
                StartCoroutine(SwitchCameraAndShowAnimation());
                isShow = true;
            }
            else
            {
                healthBarPos.gameObject.SetActive(true);
                col.enabled = true;
            }
            isPlayerinside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerinside = false;
        }
    }

    private IEnumerator SwitchCameraAndShowAnimation()
    {
        mainCam.SetActive(false);
        isShooting = true;
        Enemy enemyPunch = FindObjectOfType<EnemyPunch>();
        Enemy enemyArcher = FindObjectOfType<EnemyArcher>();
        FindObjectOfType<PlayerMoveMent>().GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        if(enemyArcher != null ) 
            enemyArcher.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        if( enemyPunch != null )
            enemyPunch.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        yield return new WaitForSeconds(2f);
        anim.SetTrigger("Show");
        healthBarPos.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        mainCam.SetActive(true);
        yield return new WaitForSeconds(1f);
        col.enabled = true;
        FindObjectOfType<PlayerMoveMent>().GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        if (enemyArcher != null)
            enemyArcher.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        if (enemyPunch != null)
            enemyPunch.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        StartCoroutine(Shooting());
    }

    private IEnumerator Shooting()
    {
        isShooting = true;
        anim.SetTrigger("Shooting");

        Vector3 playerPos = PlayerMoveMent.instance.transform.position;
        Vector3 adjustedDirection = (playerPos - posSpawnBullet.position).normalized;

        GameObject bullet = pool.SpawnFromPool("Bullet_2", posSpawnBullet.position, Quaternion.identity);
        float angle = Mathf.Atan2(adjustedDirection.y, adjustedDirection.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle);
        StartCoroutine(MoveBullet(bullet, adjustedDirection));

        yield return new WaitForSeconds(0.2f); 
        isShooting = false;
    }

    private IEnumerator MoveBullet(GameObject bullet, Vector3 direction)
    {
        float elapsedTime = 0;
        while (elapsedTime < 1.5f)
        {
            bullet.transform.position += direction * bulletSpeed * Time.unscaledDeltaTime;
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        bullet.SetActive(false);
    }
    private void DestroyGameObject()
    {
        anim.SetBool("CanShoot", false);
        Debug.Log("Destroy");
        anim.SetTrigger("Hide");
        healthBarPos.gameObject.SetActive(false);
        isDestroy =true;
        if (col != null)
        {
            col.enabled = false;
        }
    }
}

