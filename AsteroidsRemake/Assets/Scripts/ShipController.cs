using UnityEngine;

public class ShipController : MonoBehaviour
{
    public GameObject gunPosition;
    public GameObject bullet;
    public GameObject explosion;

    public float acceleration = 0.1f;
    public float lookSpeed = 1;
    public float maxLookSpeed = 5;

    private AudioSource[] audioSources;
    private Vector2 direction;
    private PlayerStats playerStats;
    private float rotateAmount = 0;
    private bool shouldDestroyShip;
    private float targetTime;

    float verticalHalfSize;
    float horizontalHalfSize;

    private void Start()
    {
        playerStats = PlayerStats.instance;
        audioSources = GetComponents<AudioSource>();
        verticalHalfSize = Camera.main.orthographicSize;
        horizontalHalfSize = verticalHalfSize * Screen.width / Screen.height;
    }

    void Update()
    {
        GetInput();
        HandleWrapping();
        HandleDestroyingShip();
        transform.position = transform.position + (Vector3)direction * Time.deltaTime;
        transform.Rotate(0, 0, rotateAmount);
    }

    private void GetInput()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            direction = Vector2.Lerp(direction, direction + (Vector2)transform.up * acceleration, 0.9f);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            if (rotateAmount < maxLookSpeed)
                rotateAmount = Mathf.Lerp(rotateAmount, rotateAmount + lookSpeed * Time.deltaTime, 0.9f);
                //rotateAmount += lookSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            if (rotateAmount > -maxLookSpeed)
                rotateAmount = Mathf.Lerp(rotateAmount, rotateAmount - lookSpeed * Time.deltaTime, 0.9f);
                //rotateAmount -= lookSpeed * Time.deltaTime;
        }
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
        {
            rotateAmount = Mathf.Lerp(rotateAmount, 0, 0.2f);
        }

        if (Input.GetKeyDown(KeyCode.Space) && !shouldDestroyShip)
        {
            audioSources[0].Play();
            Instantiate(bullet, gunPosition.transform.position, transform.rotation);
        }
    }

    private void HandleDestroyingShip()
    {
        if (shouldDestroyShip)
        {
            if (Time.time > targetTime)
            {
                Destroy(gameObject);
            }
        }
    }

    /// <summary>
    /// If the ship goes off the screen, it is wrapped around to the other side
    /// </summary>
    private void HandleWrapping()
    {
        verticalHalfSize = Camera.main.orthographicSize;
        horizontalHalfSize = verticalHalfSize * Screen.width / Screen.height;
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
            shouldDestroyShip = true;
            targetTime = Time.time + 1f;
            audioSources[1].Play();
            GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
            GetComponent<Collider2D>().enabled = false;
            playerStats.DecrementLives();

            Instantiate(explosion, transform.position, transform.rotation);
            FindObjectOfType<SceneManager>().RespawnPlayer();
        }
    }
}
