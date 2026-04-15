using UnityEngine;
using UnityEngine.UI;

public class MainWeaponButtons : MonoBehaviour
{
    public bool forward;

    public Button arrow;

    void Start()
    {
        arrow.onClick.AddListener(() => cycle_weapon(forward));
    }
    public void cycle_weapon(bool increment)
    {
        int step = increment ? 1 : -1;
        int len = GameManager.implemented_weapons.Length;

        GameManager.Instance.first_list_index = (GameManager.Instance.first_list_index + step + len) % len;
        if (GameManager.Instance.first_list_index == GameManager.Instance.second_list_index)
        {
            GameManager.Instance.first_list_index = (GameManager.Instance.first_list_index + step + len) % len;
        }
    }
    /*void OnMouseDown()
    {
        Debug.Log("hjnhtrhnujntrh");
        int step = forward ? 1 : -1;
        GameManager.Instance.first_list_index = (GameManager.Instance.first_list_index + step) % GameManager.implemented_weapons.Length;
    }*/
}
