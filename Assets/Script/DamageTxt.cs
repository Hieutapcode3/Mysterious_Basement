using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageTxt : MonoBehaviour
{
    private TextMeshPro textMesh;
    private float disappearTimer;
    private Color txtColor;
    private Vector3 moveVec;
    private static int sortingOrder;
    private const float disappear_time_max = 1f;
    private void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
    }
    private void Update()
    {
        transform.position += moveVec * Time.deltaTime;
        moveVec -= moveVec * 8f * Time.deltaTime;
        if(disappearTimer > disappear_time_max * 0.5f)
        {
            float increaseScale = 1f;
            transform.localScale += Vector3.one * increaseScale * Time.deltaTime;
        }
        else
        {
            float decreaseScale = 1f;
            transform.localScale -= Vector3.one * decreaseScale * Time.deltaTime;
        }
        
        disappearTimer -= Time.deltaTime;

        if(disappearTimer < 0)
        {
            float disappearSpeed = 3f;
            txtColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = txtColor;
            if(txtColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void Setup(int damageCount, bool isCriticalHit)
    {
        textMesh.text = damageCount.ToString();
        if (!isCriticalHit)
        {
            textMesh.fontSize = 4f;
            txtColor = new Color(255 / 255f, 240 / 255f, 0 / 255f, 255 / 255f); 
        }
        else
        {
            textMesh.fontSize = 5f;
            txtColor = new Color(255 / 255f, 73 / 255f, 0 / 255f, 255 / 255f); 
        }
        textMesh.color = txtColor;
        disappearTimer = disappear_time_max;
        sortingOrder++;
        textMesh.sortingOrder = sortingOrder;
        moveVec = new Vector3(0.7f, 1) * 10f;
    }


}
