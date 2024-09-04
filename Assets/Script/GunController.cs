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

    [SerializeField] private float pistolMaxBullets = 12;
    [SerializeField] private float shotgunMaxBullets = 4;
    [SerializeField] private float M4MaxBullets = 25;
    private float pistolBulletsRemain;
    private float shotgunBulletsRemain;
    private float M4BulletsRemain;
    public bool hasShotGun;
    public bool hasM4;
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
    public static GunController instance;

    public enum GunType
    {
        Pistol,
        Shotgun,
        M4
    }
    private void Awake()
    {
        if(instance == null)
            instance = this;
    }

    void Start()
    {
        shootEffect.SetActive(false);
        objectPool = ObjectPool.instance;
        pistolBulletsRemain = pistolMaxBullets;
        shotgunBulletsRemain = shotgunMaxBullets;
        M4BulletsRemain = M4MaxBullets;
        gunType = GunType.Pistol;
        UpdateGun();
    }

    void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        if (gunType == GunType.M4 && Input.GetMouseButton(0) && timeSinceLastShot >= timeBetweenShots && canShoot)
        {
            timeSinceLastShot = 0f;
            PlayerMoveMent.instance.ShootingAnim();
            StartCoroutine(ShootEffectCoroutine());
            ShootBullet();
            CreateBulletShell();
        }
        // Kiểm tra nếu loại súng không phải là M4 và nhấn chuột (MouseDown)
        else if (Input.GetMouseButtonDown(0) && timeSinceLastShot >= timeBetweenShots && canShoot)
        {
            timeSinceLastShot = 0f;
            PlayerMoveMent.instance.ShootingAnim();
            StartCoroutine(ShootEffectCoroutine());
            ShootBullet();
            CreateBulletShell();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            gunType = GunType.Pistol;
            UpdateGun();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && hasShotGun)
        {
            gunType = GunType.Shotgun;
            UpdateGun();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && hasM4)
        {
            gunType = GunType.M4;
            UpdateGun();
        }

        UpdateRaycast();

        if (Input.GetKeyDown(KeyCode.R))
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
        //if (gunType == GunType.M4)
        //{
        //    float randomOffset = Random.Range(-1f, 1f);
        //    mousePosition += new Vector3(randomOffset, randomOffset,0);
        //}
        mousePosition.z = 0f;
        targetPosition = mousePosition;
        direction = (targetPosition - shootStartPoint.position).normalized;

        RaycastHit2D hit = Physics2D.Raycast(shootStartPoint.position, direction, Vector3.Distance(shootStartPoint.position, mousePosition), hitLayers);

        if (hit.collider != null)
        {
            targetPosition = hit.point;
        }
    }

    private void ShootBullet()
    {
        if (canShoot)
        {
            bool outOfAmmo = false;

            if (gunType == GunType.Pistol && pistolBulletsRemain > 0)
            {
                pistolBulletsRemain -= 1;
                UpdateColorTxt(pistolBulletsRemain);
            }
            else if (gunType == GunType.Shotgun && shotgunBulletsRemain > 0)
            {
                shotgunBulletsRemain -= 1;
                UpdateColorTxt(shotgunBulletsRemain);

                int bulletCount = 3;
                Vector3[] bulletOffsets = new Vector3[] {
                    Vector3.zero,
                    new Vector3(0f, .1f, 0),
                    new Vector3(0, -.2f, 0) 
                };

                for (int i = 0; i < bulletCount; i++)
                {
                    GameObject bullet = objectPool.SpawnFromPool("Bullet", shootStartPoint.position, Quaternion.identity);
                    targetPosition += bulletOffsets[i] * 10 ;
                    bullet.transform.right =  direction + bulletOffsets[i];
                    float distance = Vector3.Distance(shootStartPoint.position, targetPosition);

                    StartCoroutine(MoveBullet(bullet, distance, targetPosition));
                }
            }
            else if (gunType == GunType.M4 && M4BulletsRemain > 0)
            {
                M4BulletsRemain -= 1;
                UpdateColorTxt(M4BulletsRemain);
            }
            else
            {
                outOfAmmo = true;
                StartCoroutine(ReloadBullet());
                return;
            }

            if (!outOfAmmo && gunType != GunType.Shotgun)
            {
                Vector3 adjustedDirection;
                if (gunType == GunType.M4)
                    adjustedDirection = direction + new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0);
                else
                    adjustedDirection = direction;
                adjustedDirection.Normalize();
                GameObject bullet = objectPool.SpawnFromPool("Bullet", shootStartPoint.position, Quaternion.identity);
                bullet.transform.right = adjustedDirection;
                float distance = Vector3.Distance(shootStartPoint.position, targetPosition);
                StartCoroutine(MoveBullet(bullet, distance, targetPosition));
            }

            UpdateBulletCountText();
        }
    }




    private IEnumerator MoveBullet(GameObject bullet, float distance,Vector3 target)
    {
        Vector3 startPosition = bullet.transform.position;
        Vector3 endPosition = target;
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
        bulletCountTxt.color = Color.white;
        Vector3 originalPosition = Gloves[1].localPosition;
        Vector3 targetPosition = Vector3.zero;
        if (gunType == GunType.Pistol && pistolBulletsRemain != pistolMaxBullets)
        {
            targetPosition = new Vector3(-0.643f, 0.982f, 0);
            pistolBulletsRemain = pistolMaxBullets;
            PlayerMoveMent.instance.ReloadBulletAnim(gunType);
        }
        else if (gunType == GunType.Shotgun && shotgunBulletsRemain != shotgunMaxBullets)
        {
            targetPosition = new Vector3(0.306f, -0.53f, 0);
            shotgunBulletsRemain = shotgunMaxBullets;
            PlayerMoveMent.instance.ReloadBulletAnim(gunType);
        }
        else if (gunType == GunType.M4 && M4BulletsRemain != M4MaxBullets)
        {
            targetPosition = new Vector3(-0.48f, 0.427f, 0);
            M4BulletsRemain = M4MaxBullets;
            PlayerMoveMent.instance.ReloadBulletAnim(gunType);

        }
        else
        {
            canShoot = true;
            yield break;
        }
        float duration = 0.3f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            Gloves[1].localPosition = Vector3.Lerp(originalPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Gloves[1].localPosition = targetPosition;

        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            Gloves[1].localPosition = Vector3.Lerp(targetPosition, originalPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Gloves[1].localPosition = originalPosition;
        yield return new WaitForSeconds(0.5f);
        UpdateBulletCountText();
        canShoot = true;
    }

    private void UpdateGun()
    {
        if (gunType == GunType.Pistol)
        {
            sprite.sprite = pistolSprite;
            Gloves[0].localPosition = new Vector3(-0.108f, -0.006f, 0);
            Gloves[1].localPosition = new Vector3(0.017f, -0.01f, 0);
            Gloves[1].localRotation = Quaternion.Euler(0, 0, 0);
            timeBetweenShots = 0.2f;
            UpdateColorTxt(pistolBulletsRemain);
        }
        else if (gunType == GunType.Shotgun)
        {
            sprite.sprite = shotgunSprite;
            Gloves[0].localPosition = new Vector3(-0.165f, 0.49f, 0);
            Gloves[1].localPosition = new Vector3(0.464f, -0.782f, 0);
            Gloves[1].localRotation = Quaternion.Euler(0, 0, 110);
            timeBetweenShots = 0.2f;
            UpdateColorTxt(shotgunBulletsRemain);
        }
        else if (gunType == GunType.M4)
        {
            sprite.sprite = M4Sprite;
            Gloves[0].localPosition = new Vector3(-0.095f, 0.27f, 0);
            Gloves[1].localPosition = new Vector3(0.262f, -0.615f, 0);
            Gloves[1].localRotation = Quaternion.Euler(0, 0, 110);
            timeBetweenShots = 0.1f;
            UpdateColorTxt(M4BulletsRemain);
        }

        UpdateBulletCountText();
    }

    private void UpdateBulletCountText()
    {
        if (gunType == GunType.Pistol)
        {
            bulletCountTxt.text = pistolBulletsRemain.ToString() + "/" + pistolMaxBullets;
        }
        else if (gunType == GunType.Shotgun)
        {
            bulletCountTxt.text = shotgunBulletsRemain.ToString() + "/" + shotgunMaxBullets;
        }
        else if (gunType == GunType.M4)
        {
            bulletCountTxt.text = M4BulletsRemain.ToString() + "/" + M4MaxBullets;
        }
    }
    private void UpdateColorTxt(float bulletRemain)
    {
        if (bulletRemain == 0)
        {
            bulletCountTxt.color = Color.red;
        }
        else
        {
            bulletCountTxt.color = Color.white;
        }
    }
}

