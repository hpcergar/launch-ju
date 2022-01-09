using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float mainThrust = 1000f;
    [SerializeField] float rotationThrust = 100f;
    [SerializeField] AudioClip mainEngineClip;
    [SerializeField] ParticleSystem mainThrustParticles;
    [SerializeField] ParticleSystem leftThrustParticles;
    [SerializeField] ParticleSystem rightThrustParticles;

    private Rigidbody currentRigidbody;
    private AudioSource currentAudioSource;

    private bool isAlive;

    // Start is called before the first frame update
    void Start()
    {
        currentRigidbody = GetComponent<Rigidbody>();
        currentAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    private void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            StartThrusting();
        }
        else
        {
            StopThrusting();
        }
    }

    private void ProcessRotation()
    {
        bool left = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A);
        bool right = Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);

        if (right && left)
        {
            return;
        }

        if (left)
        {
            StartRotation(Vector3.forward, rightThrustParticles);
        }
        else if (right)
        {
            StartRotation(Vector3.back, leftThrustParticles);
        }
        else
        {
            StopRotation();
        }
    }


    private void StartThrusting()
    {
        currentRigidbody.AddRelativeForce(Vector3.up * adaptDeltaValue(mainThrust));
        if (false == currentAudioSource.isPlaying)
        {
            currentAudioSource.PlayOneShot(mainEngineClip);
        }
        if (false == mainThrustParticles.isEmitting)
        {
            mainThrustParticles.Play();
        }
    }

    private void StopThrusting()
    {
        mainThrustParticles.Stop();
        if (true == currentAudioSource.isPlaying)
        {
            currentAudioSource.Stop();
        }
    }

    private void StartRotation(Vector3 vector, ParticleSystem particleSystem)
    {
        currentRigidbody.freezeRotation = true;
        transform.Rotate(vector * adaptDeltaValue(rotationThrust));
        currentRigidbody.freezeRotation = false;
        if (false == particleSystem.isEmitting)
        {
            particleSystem.Play();
        }
    }

    private void StopRotation()
    {
        leftThrustParticles.Stop();
        rightThrustParticles.Stop();
    }

    private float adaptDeltaValue(float value)
    {
        return Time.deltaTime * value;
    }
}
