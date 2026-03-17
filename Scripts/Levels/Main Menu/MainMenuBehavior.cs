using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuBehavior : MonoBehaviour
{
    public Button level_1;
    public Button level_2;
    public Button level_3;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        level_1.onClick.AddListener(() => load_level("TestBoss"));
        level_2.onClick.AddListener(() => load_level("TestBoss2"));
        level_3.onClick.AddListener(() => load_level("TestBoss3"));
    }
    public void load_level(string level)
    {
        SceneManager.LoadScene(level);
    }
}
