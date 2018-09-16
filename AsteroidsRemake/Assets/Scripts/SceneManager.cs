using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour {

    public GameObject asteroid;
    public float numberOfAsteroids;

	void Start () {
        Instantiate(asteroid);
	}
	
	void Update () {
		
	}
}
