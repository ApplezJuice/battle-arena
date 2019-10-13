using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkillStatusPopup : MonoBehaviour
{
    private TextMeshPro tmp;
    private float lifetime;
    private Color textColor;
    private Vector3 moveVector;

    private const float DISAPPEAR_TIMER_MAX = 1f;

    // Start is called before the first frame update
    void Awake()
    {
        tmp = transform.GetComponent<TextMeshPro>();
    }

    public void Setup(string message)
    {
        textColor = tmp.color;
        tmp.SetText(message);
        lifetime = DISAPPEAR_TIMER_MAX;
    }

    // Update is called once per frame
    void Update()
    {
        if (lifetime > (DISAPPEAR_TIMER_MAX / 2))
        {
            // first half of popup
            float increaseScaleAmount = .2f;
            transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
        }
        else
        {
            // 2nd half
            //float decreaseScaleAmount = .2f;
            //transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
        }

        lifetime -= Time.deltaTime;
        if (lifetime < 0)
        {
            // start dissapearign
            float disappearSpeed = 1f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            tmp.color = textColor;
            if (textColor.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
