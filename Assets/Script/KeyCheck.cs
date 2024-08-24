using UnityEngine;
using System.Collections;

public class KeyCheck : MonoBehaviour
{
    public ColorType keyColor;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerInven playerInventory = collision.GetComponent<PlayerInven>();
            playerInventory.PickupKey(keyColor);
            Destroy(gameObject);
        }
    }
}
