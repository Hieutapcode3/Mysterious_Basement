using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingBody : MonoBehaviour
{
    public static void Create(Transform prefab,Vector3 spawnPosition, Vector3 flyDirection)
    {
        Transform flyingBodyTransform = Instantiate(prefab,spawnPosition,Quaternion.identity);
        FlyingBody flyingBody = flyingBodyTransform.gameObject.AddComponent<FlyingBody>();
        flyingBody.Setup(flyDirection);
        FindObjectOfType<BloodSystem>().SpawnBloodDeath(spawnPosition,spawnPosition - PlayerMoveMent.instance.transform.position);
    }
    private Vector3 flyDirection;
    private float timer;
    private float eulerZ;
    private void Setup(Vector3 flyDirection)
    {
        this.flyDirection = flyDirection;
        transform.localScale = Vector3.one * 2f;
        eulerZ = 0f;
    }
    private void Update()
    {
        float flySpeed = 20f;
        transform.position += flyDirection * flySpeed * Time.deltaTime;
        transform.localScale += Vector3.one * Time.deltaTime;
        float eulerSpeed = 360f * 4f;
        eulerZ += eulerSpeed * Time.deltaTime;
        transform.localEulerAngles = new Vector3(0,0,eulerZ);
        timer += Time.deltaTime;
        if(timer >= 1f)
        {
            Destroy(gameObject);
        }
    }
}
