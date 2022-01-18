using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float crashDelay = 1f;
    [SerializeField] float nextLevelDelay = 1f;
    [SerializeField] AudioClip crashClip;
    [SerializeField] AudioClip successClip;
    [SerializeField] ParticleSystem crashParticles;
    [SerializeField] ParticleSystem successParticles;

    private AudioSource currentAudioSource;

    private bool isTransitioning = false;
    private bool isCheatEnabled = false;

    void Start()
    {
        currentAudioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        ProcessCheats();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (isTransitioning || isCheatEnabled)
        {
            return;
        }

        string tag = other.gameObject.tag;
        switch (tag)
        {
            case "Friendly":
                // TODO
                break;
            case "Finish":
                StartNextLevelSequence();
                other.gameObject.GetComponent<Oscillator>()?.Pause();
                break;
            default:
                StartCrashSequence();
                break;
        }
    }

    private void StartCrashSequence()
    {
        isTransitioning = true;
        Movement movement = GetComponent<Movement>();
        if(movement) {
            movement.enabled = false;
        }
        crashParticles.Play();
        playOneShot(crashClip);
        Invoke("Respawn", crashDelay);
    }

    private void playOneShot(AudioClip clip)
    {
        currentAudioSource.Stop();
        currentAudioSource.PlayOneShot(clip);
    }

    private void StartNextLevelSequence()
    {
        isTransitioning = true;
        GetComponent<Movement>().enabled = false;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
        successParticles.Play();
        playOneShot(successClip);
        Invoke("NextLevel", nextLevelDelay);
    }

    private void NextLevel()
    {
        ChangeLevel(1);
    }

    private void PreviousLevel()
    {
        ChangeLevel(-1);
    }

    private void ChangeLevel(int offset)
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + offset;

        try
        {
            Scene nextScene = SceneManager.GetSceneByBuildIndex(nextSceneIndex);
            SceneManager.LoadScene(nextSceneIndex);
        }
        catch
        {
            SceneManager.LoadScene(0);
        }
    }

    private void Respawn()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    private void ProcessCheats()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            NextLevel();
            return;
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            PreviousLevel();
            return;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {   
            isCheatEnabled = !isCheatEnabled;
            Debug.Log("toggle " + isCheatEnabled.ToString());
        }
    }
}
