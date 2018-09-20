using UnityEngine;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour {

    public GameObject asteroid;
    public GameObject ship;
    public Text scoreText;
    public float numberOfAsteroids;

    private int score;
    private int lives;

	void Start () {
        lives = 3;
        Instantiate(asteroid,new Vector2(3,3),transform.rotation);
        Instantiate(ship);
	}

    private void Update()
    {
        scoreText.text = score.ToString();
    }

    public void IncreaseScore(int amount)
    {
        score += amount;
    }
}
