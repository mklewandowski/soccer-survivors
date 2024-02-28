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

    [SerializeField] GameObject PlayerGO;

    float moveSpeed = 3f;
    float moveSpeedInitial = 3;
    Vector2 movementVector = new Vector2(0, 0);
    bool moveLeft;
    bool moveRight;
    bool moveUp;
    bool moveDown;
    private Rigidbody2D playerRigidbody;
    bool isMoving = false;
    Animator playerAnimator;
    [SerializeField] PlayerAnimation playerAnimation;
    float dustTimer = 0f;
    float dustTimerMax = .1f;

    bool isAlive = true;
    float health = 20f;
    float invincibleTimer = 0f;
    float invincibleTimerMax = .6f;
    private SpriteRenderer playerRenderer;
    private BoxCollider2D playerCollider;
    [SerializeField] Material FlashMaterial;
    Material playerMaterial;
    float flashTimer = 0f;
    float flashTimerMax = .15f;
    [SerializeField] GameObject HealthBar;
    [SerializeField] GameObject HealthBarBack;

	float controllerLeftStickX;
	float controllerLeftStickY;

    Ball ball;
    float ballOffsetY = -.45f;
    float ballOffsetX = .7f;
    float minKickPower = 5f;
    float maxKickPower = 20f;
    float kickPower = 5f;
    float kickTimer = 0;
    float kickTimerMax = 1f;
    [SerializeField] GameObject PowerBar;
    [SerializeField] GameObject PowerBarFront;
    float maxPowerBarWidth = 100f;
    bool holdingKick = false;
    bool kicking = false;

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
        HandleKick();
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
            playerAnimation.SetIsMoving(true);
            if (!kicking) playerAnimator.Play("player-walk" + Globals.AnimationSuffixes[(int)Globals.currentPlayerType]);
        }
        else if (movementVector.x == 0 && movementVector.y == 0 && isMoving)
        {
            isMoving = false;
            playerAnimation.SetIsMoving(false);
            if (!kicking) playerAnimator.Play("player-idle" + Globals.AnimationSuffixes[(int)Globals.currentPlayerType]);
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
    }


    private void HandleKick()
    {
        if (ball)
        {
            if (isMoving)
            {
                float orientation = 1f;
                if (PlayerGO.transform.localEulerAngles.y == 180f)
                    orientation = -1f;
                ball.SpinBall();
                ball.MoveBall(new Vector3(this.transform.localPosition.x + ballOffsetX * orientation, this.transform.localPosition.y + ballOffsetY, this.transform.localPosition.z));
            }
            else
            {
                ball.StopBall();
            }
        }

        if (!holdingKick && !kicking && ball)
        {
            holdingKick = Input.GetKeyDown(KeyCode.Space);
            if (holdingKick)
            {
                kickTimer = 0;
                PowerBar.SetActive(true);
            }
        }

        if (holdingKick)
        {
            kickTimer += Time.deltaTime;
            kickTimer = Mathf.Min(kickTimer, kickTimerMax);
            // calc width
            float percent = kickTimer / kickTimerMax;
            kickPower = minKickPower + (maxKickPower - minKickPower) * percent;
            PowerBarFront.transform.localScale = new Vector3(maxPowerBarWidth * percent, PowerBarFront.transform.localScale.y, PowerBarFront.transform.localScale.z);
        }

        bool kickBall = Input.GetKeyUp(KeyCode.Space) && ball != null;
        if (kickBall)
        {
            StartCoroutine(KickBall());

        }
    }

    IEnumerator KickBall()
    {
        kicking = true;
        float maxTime = .3f;
        playerAnimator.Play("player-shoot" + Globals.AnimationSuffixes[(int)Globals.currentPlayerType]);
        PowerBar.SetActive(false);
        kickTimer = 0;
        holdingKick = false;

        while (maxTime >= 0.0f)
        {
            maxTime -= Time.deltaTime;
            yield return null;
        }

        float xSpeed = 0;
        if (currentPlayerOrientation == PlayerOrientation.UpRight || currentPlayerOrientation == PlayerOrientation.Right || currentPlayerOrientation == PlayerOrientation.DownRight)
            xSpeed = kickPower;
        if (currentPlayerOrientation == PlayerOrientation.UpLeft || currentPlayerOrientation == PlayerOrientation.Left || currentPlayerOrientation == PlayerOrientation.DownLeft)
            xSpeed = -kickPower;
        float ySpeed = 0;
        if (currentPlayerOrientation == PlayerOrientation.UpRight || currentPlayerOrientation == PlayerOrientation.Up || currentPlayerOrientation == PlayerOrientation.UpLeft)
            ySpeed = kickPower;
        if (currentPlayerOrientation == PlayerOrientation.DownRight || currentPlayerOrientation == PlayerOrientation.Down || currentPlayerOrientation == PlayerOrientation.DownLeft)
            ySpeed = -kickPower;
        ball.LaunchBall(new Vector2(xSpeed, ySpeed));
        ball = null;
        kicking = false;
    }

    public void EndKick()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Ball")
        {
            ball = other.GetComponent<Ball>();
            ball.SpinBall();
        }
    }

}
