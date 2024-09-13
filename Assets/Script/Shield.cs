using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private List<ShieldTransformer> shieldTransformers;


    private void Update()
    {
        CheckAndDestroyShield();
    }
    public void CheckAndDestroyShield()
    {
        foreach (var shieldTransformer in shieldTransformers)
        {
            if (shieldTransformer.GetLineTrans() != null)
            {
                return;
            }
        }

        Destroy(gameObject);
        Debug.Log("Shield destroyed!");
    }
}
