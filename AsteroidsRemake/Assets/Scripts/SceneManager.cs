using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour {

    public GameObject asteroid;
    public GameObject ship;
    public Text scoreText;
    public float numberOfAsteroids;

    private static int score;
    private float lives;

	void Start () {
        lives = 3;
        Instantiate(asteroid,new Vector2(3,3),transform.rotation);
        Instantiate(ship);
	}

    private void Update()
    {
        scoreText.text = score.ToString();
    }

    public static void IncreaseScore()
    {
        score++;
    }
}
