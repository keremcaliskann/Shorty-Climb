using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    private GameController gameController;

    public Rigidbody myRigidbody;
    public Animator animator;

    private float moveSpeed;
    private bool onGround;
    public bool onAir;
    private bool moveHorizontal;
    private bool moveVertical;

    private float firstTouch;
    private float secondTouch;
    private Vector3 baseRotation;

    public int collideCount;

    private float lastTouchedGroundYPos;
    private float targetJumpHeight;


    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        myRigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        moveSpeed = 8f;
        collideCount = 0;
        lastTouchedGroundYPos = 0f;
        moveHorizontal = true;
    }

    void FixedUpdate()
    {
        if (gameController.gameStarted)
        {
            MovePlayer();

            if (Input.touchCount > 0)
            {
                foreach (Touch touch in Input.touches)
                {
                    if (touch.phase == TouchPhase.Began)
                    {
                        baseRotation = transform.rotation.eulerAngles;
                        firstTouch = touch.position.x;
                    }
                    secondTouch = touch.position.x;
                    Vector3 targetRot = new Vector3(0, baseRotation.y - (firstTouch - secondTouch) / 3, 0);
                    transform.eulerAngles = targetRot;
                }
            }

            if (collideCount <= 0 && !onAir)
            {
                if (moveHorizontal)
                {
                    gameController.PlaceLadder();
                }
            }
            /*
            if (targetJumpHeight <= transform.position.y - 0.5f && myRigidbody.velocity.y < 0)
            {
                onAir = false;
            }*/
        }
    }

    private void MovePlayer()
    {
        if (moveHorizontal)
        {
            animator.SetTrigger("Move");
            transform.position += transform.forward * Time.deltaTime * moveSpeed;
        }
        else
        {
            animator.SetTrigger("Idle");
        }
    }

    private IEnumerator MoveUpwards(float targetHeight)
    {
        animator.SetTrigger("Move");
        moveVertical = true;
        moveHorizontal = false;
        while (!moveHorizontal)
        {
            myRigidbody.useGravity = false;
            transform.position += transform.up * Time.deltaTime * moveSpeed * 2;
            /*
            if (collideCount <= 0 && !onAir)
            {
                gameController.PlaceLadder(1);
            }*/
            if (transform.position.y > targetHeight)
            {
                onAir = true;
                moveVertical = false;
                moveHorizontal = true;
                myRigidbody.useGravity = true;
                yield return null;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        collideCount--;
        if (collideCount <= 0 && gameController.timberCount <= 0)
        {
            Jump();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        collideCount++;
        if (collision.transform.CompareTag("Ground"))
        {
            if (collision.transform.position.y > lastTouchedGroundYPos && gameController.hasTimber)
            {
                float range = collision.transform.position.y + collision.transform.localScale.y / 2;
                StartCoroutine(MoveUpwards(range));
                lastTouchedGroundYPos = collision.transform.position.y;
            }
            lastTouchedGroundYPos = collision.transform.position.y;
            onGround = true;
            onAir = false;
            moveSpeed = 8;
        }
    }

    public void Jump(float upForce = 200, float jumpSpeed = 8, float blend = 0, float targetHeight = 0)
    {
        moveSpeed = jumpSpeed;
        onAir = true;
        myRigidbody.useGravity = true;
        myRigidbody.AddForce(Vector3.up * upForce);
        if (blend == 0)
        {
            animator.SetTrigger("Jump");
        }
        targetJumpHeight = targetHeight;
    }

    public void Die()
    {
        if (!onGround)
        {
            animator.SetTrigger("Die");
        }
    }

}
