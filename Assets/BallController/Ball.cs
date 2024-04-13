using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private BallContainerPrefabs _ballControllerPrefabs;
    private BallContainer _ballContainer;
    
    public static float lastExplosiveTime;
    public static float currentTime;
    [SerializeField] private int power;
    private int value;
    private Rigidbody2D rb;

    private float timerDelayExplosive = 0.5f;
    private float scaleDefautl = 0.01f;
    
    public int Power => power;

    public float ScaleDefautl => scaleDefautl;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Explosive()
    {
        lastExplosiveTime = Time.time;
        float radius = transform.localScale.x;
        float attractionForce = 0.3f;
        // Tạo một vùng cầu để xác định các đối tượng ở gần
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius * 1.5f );

        // Lặp qua từng đối tượng ở gần
        foreach (Collider col in colliders)
        {
            // Kiểm tra nếu đối tượng có Rigidbody
            Rigidbody2D rb = col.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Tính toán hướng của lực
                Vector3 direction = transform.position - col.transform.position;

                // Áp dụng lực theo hướng này
                rb.AddForce(direction.normalized * attractionForce * Time.deltaTime);
            }
        }
    }
    
    void OnCollisionEnter2D (Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Ball")) return;
        
        if (currentTime - lastExplosiveTime < timerDelayExplosive) return;
        
        Ball other = collision.gameObject.GetComponent<Ball>();
        if (other.Power == this.Power)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            Destroy(other.gameObject);
            this.Transfer();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Ball")) return;
        
        if (currentTime - lastExplosiveTime < timerDelayExplosive) return;
        
        Ball other = collision.gameObject.GetComponent<Ball>();
        if (other.Power == this.Power)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            Destroy(other.gameObject);
            this.Transfer();
        }
    }


    void FixedUpdate()
    {
        currentTime = Time.time;
    }

    private void OnDestroy()
    {
        Explosive();
    }

    void Transfer()
    {
        
    }
}
