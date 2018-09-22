using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour
{
    public GameObject asteroid;
    public GameObject ship;
    public Text scoreText;
    public Text livesText;
    public Text startGameText;
    public float numberOfAsteroids;

    public static SceneManager instance;
    public enum GameState { RUNNING, STOPPED }
    public GameState gameState;

    private GameObject shipReference;
    private bool respawningCharacter;
    private float respawnCharacterDelay = 1f;
    private float targetTime;

    private int score;
    private int lives;

    void Start()
    {
        gameState = GameState.STOPPED;
        instance = this;
        lives = 3;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space) && gameState == GameState.STOPPED)
        {
            StartGame();
        }

        scoreText.text = score.ToString();
        livesText.text = lives.ToString() + "x";

        if (respawningCharacter)
        {
            if (Time.time > targetTime)
            {
                shipReference = Instantiate(ship);
                respawningCharacter = false;
            }
        }
    }

    private void StartGame()
    {
        Asteroid[] asteroids = FindObjectsOfType<Asteroid>();
        for (int i = 0; i < asteroids.Length; i++)
        {
            Destroy(asteroids[i].gameObject);
        }
        startGameText.enabled = false;

        gameState = GameState.RUNNING;
        lives = 3;
        Instantiate(asteroid, new Vector2(3, 3), transform.rotation);
        shipReference = Instantiate(ship);
    }

    private void EndGame()
    {
        gameState = GameState.STOPPED;
        startGameText.enabled = true;
    }

    public void IncreaseScore(int amount)
    {
        score += amount;
    }

    public void DestroyPlayer()
    {
        lives--;
        Destroy(shipReference);
        targetTime = Time.time + respawnCharacterDelay;
        if (lives > 0)
        {
            respawningCharacter = true;
        }
        else
        {
            EndGame();
        }
    }
}
