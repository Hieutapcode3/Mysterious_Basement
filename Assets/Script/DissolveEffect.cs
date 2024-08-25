using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveEffect : MonoBehaviour
{
    [SerializeField] private Material material;
    private float dissolveAmount;
    private bool isDissolving;

    private void Update()
    {
        if (isDissolving)
        {
            dissolveAmount = Mathf.Clamp01(dissolveAmount  + Time.deltaTime);
            material.SetFloat("_DissolveAmount", dissolveAmount);
        }
        else
        {
            dissolveAmount = Mathf.Clamp01(dissolveAmount  - Time.deltaTime);
            material.SetFloat("_DissolveAmount", dissolveAmount);
        }
    }
    public void StartDissolve( )
    {
        isDissolving = true;
    }public void StopDissolve()
    {
        isDissolving = false ;
    }

}
