using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BallContainer : MonoBehaviour
{
    private static BallContainer _instance;
    [FormerlySerializedAs("_ballContainer")] [SerializeField] private List<Ball> ballContainer = new List<Ball>();
    
    public static BallContainer Instance => _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);

        }
        Debug.Log("BallController:", this);
    }

    public void AddBallContainer(Ball ball)
    {
        ballContainer.Add(ball);
    }

    public Ball GetBall(int index)
    {
        return ballContainer[index];
    }

    public void ReMoveBall(Ball ball)
    {
        ballContainer.Remove(ball);
    }

}
