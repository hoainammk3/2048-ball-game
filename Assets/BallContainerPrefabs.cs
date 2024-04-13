using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class BallContainerPrefabs : MonoBehaviour
{
    public static float lastExplosiveTime;
    public static float currentTime;
    private static BallContainerPrefabs _instance;
    [SerializeField] private List<Ball> ballContainerPrefabs = new List<Ball>();

    public static BallContainerPrefabs Instance => _instance;

    void Reset()
    {
        for (var i = 0; i < transform.childCount; i++)
        {
            ballContainerPrefabs.Add(transform.GetChild(i).GetComponent<Ball>());
        }
    }

    private void Awake()
    {
        Debug.Log(gameObject);
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void FixedUpdate()
    {
        currentTime = Time.time;
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Ball GetBallPrefab(int power)
    {
        return ballContainerPrefabs[power];
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
}
