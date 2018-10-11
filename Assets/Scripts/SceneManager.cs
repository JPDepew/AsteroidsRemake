using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour
{
    public GameObject asteroid;
    public GameObject ship;
    public Text scoreText;
    public Text livesText;
    public Text startGameText;
    public Text sfxCreditsText;
    public Text gameOverText;
    public float numberOfAsteroids;
    public float playerRespawnDelay = 1f;
    public float instantiateNewWaveDelay = 2f;

    public enum GameState { RUNNING, STOPPED }
    [NonSerialized]
    public GameState gameState;

    private PlayerStats playerStats;
    private ShipController shipController;
    private AudioSource audioSource;
    private GameObject shipReference;
    private bool respawningCharacter;
    private bool instantiatingNewWave;
    private float playerRespawnTimer = 1f;
    private float instantiateNewWaveTimer = 2f;

    private int score;
    private int scoreTracker;
    private int asteroidCountTracker;
    private int dstAsteroidsSpawnFromSides = 1;
    private float verticalHalfSize = 0;
    private float horizontalHalfSize = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        gameState = GameState.STOPPED;
        playerStats = PlayerStats.instance;
        verticalHalfSize = Camera.main.orthographicSize;
        horizontalHalfSize = verticalHalfSize * Screen.width / Screen.height;
        Asteroid.onSmallAsteroidDestroyed += OnSmallAsteroidDestroyed;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space) && gameState == GameState.STOPPED)
        {
            StartGame();
        }

        if (scoreTracker > 10000)
        {
            scoreTracker = 0;
            playerStats.IncrementLives();
        }

        HandleUI();
        HandleWaveTimer();
    }

    private void StartGame()
    {
        gameState = GameState.RUNNING;
        startGameText.enabled = false;
        sfxCreditsText.enabled = false;
        asteroidCountTracker = 0;
        shipReference = Instantiate(ship);
        shipController = shipReference.GetComponent<ShipController>();

        Asteroid[] asteroids = FindObjectsOfType<Asteroid>();
        for (int i = 0; i < asteroids.Length; i++)
        {
            Destroy(asteroids[i].gameObject);
        }

        InstantiateNewWave();
    }

    private void HandleUI()
    {
        scoreText.text = score.ToString();
        livesText.text = playerStats.GetLives().ToString() + "x";
    }

    IEnumerator WaitForRespawn()
    {
        yield return new WaitForSeconds(playerRespawnDelay);
        shipReference = Instantiate(ship);
        shipController = shipReference.GetComponent<ShipController>();
    }

    IEnumerator HandleWaveTimer()
    {
        yield return new WaitForSeconds(instantiateNewWaveTimer);
        InstantiateNewWave();
    }

    private void InstantiateNewWave()
    {
        for (int i = 0; i < numberOfAsteroids; i++)
        {
            int xRange = (int)UnityEngine.Random.Range(-horizontalHalfSize, horizontalHalfSize);
            int yRange = (int)UnityEngine.Random.Range(-verticalHalfSize, verticalHalfSize);

            Vector2 asteroidPositon = new Vector2(xRange, yRange);
            if ((asteroidPositon - (Vector2)shipReference.transform.position).magnitude < dstAsteroidsCanSpawnFromPlayer)
            if ((asteroidPositon - (Vector2)shipReference.transform.position).magnitude < 3)
            {
                i--; // This is probably really sketchy, I know... But it works really well...
            }
            else
            {
                Instantiate(asteroid, asteroidPositon, transform.rotation);
            }
        }
    }

    private void OnSmallAsteroidDestroyed()
    {
        asteroidCountTracker++;
        if (asteroidCountTracker >= numberOfAsteroids * 4)
        {
            numberOfAsteroids++;
            asteroidCountTracker = 0;
            StartCoroutine(HandleWaveTimer());
        }
    }

    private void EndGame()
    {
        gameState = GameState.STOPPED;
        startGameText.enabled = true;
        sfxCreditsText.enabled = true;
    }

    public void IncreaseScore(int amount)
    {
        score += amount;
        scoreTracker += amount;
    }

    public void RespawnPlayer()
    {
        if (playerStats.GetLives() > 0)
        {
            StartCoroutine(WaitForRespawn());
        }
        else
        {
            gameOverText.gameObject.SetActive(true);
        }
    }
}
