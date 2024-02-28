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
    Vector2 maxMovementVector = new Vector2(0, 0);

    float lerpTimer = 1f;
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
            lerpTimer -= Time.deltaTime;
            lerpTimer = Mathf.Max(lerpTimer, 0);
            movementVector = Vector2.Lerp(Vector2.zero, maxMovementVector, lerpTimer);
            ballRigidbody.velocity = movementVector;

            if (lerpTimer <= 0)
            {
                StopBall();
            }
        }
    }

    public void SpinBall()
    {
        ballAnimator.speed = 1f;
    }
    public void StopBall()
    {
        isMoving = false;
        ballAnimator.speed = 0;
    }
    public void MoveBall(Vector3 newPos)
    {
        this.transform.localPosition = newPos;
    }
    public void LaunchBall(Vector2 newMovement)
    {
        maxMovementVector = newMovement;
        movementVector = newMovement;
        lerpTimer = 1f;
        isMoving = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.tag);
        if (other.tag == "Wall" && isMoving)
        {
            maxMovementVector = Vector2.Reflect(maxMovementVector, other.GetComponent<Wall>().GetInNormal());
            float rotation = Random.Range(-20f, 20f);
            maxMovementVector = Quaternion.AngleAxis(rotation, Vector3.forward) * maxMovementVector;
            if (lerpTimer < .5f)
                lerpTimer = .5f;
        }
    }
}
