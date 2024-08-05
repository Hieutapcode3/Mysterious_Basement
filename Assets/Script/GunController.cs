using System.Collections;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public GameObject shootEffect;
    public GameObject bulletEffectPrefab;
    public Transform shootStartPoint;
    public float bulletSpeed = 20f;
    public LayerMask hitLayers;

    private Vector3 targetPosition;
    private Vector3 direction;

    void Start()
    {
        shootEffect.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(ShootEffectCoroutine());
            StartCoroutine(TriggerBulletEffect());
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
    private IEnumerator TriggerBulletEffect()
    {
        float distance = Vector3.Distance(shootStartPoint.position, targetPosition);
        bulletEffectPrefab.transform.position = shootStartPoint.transform.position;
        bulletEffectPrefab.SetActive(true);
        bulletEffectPrefab.transform.localScale = new Vector3(bulletEffectPrefab.transform.localScale.x, distance, bulletEffectPrefab.transform.localScale.z);
        yield return new WaitForSeconds(0.1f);
        bulletEffectPrefab.SetActive(false);
    }
    //private void OnDrawGizmos()
    //{
    //    if (shootStartPoint != null)
    //    {
    //        Gizmos.color = Color.red;
    //        Gizmos.DrawLine(shootStartPoint.position, targetPosition);
    //    }
    //}
}
