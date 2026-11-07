using TMPro;
using UnityEngine;

public class Level5AreaEffectText : MonoBehaviour
{
    public TextMeshPro text;
    public float begin_fadeout_count;
    public RectTransform rect;

    void Awake()
    {
        text = GetComponent<TextMeshPro>();
        rect = GetComponent<RectTransform>();
        begin_fadeout_count = 3;
    }
    void Update()
    {
        begin_fadeout_count -= Time.unscaledDeltaTime;
        if (begin_fadeout_count <= 0 && text.color.a >= 0)
        {
            begin_fadeout_count = 0;
            text.color = new Vector4(text.color.r, text.color.g, text.color.b, text.color.a - Time.unscaledDeltaTime / 2f);
        }
    }
    public void set_visible(int new_text)
    {
        text.text = new_text.ToString();
        begin_fadeout_count = 1.25f;
        text.color = new Vector4(text.color.r, text.color.g, text.color.b, 1);
    }
}
