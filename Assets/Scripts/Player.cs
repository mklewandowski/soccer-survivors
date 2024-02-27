using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    AudioManager audioManager;

    enum PlayerOrientation {
        Up,
        UpRight,
        UpLeft,
        Down,
        DownRight,
        DownLeft,
        Left,
        Right
    };
    PlayerOrientation currentPlayerOrientation = PlayerOrientation.Right;

    [SerializeField]
    GameObject PlayerGO;

    float moveSpeed = 2.4f;
    float moveSpeedInitial = 2.4f;
    Vector2 movementVector = new Vector2(0, 0);
    bool moveLeft;
    bool moveRight;
    bool moveUp;
    bool moveDown;
    private Rigidbody2D playerRigidbody;
    bool isMoving = false;
    Animator playerAnimator;
    float dustTimer = 0f;
    float dustTimerMax = .1f;

    bool isAlive = true;
    float health = 20f;
    float invincibleTimer = 0f;
    float invincibleTimerMax = .6f;
    private SpriteRenderer playerRenderer;
    private BoxCollider2D playerCollider;
    [SerializeField]
    Material FlashMaterial;
    Material playerMaterial;
    float flashTimer = 0f;
    float flashTimerMax = .15f;
    [SerializeField]
    GameObject HealthBar;
    [SerializeField]
    GameObject HealthBarBack;

	float controllerLeftStickX;
	float controllerLeftStickY;

    Ball ball;
    float ballOffsetY = -.5f;
    float ballOffsetX = .6f;

    // Start is called before the first frame update
    void Start()
    {
        GameObject am = GameObject.Find("AudioManager");
        if (am)
            audioManager = am.GetComponent<AudioManager>();

        playerRigidbody = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();
        playerAnimator = PlayerGO.GetComponent<Animator>();
        playerRenderer = PlayerGO.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (!isAlive || Globals.IsPaused) return;
        moveLeft = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A);
        moveRight = Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);
        moveUp = Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W);
        moveDown = Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S);

        controllerLeftStickX = Input.GetAxis("Horizontal");
        controllerLeftStickY = Input.GetAxis("Vertical");
        if (controllerLeftStickX > .5f)
            moveRight = true;
        else if (controllerLeftStickX < -.5f)
            moveLeft = true;
        if (controllerLeftStickY > .5f)
            moveUp = true;
        else if (controllerLeftStickY < -.5f)
            moveDown = true;

        movementVector = new Vector2(0, 0);

        if (moveRight)
        {
            movementVector.x = moveSpeed;
            PlayerGO.transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        else if (moveLeft)
        {
            movementVector.x = moveSpeed * -1f;
            PlayerGO.transform.localEulerAngles = new Vector3(0, 180f, 0);
        }
        if (moveUp)
            movementVector.y = moveSpeed;
        else if (moveDown)
            movementVector.y = moveSpeed * -1f;

        playerRigidbody.velocity = movementVector;

        if ((movementVector.x != 0 || movementVector.y != 0) && !isMoving)
        {
            isMoving = true;
            playerAnimator.Play("player-walk" + Globals.AnimationSuffixes[(int)Globals.currentPlayerType]);
        }
        else if (movementVector.x == 0 && movementVector.y == 0 && isMoving)
        {
            isMoving = false;
            playerAnimator.Play("player-idle" + Globals.AnimationSuffixes[(int)Globals.currentPlayerType]);
        }

        if (moveRight && !moveUp && !moveDown)
            currentPlayerOrientation = PlayerOrientation.Right;
        else if (moveLeft && !moveUp && !moveDown)
            currentPlayerOrientation = PlayerOrientation.Left;
        else if (moveUp && !moveRight && !moveLeft)
            currentPlayerOrientation = PlayerOrientation.Up;
        else if (moveDown && !moveRight && !moveLeft)
            currentPlayerOrientation = PlayerOrientation.Down;
        else if (moveRight && moveUp)
            currentPlayerOrientation = PlayerOrientation.UpRight;
        else if (moveLeft && moveUp)
            currentPlayerOrientation = PlayerOrientation.UpLeft;
        else if (moveRight && moveDown)
            currentPlayerOrientation = PlayerOrientation.DownRight;
        else if (moveLeft && moveDown)
            currentPlayerOrientation = PlayerOrientation.DownLeft;

        // if (currentPlayerOrientation == PlayerOrientation.Right || currentPlayerOrientation == PlayerOrientation.Left)
        //     GunGO.transform.localEulerAngles = new Vector3(0, 0, 0);
        // else if (currentPlayerOrientation == PlayerOrientation.UpLeft || currentPlayerOrientation == PlayerOrientation.UpRight)
        //     GunGO.transform.localEulerAngles = new Vector3(0, 0, 45f);
        // else if (currentPlayerOrientation == PlayerOrientation.Up)
        //     GunGO.transform.localEulerAngles = new Vector3(0, 0, 90f);
        // else if (currentPlayerOrientation == PlayerOrientation.Down)
        //     GunGO.transform.localEulerAngles = new Vector3(0, 0, 270f);
        // else if (currentPlayerOrientation == PlayerOrientation.DownLeft || currentPlayerOrientation == PlayerOrientation.DownRight)
        //     GunGO.transform.localEulerAngles = new Vector3(0, 0, 315f);

        if (ball)
        {
            if (isMoving)
            {
                float orientation = moveRight ? 1f : -1f;
                ball.SpinBall();
                ball.MoveBall(new Vector3(this.transform.localPosition.x + ballOffsetX * orientation, this.transform.localPosition.y + ballOffsetY, this.transform.localPosition.z));
            }
            else
            {
                ball.StopBall();
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log(other.tag);
        if (other.tag == "Ball")
        {
            ball = other.GetComponent<Ball>();
            ball.SpinBall();
        }
    }

}
