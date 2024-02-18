using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpfulItemBehaviour : MonoBehaviour
{
    private GameManager gm;
    private FrogMovement player;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindObjectOfType<GameManager>();
        player = GameObject.FindObjectOfType<FrogMovement>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// When the user touches this object, we deactivate this and play
    /// some noises and increment some values in FrogMovement.
    /// We also destroy this object on a delay so that our audio clips
    /// play properly
    /// </summary>
    /// <param name="other">The object that touched this object</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            player.playCollectionNoise = true;;
            if (this.gameObject.CompareTag("FlyObject"))
            {
                gm.numOfDoubleJumps++;
                Debug.Log($"Number of double jumps = {gm.numOfDoubleJumps}");
            }
            else if(this.gameObject.CompareTag("CoinObject"))
            {
                gm.coinsCollected++;
                Debug.Log($"Coins Collected: {gm.coinsCollected}");
            }

            gameObject.SetActive(false);
            Invoke("InvokeDestroy", 1f);
        }
            
    }

    private void InvokeDestroy()
    {
        Destroy(gameObject);
    }
}
