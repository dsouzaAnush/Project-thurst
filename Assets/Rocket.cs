using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class Rocket : MonoBehaviour
{
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float levelLoadDelay = 2f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip death;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem deathParticles;

    Rigidbody rigidBody;
    AudioSource audSource;
    FuelSystem fuelSystem;

    enum State { Alive, Dying, Transcending }
    State state = State.Alive;
    int levelpass;

    // Use this for initialization
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audSource = GetComponent<AudioSource>();
        fuelSystem = GetComponent<FuelSystem>();
        if (PlayerPrefs.HasKey("LevelPassed"))
        {

        }
        else
        {
            PlayerPrefs.SetInt("LevelPassed", 0);
        }
        levelpass = PlayerPrefs.GetInt("LevelPassed");
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive && fuelSystem.startFuel > 0)
        {
            RespondToThrust();
            RespondToRotate();
        }

        if (fuelSystem.startFuel <= 0)
        {
            DeathSequence();
            return;
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive) { return; } // ignore collisions when dead

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                // do nothing
                break;
            case "Finish":
                SuccessSequence();
                break;
            default:
                DeathSequence();
                break;
        }
    }

    private void SuccessSequence()
    {
        state = State.Transcending;
        audSource.Stop();
        audSource.PlayOneShot(success);
        successParticles.Play();
        Invoke("NextLevel", levelLoadDelay);
    }

    private void DeathSequence()
    {

        state = State.Dying;
        audSource.Stop();
        if (fuelSystem.startFuel <= 0)
            deathParticles.Play();
        else
        {
            audSource.PlayOneShot(death);
            deathParticles.Play();
        }

        Invoke("SameLevel", levelLoadDelay);
    }

    private void NextLevel()
    {
        int curIndex = SceneManager.GetActiveScene().buildIndex;
        if (levelpass < curIndex)
            PlayerPrefs.SetInt("LevelPassed", curIndex);
        if (curIndex == 4)
            SceneManager.LoadScene("menu");
        int nextIndex = curIndex + 1;
        SceneManager.LoadScene(nextIndex);

    }

    private void FirstLevel()
    {
        SceneManager.LoadScene(2);

    }
    private void SameLevel()
    {
        int curIndex = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(curIndex);

    }

    private void RespondToThrust()
    {
        if (!CrossPlatformInputManager.GetButton("Jump")) // can thrust while rotating
        {
            audSource.Stop();
            mainEngineParticles.Stop();

        }
        else
        {
            fuelSystem.fuelConsumptionRate = 8f;
            fuelSystem.ReduceFuel();
            ApplyThrust();


        }
    }

    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);


        if (!audSource.isPlaying) // so it doesn't layer
        {
            audSource.PlayOneShot(mainEngine);
        }
        mainEngineParticles.Play();

    }

    private void RespondToRotate()
    {
        rigidBody.freezeRotation = true;



        if (CrossPlatformInputManager.GetButton("Right"))
        {
            transform.Rotate(-Vector3.forward * rcsThrust * Time.deltaTime);

            fuelSystem.ReduceFuel();
        }
        else if (CrossPlatformInputManager.GetButton("Left"))
        {
            transform.Rotate(Vector3.forward * rcsThrust * Time.deltaTime);

            fuelSystem.ReduceFuel();
        }


        rigidBody.freezeRotation = false;
    }
}