using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class UIFunctions : MonoBehaviour
    {
        public void StartBtn()
        {
            SceneManager.LoadScene(Constants.MAIN_SCENE_INDEX); // Load Main Scene
        }

        public void QuitBtn()
        {
            Debug.LogWarning("Application Quited!");
            Application.Quit();
        }

        public void BackBtn()
        {
            SceneManager.LoadScene(Constants.MENU_SCENE_INDEX); // Load Menu scene
        }
    }
}