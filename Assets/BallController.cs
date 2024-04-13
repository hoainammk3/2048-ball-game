using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class BallController : MonoBehaviour
{
    private BallContainerPrefabs _ballControllerPrefabs;
    private BallContainer _ballContainer;
    
    private Ball _ballPrefab;
    private Ball _ballSleep = null;

    private bool _isDragging = false;

    private Vector3 _offset;
    private int _maxPower = 1;
    private readonly float _minBorderX = -1.74f;
    private readonly float _maxBorderX = 1.74f;
    
    private float _elapsedTime = 0;
    private readonly float _timer = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        _ballControllerPrefabs = BallContainerPrefabs.Instance;
        _ballContainer = BallContainer.Instance;
        Debug.Log("_ballControllerPrefabs:", _ballControllerPrefabs);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Init();
        Debug.Log("_ballSleep: ", _ballSleep);
        if (_ballSleep)
        {
            if (Input.GetMouseButtonDown(0) && !_isDragging)
            {
                StartDragging();
            }
            else if (Input.GetMouseButtonUp(0) && _isDragging)
            {
                StopDragging();
                _ballSleep.GetComponent<Rigidbody2D>().gravityScale = 1;
                _ballSleep = null;
                _elapsedTime = 0;
            }
            
            if (_isDragging)
            {
                DragObject();
            }
        }
        
        if (_ballSleep == null)
        {
            _elapsedTime += 1 / 30.0f;
        }
    }

    void StartDragging()
    {
        _isDragging = true;
        _offset = _ballSleep.transform.position - GetMouseWorldPosition();
    }

    void StopDragging()
    {
        _isDragging = false;
    }

    void DragObject()
    {
        Vector3 targetPosition = GetMouseWorldPosition() + _offset;
//        Vector3 targetPosition = _ballSleep.transform.position;

        targetPosition.y = _ballSleep.transform.position.y; // Giữ nguyên vị trí trên trục y
        targetPosition.z = _ballSleep.transform.position.z; // Giữ nguyên vị trí trên trục z

        float radiusBall = _ballSleep.transform.gameObject.GetComponent<CircleCollider2D>().radius * _ballSleep.Power *
                           _ballSleep.ScaleDefautl;
        if (targetPosition.x - radiusBall < _minBorderX)
        {
            targetPosition.x = _minBorderX + radiusBall;
        }

        if (targetPosition.x + radiusBall > _maxBorderX)
        {
            targetPosition.x = _maxBorderX - radiusBall;
        }
        _ballSleep.transform.position = targetPosition;
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane; // Set z distance to the camera
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
    
    
    int RandomPower()
    {
        if (_maxPower < 3) 
            return (int) Random.Range(1f, 3.5f);
        
        if (_maxPower < 9) 
            return (int) Random.Range(1f, _maxPower - 3f);
        
        return (int) Random.Range(1f, 7f);
    }
    
    void Init()
    {
        if (_ballSleep || _elapsedTime < _timer) return;
        int power = RandomPower();
        Vector3 position = new Vector3(0, 2f, 0);
        _ballPrefab = _ballControllerPrefabs.GetBallPrefab(power);
        
        _ballSleep = Instantiate(_ballPrefab, position, Quaternion.identity);
        Debug.Log(_ballSleep);
        _ballSleep.gameObject.SetActive(true);
        _ballSleep.GetComponent<Rigidbody2D>().gravityScale = 0;
        _ballContainer.AddBallContainer(_ballSleep);
    }

}
