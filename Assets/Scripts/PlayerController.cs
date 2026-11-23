using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public UIMgr uiMgr;
    public float thrustForce = 1f;
    public float maxSpeed = 5f;
    
    public GameObject explosionEffect;
    GameObject boosterFlame = null;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (boosterFlame == null)
        {
            boosterFlame = GameObject.FindGameObjectWithTag("FLAME");
            boosterFlame.SetActive(false);
        }
    }

    void Update()
    {
        MovePlayer();
        DisplayFlame();
    }

    void MovePlayer()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            // 마우스 방향 계산
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(
                Mouse.current.position.value);
            Vector2 direction = (mousePos - transform.position).normalized;

            // 플레이어를 클릭된 방향으로 이동
            transform.up = direction;
            rb.AddForce(direction * thrustForce);
            if (rb.linearVelocity.magnitude > maxSpeed)
            {
                rb.linearVelocity = 
                    rb.linearVelocity.normalized * maxSpeed;
            }
        }
    }

    void DisplayFlame()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (boosterFlame != null)
                boosterFlame.SetActive(true);
        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            if (boosterFlame != null)
                boosterFlame.SetActive(false);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
        Instantiate(explosionEffect, 
                    transform.position,
                    transform.rotation);
        uiMgr.GameOver();
    }
}
