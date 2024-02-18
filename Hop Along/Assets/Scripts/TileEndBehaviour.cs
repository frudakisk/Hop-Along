using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileEndBehaviour : MonoBehaviour
{

    public float destroyTime = 1.5f;

    private void OnTriggerEnter(Collider other)
    {
        //Check if we collided with the player
        if(other.gameObject.GetComponent<FrogMovement>())
        {
            //if we did, spawn a new tile
            var gm = GameObject.FindObjectOfType<GameManager>();
            gm.SpawnNextTile();

            //destroy this entire tile after a short delay
            Destroy(transform.parent.gameObject, destroyTime);
        }
    }
}
