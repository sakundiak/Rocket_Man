using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float sceneDelay = 1f;
    [SerializeField] AudioClip crashAudio;
    [SerializeField] AudioClip successAudio;

    [SerializeField] ParticleSystem crashParticles;
    [SerializeField] ParticleSystem successParticles;

    AudioSource audioSource;    

    bool isTransitioning = false;
    bool collisionDisabled = false;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();       
    }
    void Update()
    {
        DebugKeys();
    }
    void DebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L)) 
        { 
            LoadNextLevel(); 
        }
        else if (Input.GetKeyDown(KeyCode.C)) 
        { 
            collisionDisabled = !collisionDisabled; //toggles collision 
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (isTransitioning || collisionDisabled)
        {
            return;
        }
        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("This object is friendly.");
                break;
            case "Finish":                
                NextLevelSequence();
                break;
            default:                
                StartCrashSequence();
                break;
        }

    }
    void StartCrashSequence()
    {        
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(crashAudio);        
        GetComponent<Movement>().enabled = false;
        crashParticles.Play();
        Invoke("ReloadLevel", sceneDelay);
    }
    void NextLevelSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(successAudio);        
        GetComponent<Movement>().enabled = false;
        successParticles.Play();
        Invoke("LoadNextLevel", sceneDelay);
    }
    void ReloadLevel()
    {        
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);        
    }

    void LoadNextLevel()
    {        
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
        Invoke("LoadNextLevel", sceneDelay);
    }
}
