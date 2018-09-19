using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour {

    public GameObject asteroid;
    public GameObject ship;
    public float numberOfAsteroids;

	void Start () {
        Instantiate(asteroid,new Vector2(3,3),transform.rotation);
        Instantiate(ship);
	}
}
