using TMPro;
using UnityEngine;
public class Level5KnockoutText : MonoBehaviour
{
    public TextMeshPro text;
    public float begin_fadeout_count;
    public RectTransform rect;
    void Awake()
    {
        text = GetComponent<TextMeshPro>();
        rect = GetComponent<RectTransform>();
        begin_fadeout_count = 2;
    }
    void Update()
    {
        begin_fadeout_count -= Time.unscaledDeltaTime;
        if (begin_fadeout_count <= 0)
        {
            begin_fadeout_count = 0;
            text.color = new Vector4(text.color.r, text.color.g, text.color.b, text.color.a - Time.unscaledDeltaTime / 2f);
            if (text.color.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
