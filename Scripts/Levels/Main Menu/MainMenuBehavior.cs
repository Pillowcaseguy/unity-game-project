using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuBehavior : MonoBehaviour
{
    public Button tutorial;
    public Button level_1;
    public Button level_2;
    public Button level_3;
    public Button level_4;
    public Button level_5;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tutorial.onClick.AddListener(() => load_level("Tutorial"));
        level_1.onClick.AddListener(() => load_level("TestBoss"));
        level_2.onClick.AddListener(() => load_level("TestBoss2"));
        level_3.onClick.AddListener(() => load_level("TestBoss3"));
        level_4.onClick.AddListener(() => load_level("TestBoss4"));
        level_5.onClick.AddListener(() => load_level("TestBoss5"));
    }
    public void load_level(string level)
    {
        SceneManager.LoadScene(level);
    }
}
