using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterMoveController : MonoBehaviour
{
       
    private Rigidbody2D rig;  
    private Animator anim;
    private CharacterSoundController sound;

    [Header("Scoring")]
    public ScoreController score;
    public float scoringRatio;
    private float lastPositionX;

    [Header("Movement")]
    public float moveAccel;
    public float maxSpeed;

    [Header("Jump")]
    public float jumpAccel;
    private bool isJumping;
    private bool isOnGround;

    [Header("Ground Raycast")]
    public float groundRaycastDistance;
    public LayerMask groundLayerMask;

    [Header("GameOver")]
    public GameObject gameOverScreen;
    public float fallPositionY;

    [Header("Camera")]
    public CameraMoveController gameCamera;
    


    private void Start()
    {
    rig = GetComponent<Rigidbody2D>();
    anim = GetComponent<Animator>();
    sound = GetComponent<CharacterSoundController>();
    }

    private void Update()
    {
     // read input
     if (Input.GetMouseButtonDown(0))
     {
         if (isOnGround)
         {
             isJumping = true;

             sound.PlayJump();
         }
         
     }


     // change animation
    anim.SetBool("isOnGround", isOnGround);

    // calculate score
    int distancePassed = Mathf.FloorToInt(transform.position.x - lastPositionX);
    int scoreIncrement = Mathf.FloorToInt(distancePassed / scoringRatio);

    if (scoreIncrement > 0)
    {
        score.IncreaseCurrentScore(scoreIncrement);
        lastPositionX += distancePassed;
    }
    // game over
    if (transform.position.y < fallPositionY)
    {
        GameOver();
    }
}

    private void GameOver()
{
    // set high score
    score.FinishScoring();

    // stop camera movement
    gameCamera.enabled = false;

    // show gameover
    gameOverScreen.SetActive(true);

    // disable this too
    this.enabled = false;

}
    

    private void FixedUpdate()
    {
    // raycast ground
    RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundRaycastDistance, groundLayerMask);
    if (hit)
    {
        if (!isOnGround && rig.velocity.y <= 0)
        {
            isOnGround = true;
        }
    }
    else
    {
        isOnGround = false;
    }

    // calculate velocity vector
    Vector2 velocityVector = rig.velocity;

    if (isJumping)
    {
        velocityVector.y += jumpAccel;
        isJumping = false;
    }

    velocityVector.x = Mathf.Clamp(velocityVector.x + moveAccel * Time.deltaTime, 0.0f, maxSpeed);

    rig.velocity = velocityVector;
    }

private void OnDrawGizmos()
{
    Debug.DrawLine(transform.position, transform.position + (Vector3.down * groundRaycastDistance), Color.white);
}
}
