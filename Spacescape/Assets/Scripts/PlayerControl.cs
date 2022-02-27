using TMPro;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private float gravity;

    public float boostForce = 2000f;

    private Rigidbody2D rb;

    private Vector2 startpos;

    private int score = 0;

    public static int highScore = 0;

    public PlayFabManager playFabManager;

    public static PlayerControl Instance { get; private set; }

    [SerializeField]
    private ParticleSystem BoostEffect;

    [SerializeField]
    private ParticleSystem ExplosionEffect;

    public GameObject Player;

    public Camera MainCamera;

    public GameObject background;

    public float forwardSpeed;

    public GameObject GameOver;

    public GameObject Canvas;

    public AudioSource audioSource;

    public AudioClip explosionAudio;

    public AudioClip boostAudio;

    public TextMeshProUGUI

            scoreText,
            highScoreText,
            endScoreText;

    public Transform target;

    public float smoothSpeed = 0.125f;

    public Vector3 offset;

    public bool isGameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        isGameOver = false;
        Instance = this;
        startpos = transform.position;
        rb = GetComponent<Rigidbody2D>();
        gravity = rb.gravityScale;
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 vel = rb.velocity;
        float ang = Mathf.Atan2(vel.y, 10) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 180, -ang));

        if (Input.GetKey("space"))
        {
            rb.AddForce(Vector2.up * gravity * Time.deltaTime * boostForce);
        }
        if (Input.GetKeyDown("space"))
        {
            BoostEffect.Play();
        }

        if (Input.GetKeyUp("space"))
        {
            BoostEffect.Stop();
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    BoostEffect.Play();
                    break;
                case TouchPhase.Moved:
                    rb
                        .AddForce(Vector2.up *
                        gravity *
                        Time.deltaTime *
                        boostForce);
                    break;
                case TouchPhase.Stationary:
                    rb
                        .AddForce(Vector2.up *
                        gravity *
                        Time.deltaTime *
                        boostForce);

                    // Debug.Log("Boost!!");
                    // audioSource.Play();
                    break;
                case TouchPhase.Ended:
                    BoostEffect.Stop();

                    // audioSource.Pause();
                    break;
                default:
                    break;
            }
        }
        score++;
        highScore = (int) score;
        scoreText.text = highScore.ToString();
        if (PlayerPrefs.GetInt("score") <= highScore)
            PlayerPrefs.SetInt("score", highScore);
    }

    void FixedUpdate()
    {
        ExplosionEffect.transform.position =
            new Vector3(Player.transform.position.x,
                Player.transform.position.y,
                -1);
        background.transform.position =
            new Vector3(MainCamera.transform.position.x, 0, 1);
        transform
            .Translate(new Vector2(1f, 0) * -forwardSpeed * Time.deltaTime);

        Vector3 desiredPostition = target.position + offset;
        Vector3 smoothedPosition =
            Vector3
                .Lerp(MainCamera.transform.position,
                desiredPostition,
                smoothSpeed);
        MainCamera.transform.position = smoothedPosition;
    }

    //Check collision
    void OnCollisionEnter2D(Collision2D other)
    {
        // Debug.Log("May Day!!!");
        ExplosionEffect.Play();
        audioSource.Play();
        GameOver.SetActive(true);
        Canvas.SetActive(false);
        Destroy (Player);
        setHighScore();

        if (PlayerPrefs.GetInt("vibration") == 1)
        {
            Vibration.Vibrate(100);
            Debug.Log("Brr");
        }

        isGameOver = true;
    }

    //Set the High-Score
    void setHighScore()
    {
        endScoreText.text = score.ToString();
        highScoreText.text = playFabManager.highScore.ToString();
        playFabManager.SendLeaderBoard (highScore);
    }

    //Reset the Score
    public static void ResetScore()
    {
        PlayerPrefs.SetInt("score", 0);
    }
}
