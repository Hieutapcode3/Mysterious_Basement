using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GunController : MonoBehaviour
{
    public GameObject shootEffect;
    public GameObject bulletShellPrefab;
    public Transform shootStartPoint;
    public float bulletSpeed = 20f;
    public LayerMask hitLayers;
    private float timeBetweenShots = 0.2f;
    private float timeSinceLastShot = 0f;
    private bool canShoot = true;
    [SerializeField] private float bulletMax = 12;
    [SerializeField] private float bulletRemain;

    private Vector3 targetPosition;
    private Vector3 direction;

    private ObjectPool objectPool;
    public GunType gunType;
    [SerializeField] private Text bulletCountTxt;
    [SerializeField] private Sprite pistolSprite;
    [SerializeField] private Sprite shotgunSprite;
    [SerializeField] private Sprite M4Sprite;
    [SerializeField] private Transform[] Gloves;
    [SerializeField] private SpriteRenderer sprite;

    public enum GunType
    {
        Pistol,
        Shotgun,
        M4
    }

    void Start()
    {
        shootEffect.SetActive(false);
        objectPool = ObjectPool.instance;
        bulletRemain = bulletMax;
        gunType = GunType.Pistol;
        UpdateGun();  
    }

    void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && timeSinceLastShot >= timeBetweenShots && canShoot)
        {
            timeSinceLastShot = 0f;
            PlayerMoveMent.instance.ShootingAnim();
            StartCoroutine(ShootEffectCoroutine());
            ShootBullet();
            CreateBulletShell();
        }

        // Switching guns
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            gunType = GunType.Pistol;
            

            UpdateGun();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            gunType = GunType.Shotgun;
            
            UpdateGun();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            gunType = GunType.M4;
            
            UpdateGun();
        }

        UpdateRaycast();

        if (Input.GetKeyDown(KeyCode.R) && bulletRemain != bulletMax)
        {
            StartCoroutine(ReloadBullet());
        }
    }

    private IEnumerator ShootEffectCoroutine()
    {
        shootEffect.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        shootEffect.SetActive(false);
    }

    private void UpdateRaycast()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;
        direction = (mousePosition - shootStartPoint.position).normalized;
        targetPosition = mousePosition;

        RaycastHit2D hit = Physics2D.Raycast(shootStartPoint.position, direction, Vector3.Distance(shootStartPoint.position, mousePosition), hitLayers);

        if (hit.collider != null)
        {
            targetPosition = hit.point;
        }
    }

    private void ShootBullet()
    {
        if (bulletRemain > 0 && canShoot)
        {
            bulletRemain -= 1;
            GameObject bullet = objectPool.SpawnFromPool("Bullet", shootStartPoint.position, Quaternion.identity);
            if (bulletRemain == 0)
                bulletCountTxt.color = Color.red;
            else
                bulletCountTxt.color = Color.white;

            bulletCountTxt.text = bulletRemain.ToString() + "/" + bulletMax;
            bullet.transform.right = direction;
            float distance = Vector3.Distance(shootStartPoint.position, targetPosition);
            StartCoroutine(MoveBullet(bullet, distance));
        }
        else
        {
            StartCoroutine(ReloadBullet());
            bulletRemain = bulletMax;
        }
    }

    private IEnumerator MoveBullet(GameObject bullet, float distance)
    {
        Vector3 startPosition = bullet.transform.position;
        Vector3 endPosition = targetPosition;
        float travelTime = distance / bulletSpeed;
        float elapsedTime = 0;

        while (elapsedTime < travelTime)
        {
            bullet.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / travelTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        bullet.transform.position = endPosition;
        bullet.SetActive(false);
    }

    private void CreateBulletShell()
    {
        GameObject bulletShell = Instantiate(bulletShellPrefab, transform.position, Quaternion.identity);
        Transform bulletShellParent = GameObject.Find("BulletShell").transform;
        bulletShell.transform.SetParent(bulletShellParent);
        bulletShell.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        StartCoroutine(MoveAndRotateBulletShell(bulletShell));
    }

    private IEnumerator MoveAndRotateBulletShell(GameObject bulletShell)
    {
        Vector3 startPosition = bulletShell.transform.position;
        float heightTransformUp = Random.Range(1f, 2f);
        float angle = GunRotate.Instance.angle;
        Vector3 endPosition = startPosition + Vector3.up * heightTransformUp;

        if (angle >= -150 && angle < -90)
        {
            endPosition = startPosition + Vector3.left * heightTransformUp;
        }
        else if (angle >= -90 && angle < -30)
        {
            endPosition = startPosition + Vector3.right * heightTransformUp;
        }

        float durationMove = 0.5f;
        float durationRotate = 1f;
        float elapsedTime = 0;
        float targetRotationZ = Random.Range(0f, 360f);

        while (elapsedTime < durationMove)
        {
            bulletShell.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / durationMove);
            float currentRotationZ = Mathf.Lerp(0f, targetRotationZ, elapsedTime / durationRotate);
            bulletShell.transform.rotation = Quaternion.Euler(0f, 0f, currentRotationZ);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        bulletShell.transform.SetPositionAndRotation(endPosition, Quaternion.Euler(0f, 0f, targetRotationZ));
    }

    private IEnumerator ReloadBullet()
    {
        canShoot = false;
        bulletRemain = bulletMax;
        bulletCountTxt.color = Color.white;
        PlayerMoveMent.instance.ReloadBulletAnim();
        yield return new WaitForSeconds(0.75f);
        bulletCountTxt.text = bulletRemain.ToString() + "/" + bulletMax;
        canShoot = true;
    }

    private void UpdateGun()
    {
        switch (gunType)
        {
            case GunType.Pistol:
                sprite.sprite = pistolSprite;
                Gloves[0].position = new Vector3(-0.108f, -0.006f, 0);
                Gloves[1].position = new Vector3(0.017f, -0.01f, 0);
                Gloves[1].rotation = Quaternion.Euler(0, 0, 0);
                break;
            case GunType.Shotgun:
                sprite.sprite = shotgunSprite;
                Gloves[0].position = new Vector3(-0.165f, 0.49f, 0);
                Gloves[1].position = new Vector3(0.321f, -0.48f, 0);
                Gloves[1].rotation = Quaternion.Euler(0, 0, 110);
                break;
            case GunType.M4:
                sprite.sprite = M4Sprite;
                Gloves[0].position = new Vector3(-0.095f, 0.27f, 0);
                Gloves[1].position = new Vector3(0.262f, -0.615f, 0);
                Gloves[1].rotation = Quaternion.Euler(0, 0, 110);
                break;
        }
    }
}
