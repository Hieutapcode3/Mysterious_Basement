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
    public void CreateDamageTxt(int damage,Vector3 SpawnPos,bool isCriticalHit)
    {
        Transform damageTransform = Instantiate(pfdamageTxt, SpawnPos, Quaternion.identity);
        Transform DamageTxtParent = GameObject.Find("DamageTxt").transform;
        DamageTxt damageUp = damageTransform.GetComponent<DamageTxt>();
        damageUp.transform.SetParent(DamageTxtParent);
        damageUp.Setup(damage, isCriticalHit);
    }
}
