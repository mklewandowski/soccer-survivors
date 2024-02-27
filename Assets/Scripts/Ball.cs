using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ball : MonoBehaviour
{
    Animator ballAnimator;
    Rigidbody2D ballRigidbody;
    bool isMoving = false;
    Vector2 movementVector = new Vector2(0, 0);
    // Start is called before the first frame update
    void Start()
    {
        ballAnimator = this.GetComponent<Animator>();
        ballRigidbody = this.GetComponent<Rigidbody2D>();
        StopBall();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            ballRigidbody.velocity = movementVector;
        }

    }

    public void SpinBall()
    {
        ballAnimator.speed = 1f;
    }
    public void StopBall()
    {
        ballAnimator.speed = 0;
    }
    public void MoveBall(Vector3 newPos)
    {
        this.transform.localPosition = newPos;
    }
    public void LaunchBall(Vector2 newMovement)
    {
        movementVector = newMovement;
        isMoving = true;
    }
}
