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
        HandleRespawnTimer();
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

    private void HandleRespawnTimer()
    {
        if (respawningCharacter)
        {
            if (Time.time > playerRespawnTimer)
            {
                shipReference = Instantiate(ship);
                shipController = shipReference.GetComponent<ShipController>();
                respawningCharacter = false;
            }
        }
    }

    private void HandleWaveTimer()
    {
        if (instantiatingNewWave)
        {
            if (Time.time > instantiateNewWaveTimer)
            {
                InstantiateNewWave();
                instantiatingNewWave = false;
            }
        }
    }

    private void InstantiateNewWave()
    {
        for (int i = 0; i < numberOfAsteroids; i++)
        {
            int xRange = (int)Random.Range(-horizontalHalfSize, horizontalHalfSize);
            int yRange = (int)Random.Range(-verticalHalfSize, verticalHalfSize);
            if (xRange < 2 && xRange > -2)
            {
                xRange = 2;
            }
            if (yRange < 2 && yRange > -2)
            {
                yRange = 2;
            }

            Vector2 asteroidPositon = new Vector2(xRange, yRange);

            Instantiate(asteroid, asteroidPositon, transform.rotation);
        }
    }

    private void OnSmallAsteroidDestroyed()
    {
        asteroidCountTracker++;
        if (asteroidCountTracker >= numberOfAsteroids * 4)
        {
            numberOfAsteroids++;
            asteroidCountTracker = 0;
            instantiateNewWaveTimer = Time.time + instantiateNewWaveDelay;
            instantiatingNewWave = true;
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
            respawningCharacter = true;
            playerRespawnTimer = Time.time + playerRespawnDelay;
        }
        else
        {
            gameOverText.gameObject.SetActive(true);
        }
    }
}
