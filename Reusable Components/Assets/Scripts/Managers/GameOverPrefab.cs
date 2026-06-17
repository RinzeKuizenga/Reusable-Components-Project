using UnityEngine;

public class DebugLoader : MonoBehaviour
{
    [SerializeField] private string sceneName;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            SceneLoader.Instance.LoadScene(sceneName);
        }
    }
}
