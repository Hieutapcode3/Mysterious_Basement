using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSystem : MonoBehaviour
{
    [SerializeField] private Sprite[] bloodSprites;  
    [SerializeField] private GameObject bloodPrefab; 
    [SerializeField] private int minBloodCount = 3;  
    [SerializeField] private int maxBloodCount = 5;  

    public void SpawnBlood(Vector2 position, Vector2 direction)
    {
        int bloodCount = Random.Range(minBloodCount, maxBloodCount);  

        for (int i = 0; i < bloodCount; i++)
        {
            GameObject blood = Instantiate(bloodPrefab, position, Quaternion.identity);
            blood.transform.parent = transform;
            SpriteRenderer spriteRenderer = blood.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = bloodSprites[Random.Range(0, bloodSprites.Length)];
            Vector2 randomOffset = Random.insideUnitCircle; 
            blood.transform.position = position + randomOffset;
            blood.transform.up = direction * 5f ; 

        }
    }
    public void SpawnBloodDeath(Vector2 position, Vector2 direction)
    {
        int bloodCount = 50; 

        for (int i = 0; i < bloodCount; i++)
        {
            GameObject blood = Instantiate(bloodPrefab, position, Quaternion.identity);
            blood.transform.parent = transform;
            SpriteRenderer spriteRenderer = blood.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = bloodSprites[Random.Range(0, bloodSprites.Length)];
            float spread = 2f; 
            float randomAngle = Random.Range(-spread, spread); 
            Vector2 randomDirection = RotateVector(direction, randomAngle); 
            float randomDistance = Random.Range(0.0f, 2.0f);
            float randomMovement = Random.Range(-1f, 1f); 

            blood.transform.position = position + randomDirection * randomDistance;
            blood.transform.localScale = Vector3.one * Random.Range(0.5f, 1.5f);

            Vector2 moveOffset = Vector2.Perpendicular(randomDirection) * randomMovement;
            blood.transform.position += (Vector3)moveOffset * Time.deltaTime;
        }
    }

    private Vector2 RotateVector(Vector2 vector, float angle)
    {
        float rad = angle * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);
        return new Vector2(
            cos * vector.x - sin * vector.y,
            sin * vector.x + cos * vector.y
        );
    }


}
