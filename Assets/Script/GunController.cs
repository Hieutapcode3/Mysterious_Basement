using System.Collections;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public GameObject shootEffect;
    public GameObject bulletShellPrefab;
    public Transform shootStartPoint;
    public float bulletSpeed = 20f;
    public LayerMask hitLayers;

    private Vector3 targetPosition;
    private Vector3 direction;

    private ObjectPool objectPool;

    void Start()
    {
        shootEffect.SetActive(false);
        objectPool = ObjectPool.instance;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(ShootEffectCoroutine());
            ShootBullet();
            CreateBulletShell();
        }

        UpdateRaycast();
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
        GameObject bullet = objectPool.SpawnFromPool("Bullet", shootStartPoint.position, Quaternion.identity);
        bullet.transform.right = direction;

        float distance = Vector3.Distance(shootStartPoint.position, targetPosition);
        StartCoroutine(MoveBullet(bullet, distance));
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
        float heightTransformUp = Random.Range(1f,2f);
        float angle = GunRotate.Instance.angle;
        Vector3 endPosition = startPosition + Vector3.up * heightTransformUp;
        if(angle >= -150 && angle < -90)
        {
             endPosition = startPosition + Vector3.left * heightTransformUp;
        }
        else if(angle >= -90 && angle < -30)
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

}



//private void OnDrawGizmos()
//{
//    if (shootStartPoint != null)
//    {
//        Gizmos.color = Color.red;
//        Gizmos.DrawLine(shootStartPoint.position, targetPosition);
//    }
//}
