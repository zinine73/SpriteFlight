using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] float minSize = 0.5f;
    [SerializeField] float maxSize = 2.0f;
    [SerializeField] float minSpeed = 50f;
    [SerializeField] float maxSpeed = 150f;
    [SerializeField] float maxSpin = 10f;
    Rigidbody2D rb;
    void Start()
    {
        float randomSize = Random.Range(minSize, maxSize);
        float randomSpeed = Random.Range(minSpeed, maxSpeed) / randomSize;
        transform.localScale = new Vector3(randomSize, randomSize, 1);
        Vector2 randomDirection = Random.insideUnitCircle;
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(randomDirection * randomSpeed);
        float randomTorque = Random.Range(-maxSpin, maxSpin);
        rb.AddTorque(randomTorque);
    }

    void Update()
    {
        
    }
}
