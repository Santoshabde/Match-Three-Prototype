using UnityEngine;
using UnityEngine.SceneManagement;

public class M3_GameSceneLoader : MonoBehaviour
{
    private void Start()
    {
        TestLoad();
    }

    public void TestLoad()
    {
        SceneManager.LoadSceneAsync(1);
    }
}
