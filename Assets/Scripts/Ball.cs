using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ball : MonoBehaviour
{
    Animator ballAnimator;
    // Start is called before the first frame update
    void Start()
    {
        ballAnimator = this.GetComponent<Animator>();
        StopBall();
    }

    // Update is called once per frame
    void Update()
    {

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
}
