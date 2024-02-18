using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{

    [Header("Audio Clips")]
    public AudioClip[] gameSounds; // 0 - backgroun music, 1 - death music, 2 - game over screen music
    private AudioSource audioSource;
    
    [Header("Prefabs")]
    public Transform tile;
    public Transform platform;
    public Transform crackedPlatform;
    public Transform[] enemyPrefabs; //we have multiple different enemies to spawn
    public Transform[] helpfulItemsPrefabs; //we have multiple helpful items to spawn

    [Header("GameObjects")]
    public GameObject gameOverPanel;

    [Header("Tile Data")]
    public Vector3 startPoint = new Vector3(0, 0, 8);
    public int initSpawnNum = 10;
    private Vector3 nextTileLocation;
    private Quaternion nextTileRotation;
    private int maxNumberOfPlatforms = 3;

    [Header("Power up & Collectables")]
    public int numOfDoubleJumps = 0;
    public int coinsCollected = 0;

    //Spawn Rates for items and enemies
    private float enemySpawnRate = 20f;
    private float helpfulItemSpawnRate = 5f;

    [Header("TextMeshPro Inputs")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI doubleJumpText;
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI highscoreText;
    public TextMeshProUGUI gameOverScore;

    //private script references
    private FrogMovement player;

    //local private variables
    private float timer = 0f;
    private bool gameOverSequenceComplete = false;
    private float score;
    private string highscoreKey = "HighScore";
    private int currentHighscore;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindObjectOfType<FrogMovement>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = gameSounds[0];
        audioSource.Play();
        //Set our starting point
        nextTileLocation = startPoint;
        nextTileRotation = Quaternion.identity;

        for(int i = 0; i < initSpawnNum; i++)
        {
            SpawnNextTile();
        }

        //get the current highscore integer
        currentHighscore = PlayerPrefs.GetInt(highscoreKey, 0);
        //set the current highscore
        highscoreText.text = "Highscore: " + string.Format("{0:0}", currentHighscore);
    }

    private void Update()
    {
        doubleJumpText.text = "Double Jumps: " + string.Format("{0}", numOfDoubleJumps);
        coinsText.text = "Coins Collected: " + string.Format("{0}", coinsCollected);
        score = CalculateScore();
        scoreText.text = "Score: " + string.Format("{0:0}", score);

        timer += Time.deltaTime;
        GameOverSequence();
    }

    /// <summary>
    /// Spawns a Tile Game Object with its randomized number of platforms (btw 1 and maxNumberOfPlatforms)
    /// And figures out where the next tile will spawn
    /// </summary>
    public void SpawnNextTile()
    {
        //spawn a tile, but now we need to figure out how many platforms to put on it
        var newTile = Instantiate(tile, nextTileLocation, nextTileRotation);
        //must always be at least one platform per tile
        SpawnPlatforms(newTile, Random.Range(1,maxNumberOfPlatforms+1));

        //Figure out where and at what rotation we should spawn the next item
        var nextTile = newTile.Find("Spawn Next Point");
        nextTileLocation = nextTile.position;
        nextTileRotation = nextTile.rotation;
    }

    /// <summary>
    /// Spawns platform game object(s) in tagged spots of a
    /// Tile game object
    /// </summary>
    /// <param name="newTile"> a Tile that has been instantiated via the tile GameObject prefab</param>
    /// <param name="numOfPlatforms">The number of platforms we want to spawn on the current tile.
    /// Cannot be larger than the number of tagged PlatformSpawn areas.</param>
    public void SpawnPlatforms(Transform newTile, int numOfPlatforms)
    {
        //get all possible places we can spawn a platform
        var tileSpawnPoints = new List<GameObject>();

        //Go through each of the child objects in our Tile
        foreach(Transform child in newTile)
        {
            //if it has our platformspawn tag
            if(child.CompareTag("PlatformSpawn"))
            {
                //add it as a possibility
                tileSpawnPoints.Add(child.gameObject);
            }
        }

        //make sure there that numOfPlatforms is not created than tileSpawnPoints.Count
        if(tileSpawnPoints.Count < numOfPlatforms)
        {
            //we have an issue and need to stop
            throw new System.ArgumentException("Cannot make more platforms then there are locations");
        }

        //make sure there is at least one spawn point
        if(tileSpawnPoints.Count > 0)
        {
            List<int> indexList = new List<int>();
            int i = 0;
            while(i < numOfPlatforms)
            {
                int index = Random.Range(0, tileSpawnPoints.Count);
                if(!indexList.Contains(index))
                {
                    indexList.Add(index);
                    var spawnPoint = tileSpawnPoints[index];
                    //store its position for us to use
                    var spawnPos = spawnPoint.transform.position;

                    //create the platform or a cracked platform
                    Transform newPlatform;
                    if(Random.Range(0,4) == 0)
                    {
                        newPlatform = Instantiate(crackedPlatform, spawnPos, Quaternion.identity);
                    }
                    else
                    {
                        newPlatform = Instantiate(platform, spawnPos, Quaternion.identity);
                        //if we have a normal platform, let's place an object there right how
                        foreach(Transform child in newPlatform)
                        {
                            if(child.CompareTag("EnemySpawn"))
                            {
                                //place a prefab there given a percentage chance
                                if(Random.Range(0,100) <= enemySpawnRate) //20% of the time we spawn an enemy
                                {
                                    //Equally randomly select an enemy to spawn
                                    int enemyIndex = Random.Range(0, enemyPrefabs.Length);
                                    //find appropriate offset
                                    Vector3 offset = EnemyOffset(enemyPrefabs[enemyIndex]);

                                    var newEnemy = Instantiate(enemyPrefabs[enemyIndex], child.transform.position + offset, Quaternion.identity);
                                    //have it parented to the platform
                                    newEnemy.SetParent(child.transform);

                                }else if (Random.Range(0,100) <= helpfulItemSpawnRate) //5% chance we get a helpful item
                                {
                                    int helpfulIndex = Random.Range(0, helpfulItemsPrefabs.Length);
                                    Vector3 offset = EnemyOffset(helpfulItemsPrefabs[helpfulIndex]);
                                    var newHelpfulItem = Instantiate(helpfulItemsPrefabs[helpfulIndex], child.transform.position + offset, Quaternion.identity);
                                    newHelpfulItem.SetParent(child.transform);
                                }
                                
                            }
                        }
                    }
                    

                    //Have it parented to the tile
                    newPlatform.SetParent(spawnPoint.transform);
                    i++;
                }
            }
        }
    }

    /// <summary>
    /// A wraparound function for showing the game over panel
    /// </summary>
    public void InvokeGameOverPanel()
    {
        gameOverPanel.SetActive(true);
        //play the game over music
        audioSource.clip = gameSounds[3];
        audioSource.loop = true;
        audioSource.Play();
    }

    /// <summary>
    /// Create an appropriate offset from y axis for the
    /// enemy that we are currently working with
    /// </summary>
    /// <param name="enemy"></param>
    /// <returns></returns>
    private Vector3 EnemyOffset(Transform enemy)
    {
        Transform etran = enemy.transform;
        if(etran.CompareTag("CannonEnemy"))
        {
            return new Vector3(0, 0.25f, 0);
        }
        else if (etran.CompareTag("SpikeEnemy"))
        {
            return new Vector3(0, 0, 0);
        }
        else if (etran.CompareTag("FlyObject") || etran.CompareTag("CoinObject"))
        {
            return new Vector3(0, 0.5f, 0);
        }

        //if all fails, return empty vector
        return new Vector3(0, 0, 0);
    }

    /// <summary>
    /// Calculates the score of the player
    /// </summary>
    /// <returns></returns>
    private float CalculateScore()
    {
        // player y * time since game started * coins collected
        float score = player.transform.position.z * 10 + (coinsCollected * 100);
        return score;
    }

    /// <summary>
    /// The order of which things should happen when the game is over
    /// </summary>
    public void GameOverSequence()
    {
        //check if the game over sequence has already been triggered
        if (player.gameOver && gameOverSequenceComplete == false)
        {
            //capture the current score & see if its larger than the current highscore
            UpdateHighscore();
            //When the player dies, I want to play a death sound
            //and when the sound is done, we go to the game over screen
            audioSource.Stop();
            audioSource.PlayOneShot(gameSounds[1]);
            gameOverScore.text = "Score: " + string.Format("{0:0}", score);
            gameOverSequenceComplete = true;
            Invoke("InvokeGameOverPanel", 3f);

        }
    }

    private void UpdateHighscore()
    {
        Debug.Log($"End Game score: {score}");
        if(score > currentHighscore)
        {
            //if the score is larger than the current highscore
            //we need to set the current highscore to score
            int s = (int)score;
            PlayerPrefs.SetInt(highscoreKey, s);
        }
    }

}
