using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    //private
    private GameObject player;
    private float maxDurationTime = 10f;
    private float currentDurationTime = 0f;

    //public
    public float speed = 8f;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //stop and start time depending on isPaused
        Time.timeScale = PauseScreenBehaviour.isPaused ? 0 : 1;
        //move the bullet when the game is not paused
        if(!PauseScreenBehaviour.isPaused)
        {
            transform.Translate(Vector3.up * Time.deltaTime * speed);

            //count how long the bullet has been in existance
            currentDurationTime += Time.deltaTime;
            if (currentDurationTime > maxDurationTime || player.GetComponent<FrogMovement>().gameOver == true)
            {
                //destroy the bullet after max time
                //or if the game is over
                Destroy(gameObject);
            }
        }



    }

    /// <summary>
    /// If the bullet hits the player, we destroy the bullet
    /// </summary>
    /// <param name="collision">the object that collided with this object</param>
    private void OnCollisionEnter(Collision collision)
    {
        if(player.gameObject.name == "Player")
        {
            Destroy(gameObject);
        }
    }

}
