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

    public enum GameState { RUNNING, STOPPED }
    public GameState gameState;

    private PlayerStats playerStats;
    private ShipController shipController;
    private AudioSource audioSource;
    private GameObject shipReference;
    private bool respawningCharacter;
    private float respawnCharacterDelay = 1f;
    private float targetTime;

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

        scoreText.text = score.ToString();
        if (shipController != null)
        {
            livesText.text = playerStats.GetLives().ToString() + "x";
        }
        if (scoreTracker > 10000)
        {
            scoreTracker = 0;
            playerStats.IncrementLives();
        }

        if (respawningCharacter)
        {
            if (Time.time > targetTime)
            {
                shipReference = Instantiate(ship);
                shipController = shipReference.GetComponent<ShipController>();
                respawningCharacter = false;
            }
        }
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
            InstantiateNewWave();
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
        //audioSource.Play();
        score += amount;
        scoreTracker += amount;
    }

    public void RespawnPlayer()
    {
        if (playerStats.GetLives() > 0)
        {
            respawningCharacter = true;
            targetTime = Time.time + respawnCharacterDelay;
        }
        else
        {
            gameOverText.gameObject.SetActive(true);
        }
    }
}
