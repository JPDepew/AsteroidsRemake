using UnityEngine;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour
{

    public GameObject asteroid;
    public GameObject ship;
    public Text scoreText;
    public Text livesText;
    public float numberOfAsteroids;

    public static SceneManager instance;

    private GameObject shipReference;
    private bool respawningCharacter;
    private float respawnCharacterDelay = 1f;
    private float targetTime;

    private int score;
    private int lives;

    void Start()
    {
        instance = this;
        lives = 3;
        Instantiate(asteroid, new Vector2(3, 3), transform.rotation);
        shipReference = Instantiate(ship);
    }

    private void Update()
    {
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
    }
}
