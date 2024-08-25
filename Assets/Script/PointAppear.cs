using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAppear : MonoBehaviour
{
    [SerializeField] private GameObject obj; 
    [SerializeField] private DissolveEffect dissolveEffect;
    [SerializeField] private MonoBehaviour enemyController; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(DissolveAndActivateEnemy());
        }
    }

    IEnumerator DissolveAndActivateEnemy()
    {
        enemyController.enabled = false;
        obj.SetActive(true);
        dissolveEffect.StartDissolve(); 
        yield return new WaitForSeconds(5f); 
        enemyController.enabled = true; 
    }
}
