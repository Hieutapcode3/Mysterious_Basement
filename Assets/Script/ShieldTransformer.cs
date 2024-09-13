using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldTransformer : MonoBehaviour
{
    public Transform pfhealthBar;
    public Transform healthBarPos;
    public HealthSysytem healthSystem;
    [SerializeField] protected int health;
    [SerializeField] Sprite ShieldDestroy;
    private SpriteRenderer sprite;
    [SerializeField] private Material defaultMaterial;
    private Transform lineTransformer;


    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        lineTransformer = transform.Find("LineTransformer");
    }

    void Start()
    {
        healthSystem = new HealthSysytem(health);
        Transform healthBarTransform = Instantiate(pfhealthBar, healthBarPos.transform.position, Quaternion.identity);
        healthBarTransform.SetParent(healthBarPos);
        HealthBar healthBar = healthBarTransform.GetComponent<HealthBar>();
        healthBar.SetUp(healthSystem);
    }
    private void Update()
    {
        health = healthSystem.GetHealth();
        ShieldBreak();
    }
    private void ShieldBreak()
    {
        if (health == 0)
        {
            sprite.sprite = ShieldDestroy;
            healthBarPos.gameObject.SetActive(false);
            sprite.material = defaultMaterial;
            if (lineTransformer != null)
            {
                Destroy(lineTransformer.gameObject);
            }
        }
    }
    public int GetHealth()
    {
        return health;
    }
    public Transform GetLineTrans()
    {
        return lineTransformer;
    }
}
