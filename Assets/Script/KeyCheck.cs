using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KeyCheck : MonoBehaviour
{
    public ColorType keyColor;
    [SerializeField] private Material material;
    [SerializeField] private List<GameObject> enemiesInRoom;
    public enum ColorType
    {
        Red,
        Yellow,
        Green,
        Blue
    }

    public float bounceHeight = 0.1f; 
    public float bounceSpeed = 1f; 

    private void Start()
    {
        StartCoroutine(BounceEffect());
    }
    private void Update()
    {
    }
    private IEnumerator BounceEffect()
    {
        Vector3 originalPosition = transform.position;
        float elapsedTime = 0f;

        while (true) 
        {
            elapsedTime += Time.deltaTime * bounceSpeed;
            float yOffset = Mathf.Sin(elapsedTime) * bounceHeight;
            transform.position = originalPosition + new Vector3(0, yOffset, 0);

            yield return null; 
        }
    }
    private IEnumerator ActivateEnemies()
    {
        foreach (GameObject enemy in enemiesInRoom)
        {
            enemy.SetActive(true);
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            enemyController.enabled = false;

            DissolveEffect dissolve = enemy.GetComponent<DissolveEffect>();
            if (dissolve != null)
            {
                dissolve.StopDissolve();
            }
        }
        yield return new WaitForSeconds(1.2f);
        foreach (GameObject enemy in enemiesInRoom)
        {
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                enemyController.enabled = true;
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerInven playerInventory = collision.GetComponent<PlayerInven>();
            playerInventory.PickupKey(keyColor);
            StartCoroutine(ActivateEnemies());
            material.SetFloat("_AlphaSmooth", 1);
            StartCoroutine(DestroyObject());
        }
    }
    private IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(1.2f);
        material.SetFloat("_AlphaSmooth", .1f);
        Destroy(gameObject);

    }
}
