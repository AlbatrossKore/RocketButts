using UnityEngine;
using UnityEngine.SceneManagement; 

public class Rocket : MonoBehaviour
{
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    Rigidbody rigidBody;
    AudioSource audioSource; 

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Thrust();
        Rotate();
    }

    void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                 // do nothing
                print("OK"); //todo remove            
                break;
            case "Finish":
                print("hit finish");
                SceneManager.LoadScene(1);
                break;
            default:
                print("dead");
                SceneManager.LoadScene(0);
                break;
        }   
    }

    private void Rotate()
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

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * mainThrust);
            if (!audioSource.isPlaying)                 //So it doesn't layer
                audioSource.Play();
        }
        else
        {
            audioSource.Stop();
        }
    }
}
