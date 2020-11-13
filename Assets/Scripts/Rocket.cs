using UnityEngine;
using UnityEngine.SceneManagement; 

public class Rocket : MonoBehaviour
{
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float levelLoadDelay = 3f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip objectiveReached;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] ParticleSystem objectiveParticles;
    
    Rigidbody rigidBody;
    AudioSource audioSource; 

    bool isTransitioning = false;
    bool collisionsDisabled = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTransitioning)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        } 

        #if UNITY_EDITOR // this will make it so the debug keys are only functinoal while the game is open in the editor! 
         if (Input.GetKeyDown(KeyCode.C))
        {
            collisionsDisabled = !collisionsDisabled; // not false so it toggles. 

            // Call UI Element to let user know debug key has been activated
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextScene();
        }    
        #endif  
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isTransitioning || collisionsDisabled)
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
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(objectiveReached);
        objectiveParticles.Play();
        Invoke("LoadNextScene",levelLoadDelay); 
    }
    void StartDeathSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(death);
        deathParticles.Play();
        Invoke("permadeath",levelLoadDelay); 
    }
    void LoadNextScene()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if(nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0; // Loop back to start
        }
        SceneManager.LoadScene(nextSceneIndex);

    }

    void permadeath()
    {
          SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // For testing purposes, we're reloading the current scene now! :3

    }

    void RespondToRotateInput()
    {
        rigidBody.angularVelocity = Vector3.zero; // remove rotation due to physics
        
        float rotationThisFrame = rcsThrust * Time.deltaTime; 
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
    }

    void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        } 
        else
        {
            StopApplyingThrust();
        }
    }

    void StopApplyingThrust()
    {
        audioSource.Stop();
        mainEngineParticles.Stop();
    }

    void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
         if (!audioSource.isPlaying)                 //So it doesn't layer
             audioSource.PlayOneShot(mainEngine);

     mainEngineParticles.Play();

    }
}
