using System.CodeDom;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private TubeScriptableObject[] fallingSettings;

    public enum Scenes
    {
        Island,
        Falling_1,
        Level_1,
        Falling_2,
        Level_2,
    }

    private static SceneTransition instance;

    private static TubeScriptableObject tubeSettings;

    private void Awake()
    {
        instance = this;
    }

    public static void ChangeScene(Scenes scene)
    {
        switch(scene)
        {
            case Scenes.Island:
                SceneManager.LoadScene("Island");
                break;

            case Scenes.Falling_1:
                tubeSettings = instance.fallingSettings[0];
                SceneManager.LoadScene("Falling");
                break;

            case Scenes.Level_1:
                SceneManager.LoadScene("Buncer");
                break;

            case Scenes.Falling_2:
                tubeSettings = instance.fallingSettings[1];
                SceneManager.LoadScene("Falling");
                break;

            case Scenes.Level_2:
                SceneManager.LoadScene("Level 2");
                break;
        }
    }

    public static TubeScriptableObject GetTubeSettings => tubeSettings;
}
