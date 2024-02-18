using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogMovement : MonoBehaviour
{
    //private references
    private AudioSource frogAudio;
    private GameManager gm;
    private FrogController frogController;
    private Rigidbody rb;


    public ParticleSystem jumpingParticles;

    [Header("Frog AudioClip Items")]
    public AudioClip[] frogNoises;
    public bool playCollectionNoise = false;

    [Header("Speed and Jumping")]
    public float horizontalSpeed = 5.0f;
    public float forwardSpeed = 5.0f;
    public float jumpAmount = 20.0f;

    private float horizontalInput;
    private float forwardInput;
    private float fallLimit = -5.0f;
    private float maxHeight = 25.0f;
    private KeyCode key = KeyCode.Space;

    [Header("Booleans")]
    public bool gameOver = false; //very powerful
    public bool isDoubleJumping = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gm = GameObject.FindObjectOfType<GameManager>();
        frogController = GameObject.FindObjectOfType<FrogController>();
        frogAudio = GetComponent<AudioSource>();
    }

    // Updates every frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        forwardInput = Input.GetAxis("Vertical");

        bool isPaused = PauseScreenBehaviour.isPaused;

        //freeze the player when the game is paused
        if(isPaused)
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
        else
        {
            //only freeze rotation when game is not paused
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }

        //if game is over, we don't want the player to be able to move anymore
        if (gameOver == false && isPaused == false)
        {
            transform.Translate(Vector3.right * horizontalSpeed * Time.deltaTime * horizontalInput);
            transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime * forwardInput);
        }

        //Adding force need to be in the update function because we are constantly updating the position
        //of the player
        if (!gameOver && Input.GetKeyDown(key) && transform.position.y > -0.25 && transform.position.y < 0.5)
        {
            //add an upward force to the player to simulate a jump
            rb.AddForce(Vector3.up * jumpAmount, ForceMode.Impulse);
            //perform jumping animation and noise
            frogController.Jump();
            jumpingParticles.Play();
            frogAudio.PlayOneShot(frogNoises[0]);
            Invoke("Idle", 1f);

        }
        //if the player has a double jump, they can jump whenever they want another time
        else if (!gameOver && Input.GetKeyDown(key) && gm.numOfDoubleJumps > 0 && isDoubleJumping == false &&
            (transform.position.y <= -0.25 || transform.position.y >= 0.5))
        {
            isDoubleJumping = true;
            rb.AddForce(Vector3.up * jumpAmount * 1.5f, ForceMode.Impulse);
            jumpingParticles.Play();
            frogAudio.PlayOneShot(frogNoises[0]);
            gm.numOfDoubleJumps--;
        }

        //write something to indicate that we are not double jumping anymore
        if (transform.position.y > -0.25 && transform.position.y < 0.5)
        {
            isDoubleJumping = false;
        }

        //make sure our player does not go over the max height limit while jumping
        if(transform.position.y > maxHeight)
        {
            transform.position = new Vector3(transform.position.x, maxHeight, transform.position.z);
        }

        //check to see if the player fell off the map
        //if they did, set gameOver to true
        if (transform.position.y < fallLimit)
        {
            gameOver = true;
        }

        //play our collection noise if set to true and then reset to false again
        if(playCollectionNoise)
        {
            frogAudio.PlayOneShot(frogNoises[1]);
            playCollectionNoise = false;
        }
    }

    /// <summary>
    /// Setting up the Idle animation to be used in an Invoke function
    /// </summary>
    void Idle()
    {
        frogController.Idle();
    }


}
