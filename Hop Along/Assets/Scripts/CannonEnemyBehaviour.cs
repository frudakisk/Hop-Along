using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonEnemyBehaviour : MonoBehaviour
{
    //public
    public Transform bulletPrefab;

    //private
    private float delay = 3f;
    private Vector3 originalScale = Vector3.one;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        Invoke("SpawnBullet", delay);
    }

    // Update is called once per frame
    void Update()
    {
        //get a standard rotation
        Quaternion currentRotation = Quaternion.Euler(0, 0, 0);
        //make the object look at the player
        transform.LookAt(player.transform);

        //make the object only rotate on Y axis
        Quaternion offsetRotation = Quaternion.Euler(currentRotation.eulerAngles.x, transform.eulerAngles.y, currentRotation.eulerAngles.z);
        //set the objects rotation to this new offset rotation
        transform.rotation = offsetRotation;

        //reset the local scale after rotation - incase the scale gets messed up
        transform.localScale = originalScale;

        
    }

    /// <summary>
    /// Spawns a bullet infront of the cannon at a random time between 1 and 5 seconds.
    /// </summary>
    private void SpawnBullet()
    {
        if(player.GetComponent<FrogMovement>().gameOver == false)
        {
            float spawnDelay = Random.Range(1, 6);

            //where do we spawn the bullet? - at our SpawnBullets game object location
            Transform bulletSpawn = transform.Find("SpawnBullet");
            //set the rotation of the bullet to go towards the players current location
            //only want its z axis to change so its turned toward player
            Quaternion bulletRotation = Quaternion.Euler(bulletPrefab.rotation.eulerAngles.x, bulletPrefab.rotation.eulerAngles.y, -transform.rotation.eulerAngles.y + 180);
            Instantiate(bulletPrefab, bulletSpawn.position, bulletRotation);


            Invoke("SpawnBullet", spawnDelay);
        }
        
    }
}
