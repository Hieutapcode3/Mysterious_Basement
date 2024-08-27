using UnityEngine;

public class PlayerMoveMent : MonoBehaviour
{
    [SerializeField] private float m_speed = 2f;

    [SerializeField] private Sprite headTop;
    [SerializeField] private Sprite bodyTop;
    [SerializeField] private Sprite headRight;
    [SerializeField] private Sprite bodyRight;
    [SerializeField] private Sprite headDown;
    [SerializeField] private Sprite bodyDown;

    [SerializeField] private GameObject headObject; 
    [SerializeField] private GameObject bodyObject;
    public Transform pfhealthBar;
    public Transform healthBarPos;
    private HealthSysytem healthSystem;
    public static PlayerMoveMent instance;

    private Rigidbody2D rb;
    private Animator anim;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        if(instance == null )
            instance = this;
    }
    void Start()
    {
        healthSystem = new HealthSysytem(100);
        Transform healthBarTransform = Instantiate(pfhealthBar, healthBarPos.transform.position, Quaternion.identity);
        healthBarTransform.SetParent(healthBarPos);
        HealthBar healthBar = healthBarTransform.GetComponent<HealthBar>();
        healthBar.SetUp(healthSystem);
    }

    void Update()
    {
        Movement();
        ChangeSprite(GunRotate.Instance.angle);
    }

    private void Movement()
    {
        float dirX = Input.GetAxisRaw("Horizontal");
        float dirY = Input.GetAxisRaw("Vertical");
        Vector2 direction = new Vector2(dirX, dirY).normalized; 
        rb.velocity = direction * m_speed;
        if (dirX != 0 && dirY != 0)
        {
            anim.SetInteger("State", 3);
        }
        else if (dirX == 0 && dirY != 0)
        {
            anim.SetInteger("State", 3);
        }
        else if (dirX != 0 && dirY == 0)
        {
            if (dirX > 0)
                anim.SetInteger("State", 2);
            else
                anim.SetInteger("State", 1);
        }
        else
        {
            anim.SetInteger("State", 0);
        }
    }

    private void ChangeSprite(float angle)
    {
        if (angle >= 30f && angle <= 150f)
        {
            bodyObject.GetComponent<SpriteRenderer>().sprite = bodyTop;
            headObject.GetComponent<SpriteRenderer>().sprite = headTop;
        }
        else if (angle >= -150f && angle <= -30f)
        {
            bodyObject.GetComponent<SpriteRenderer>().sprite = bodyDown;
            headObject.GetComponent<SpriteRenderer>().sprite = headDown;
        }
        else if((angle > 150f && angle <= 180) || (angle < -150f && angle >= -180))
        {
            bodyObject.GetComponent<SpriteRenderer>().sprite = bodyRight;
            headObject.GetComponent<SpriteRenderer>().sprite = headRight;
            bodyObject.GetComponent<SpriteRenderer>().flipX = true;
            headObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            bodyObject.GetComponent<SpriteRenderer>().sprite = bodyRight;
            headObject.GetComponent<SpriteRenderer>().sprite = headRight;
            bodyObject.GetComponent<SpriteRenderer>().flipX = false;
            headObject.GetComponent<SpriteRenderer>().flipX = false;
        }
    }
    public void ShootingAnim()
    {
        anim.SetTrigger("Shooting");
    }
    public void ReloadBulletAnim()
    {
        anim.SetTrigger("Reload");
    }
    public void TeleportTo(Transform transformPos)
    {
        gameObject.transform.position = transformPos.position;
    }
}
