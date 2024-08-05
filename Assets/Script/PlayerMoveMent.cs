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

    private Rigidbody2D rb;
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Movement();
        ChangeSprite(GunRotate.Instance.angle);
        if(Input.GetMouseButtonDown(0))
        {
            Shooting();
        }
    }

    private void Movement()
    {
        float dirX = Input.GetAxisRaw("Horizontal");
        float dirY = Input.GetAxisRaw("Vertical");
        rb.velocity = new Vector2(dirX * m_speed, dirY * m_speed);
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
    private void Shooting()
    {
        anim.SetTrigger("Shooting");
    }
}
