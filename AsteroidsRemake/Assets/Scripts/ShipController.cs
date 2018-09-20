using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{

    public float maxSpeed = 2;
    public float lookSpeed = 1;
    public float maxLookSpeed = 5;

    public GameObject gunPosition;
    public GameObject bullet;
    public GameObject explosion;

    private Vector2 direction;
    private float actualSpeed = 0;
    private float rotateAmount = 0;
    Vector2 oldPosition;
    bool collided;

    float verticalHalfSize;
    float horizontalHalfSize;

    private void Start()
    {
        verticalHalfSize = Camera.main.orthographicSize;
        horizontalHalfSize = verticalHalfSize * Screen.width / Screen.height;
    }

    void Update()
    {
        GetInput();
        HandleWrapping();

        transform.position = transform.position + transform.up * Time.deltaTime * actualSpeed;
        transform.Rotate(0, 0, rotateAmount);
    }

    private void GetInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            if (actualSpeed < maxSpeed)
            {
                actualSpeed += 0.2f;
            }
        }
        if (Input.GetKey(KeyCode.S))
        {
            if (actualSpeed > -maxSpeed)
            {
                actualSpeed -= 0.2f;
            }
        }
        if (Input.GetKey(KeyCode.A))
        {
            if (rotateAmount < maxLookSpeed)
                rotateAmount += lookSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            if (rotateAmount > -maxLookSpeed)
                rotateAmount += -lookSpeed * Time.deltaTime;
        }
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            rotateAmount = Mathf.Lerp(rotateAmount, 0, 0.2f);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(bullet, gunPosition.transform.position, transform.rotation);
        }
    }

    private void HandleWrapping()
    {
        if (transform.position.y > verticalHalfSize)
        {
            transform.position = new Vector2(transform.position.x, -verticalHalfSize);
        }
        if (transform.position.y < -verticalHalfSize)
        {
            transform.position = new Vector2(transform.position.x, verticalHalfSize);
        }
        if (transform.position.x > horizontalHalfSize)
        {
            transform.position = new Vector2(-horizontalHalfSize, transform.position.y);
        }
        if (transform.position.x < -horizontalHalfSize)
        {
            transform.position = new Vector2(horizontalHalfSize, transform.position.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Asteroid")
        {
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
