using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveEffect : MonoBehaviour
{
    [SerializeField] private Material material;
    private float dissolveAmount;
    private bool isDissolving;
    [SerializeField] private bool isEnemy;

    private void Start()
    {
        dissolveAmount = isEnemy ? 1f : 0f;
        material.SetFloat("_DissolveAmount", dissolveAmount);
    }

    private void Update()
    {
        if (isDissolving)
        {
            dissolveAmount = Mathf.Clamp01(dissolveAmount + Time.deltaTime);
            material.SetFloat("_DissolveAmount", dissolveAmount);
        }
        else
        {
            dissolveAmount = Mathf.Clamp01(dissolveAmount - Time.deltaTime);
            material.SetFloat("_DissolveAmount", dissolveAmount);
        }
    }

    public void StartDissolve()
    {
        isDissolving = true;
    }

    public void StopDissolve()
    {
        isDissolving = false;
    }
}
