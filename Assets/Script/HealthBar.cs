using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private HealthSysytem healthSysytem;
    private Transform bar;

    private void Start()
    {
        Debug.Log("GameObject Name: " + gameObject.name);
        bar = transform.Find("Bar");
    }

    public void SetUp(HealthSysytem healthSysytem)
    {
        this.healthSysytem = healthSysytem;
        healthSysytem.OnHealthChanged += HealthSystem_OnHealthChanged;
    }

    private void HealthSystem_OnHealthChanged(object sender, System.EventArgs e)
    {
        float healthPercent;
        if (gameObject.name.Contains("pfHealthBarEnemy"))
        {
            healthPercent = healthSysytem.GetEnemyhealthPercent();
        }
        else
        {
            healthPercent = healthSysytem.GethealthPercent();
        }

        bar.transform.localScale = new Vector3(healthPercent, bar.transform.localScale.y, 1);
    }
}
