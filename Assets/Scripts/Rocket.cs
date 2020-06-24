using UnityEngine;
using UnityEngine.SceneManagement; 

public class Rocket : MonoBehaviour
{
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip objectiveReached;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] ParticleSystem objectiveParticles;
    
    Rigidbody rigidBody;
    AudioSource audioSource; 

    enum State {Alive, Dying, Transcending };
    State state = State.Alive;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive)
        {
        RespondToThrustInput();
        RespondToRotateInput();
        } 
    }

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive)
        {
            return;
        }
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                 // do nothing         
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }  
    }
    void StartSuccessSequence()
    {
        state = State.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(objectiveReached);
        objectiveParticles.Play();
        Invoke("LoadNextScene",3f); // paramaterise time   
    }
    void StartDeathSequence()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(death);
        deathParticles.Play();
        Invoke("permadeath",3f); // paramaterise time
    }
    void LoadNextScene()
    {
        SceneManager.LoadScene(1); //Todo allow for more than 2 levels
    }

    void permadeath()
    {
        SceneManager.LoadScene(0);
    }

    void RespondToRotateInput()
    {
        rigidBody.freezeRotation = true; // take manual control of the rotation
        float rotationThisFrame = rcsThrust * Time.deltaTime; 

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }

        rigidBody.freezeRotation = false; // resume physics control of rotation
    }

    void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        } 
        else
        {
            audioSource.Stop();
            mainEngineParticles.Stop();
        }
    }
    void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust);
         if (!audioSource.isPlaying)                 //So it doesn't layer
             audioSource.PlayOneShot(mainEngine);

     mainEngineParticles.Play();

    }
}
