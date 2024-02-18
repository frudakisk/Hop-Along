using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackedPlatformBehaviour : MonoBehaviour
{

    [Header("Wait Times")]
    public float waitTime = 5f;
    public float waitToShake = 1f;

    
    [Header("Shake Items")]
    public float shakeDuration = 0.5f;
    public float shakeIntensity = 0.2f;


    private bool destroyObject = false;
    private bool playerTouched = false;
    private Vector3 originalPosition;
    public GameObject explosionParticlesPrefab;

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y == 0 && playerTouched)
        {
            Invoke("Shake", waitToShake); 
        }

        if(destroyObject)
        {
            GameObject particles = Instantiate(explosionParticlesPrefab, transform.position, Quaternion.identity);
            particles.GetComponent<ParticleSystem>().Play();
            gameObject.SetActive(false);
            //Destroy(gameObject);
        }
    }

    /// <summary>
    /// When the player collides with this object we start the InvokeFall
    /// method
    /// </summary>
    /// <param name="collision">the other object</param>
    public void OnCollisionEnter(Collision collision)
    {
        
        //when the frog lands on the platform, we need to platform to
        //fall a couple meters and then disappear after a couple of seconds
        if(collision.gameObject.GetComponent<FrogMovement>())
        {
            //shake the platform on it's y axis for a couple of seconds
            playerTouched = true;
            Invoke("InvokeFall", waitTime);
        }
    }

    /// <summary>
    /// Turns the rigid body constraints off so the object just falls to gravity
    /// </summary>
    private void InvokeFall()
    {
        
    }

    /// <summary>
    /// starts a coroutine
    /// </summary>
    public void Shake()
    {
        StartCoroutine(ShakeCoroutine());
    }

    /// <summary>
    /// A coroutine that shakes the game object
    /// </summary>
    /// <returns>idk</returns>
    IEnumerator ShakeCoroutine()
    {
        float elapsedTime = 0f;
        while(elapsedTime < shakeDuration)
        {
            //Generate a random offset for the shake
            float offsetX = Random.Range(-1f, 1f) * shakeIntensity;
            float offsetY = Random.Range(-1f, 1f) * shakeIntensity;

            //Apply the offset to the gameobjects position
            transform.position = new Vector3(originalPosition.x + offsetX, originalPosition.y + offsetY, originalPosition.z);

            //increment the elapsed time Time.deltaTime is the amount of time
            //that passed since the last frame was made
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        //reset to original position when shake is complete
        transform.position = originalPosition;
        destroyObject = true;
    }
}
