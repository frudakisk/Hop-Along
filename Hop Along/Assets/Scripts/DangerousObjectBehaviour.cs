using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerousObjectBehaviour : MonoBehaviour
{
    /// <summary>
    /// If the player gets hit by a dangerous item, we end the game
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<FrogMovement>())
        {
            Debug.Log("Frog hit dangerous object");
            FrogMovement player = GameObject.FindObjectOfType<FrogMovement>();
            player.gameOver = true;
        }
    }
}
