using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public float thrustForce = 1f;
    public float maxSpeed = 5f;
    public float scoreMultiplier = 10f;
    public UIDocument uiDoc;
    public GameObject explosionEffect;
    GameObject boosterFlame = null;
    Rigidbody2D rb;
    float elasedTime = 0f;
    float score = 0f;
    Label scoreText;
    Button restartButton;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (boosterFlame == null)
        {
            boosterFlame = GameObject.FindGameObjectWithTag("FLAME");
            boosterFlame.SetActive(false);
        }
        scoreText = uiDoc.rootVisualElement.Q<Label>("ScoreLabel");
        restartButton = uiDoc.rootVisualElement.Q<Button>("RestartButton");
        restartButton.style.display = DisplayStyle.None;
        restartButton.clicked += ReloadScene;
    }

    void Update()
    {
        UpdateScore();
        MovePlayer();
        DisplayFlame();
    }

    void UpdateScore()
    {
        elasedTime += Time.deltaTime;
        score = Mathf.FloorToInt(elasedTime * scoreMultiplier);
        scoreText.text = $"Score: {(int)score:d5}";  
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
        restartButton.style.display = DisplayStyle.Flex;
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(0);//(SceneManager.GetActiveScene().name);
    }
}
