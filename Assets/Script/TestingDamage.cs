using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingDamage : MonoBehaviour
{
    [SerializeField] private Transform pfdamageTxt;
    public static TestingDamage Instance;

    private void Awake()
    {
        Instance = this;
    }
    public void CreateDamageTxt(Vector3 SpawnPos,bool isCriticalHit)
    {
        Transform damageTransform = Instantiate(pfdamageTxt, SpawnPos, Quaternion.identity);
        DamageTxt damageUp = damageTransform.GetComponent<DamageTxt>();
        damageUp.Setup(Random.Range(100,200), isCriticalHit);
    }
}
