using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionEvent : MonoBehaviour
{
    public static SceneTransitionEvent Instance;

    [SerializeField] private string sceneToLoad;
    [SerializeField] private Animator animator;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (animator == null)
                animator = GetComponent<Animator>();

            SceneManager.sceneLoaded += HandleSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadSceneFromAnimation()
    {
        SceneLoader.Instance.LoadScene(sceneToLoad);
    }

    void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Loaded");
        animator.SetBool("Loaded", true);
    }

    public void DestroyObject()
    {
        SceneManager.sceneLoaded -= HandleSceneLoaded;

        if (Instance == this)
            Instance = null;

        Destroy(gameObject);
    }
}