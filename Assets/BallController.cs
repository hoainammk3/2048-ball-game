using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class BallController : MonoBehaviour
{
    [FormerlySerializedAs("ballDefinePrefab")] [SerializeField] private Ball ballPrefab;

    [SerializeField] private GameObject ballController;
    private Ball _ballSleep = null; 
    
    private int maxPower = 1;

    private bool isDragging = false;

    private Vector3 offset;

    private float minBorderX = -1.74f;
    private float maxBorderX = 1.74f;
    private float elapTime = 0;

    private float timer = 1.5f;

    private void Reset()
    {
        ballController = GameObject.Find("BallController");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Init();
        if (_ballSleep != null)
        {
            if (Input.GetMouseButtonDown(0) && !isDragging)
            {
                StartDragging();
            }
            else if (Input.GetMouseButtonUp(0) && isDragging)
            {
                StopDragging();
                _ballSleep.GetComponent<Rigidbody2D>().gravityScale = 1;
                _ballSleep = null;
                elapTime = 0;
            }
            
            if (isDragging)
            {
                DragObject();
            }
        }
        
        if (_ballSleep == null)
        {
            elapTime += 1 / 30.0f;
        }
    }

    void StartDragging()
    {
        isDragging = true;
        offset = _ballSleep.transform.position - GetMouseWorldPosition();
    }

    void StopDragging()
    {
        isDragging = false;
    }

    void DragObject()
    {
        Vector3 targetPosition = GetMouseWorldPosition() + offset;
//        Vector3 targetPosition = _ballSleep.transform.position;

        targetPosition.y = _ballSleep.transform.position.y; // Giữ nguyên vị trí trên trục y
        targetPosition.z = _ballSleep.transform.position.z; // Giữ nguyên vị trí trên trục z

        float radiusBall = _ballSleep.transform.gameObject.GetComponent<CircleCollider2D>().radius * _ballSleep.Power *
                           _ballSleep.ScaleDefautl;
        if (targetPosition.x - radiusBall < minBorderX)
        {
            targetPosition.x = minBorderX + radiusBall;
        }

        if (targetPosition.x + radiusBall > maxBorderX)
        {
            targetPosition.x = maxBorderX - radiusBall;
        }
        _ballSleep.transform.position = targetPosition;
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane; // Set z distance to the camera
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
    
    
    int randomPower()
    {
        if (maxPower < 3) 
            return (int) Random.Range(1f, 3.5f);
        
        if (maxPower < 9) 
            return (int) Random.Range(1f, maxPower - 3f);
        
        return (int) Random.Range(1f, 7f);
    }
    
    void Init()
    {
        if (_ballSleep != null || elapTime < timer) return;
        int _power = randomPower();
        Vector3 position = new Vector3(0, 2f, 0);

        switch (_power)
        {
            case 1:
                _ballSleep = Instantiate(ballPrefab, position, Quaternion.identity);
        }
        _ballSleep = Instantiate(ballPrefab, position, Quaternion.identity);
        
        _ballSleep.gameObject.SetActive(true);
        _ballSleep.SetPower(randomPower());
        _ballSleep.UpdateSizeAndValue();
        _ballSleep.GetComponent<Rigidbody2D>().gravityScale = 0;
    }

    
    
}
