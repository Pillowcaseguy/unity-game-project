using TMPro;
using UnityEngine;

public class Level5VictoryText : MonoBehaviour
{
    public TextMeshPro text;
    public float begin_fadeout_count;
    public RectTransform rect;
    public bool fading_in;
    void Awake()
    {
        text = GetComponent<TextMeshPro>();
        rect = GetComponent<RectTransform>();
        begin_fadeout_count = 3;
        fading_in = true;
    }
    void Update()
    {
        if (fading_in)
        {
            text.color = new Vector4(text.color.r, text.color.g, text.color.b, text.color.a + Time.unscaledDeltaTime / 2f);
        }
        begin_fadeout_count -= Time.unscaledDeltaTime;
        if (begin_fadeout_count <= 0)
        {
            fading_in = false;
            begin_fadeout_count = 0;
            text.color = new Vector4(text.color.r, text.color.g, text.color.b, text.color.a - Time.unscaledDeltaTime / 2f);
            if (text.color.a <= 0 && !fading_in)
            {
                Destroy(gameObject);
            }
        }
    }
}
