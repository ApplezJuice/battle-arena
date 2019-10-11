using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    private TextMeshPro tmp;
    private float lifetime;
    private Color textColor;
    private Vector3 moveVector;

    private const float DISAPPEAR_TIMER_MAX = 1f;

    private void Awake()
    {
        tmp = transform.GetComponent<TextMeshPro>();
    }
    public void Setup(int dmg, bool critical)
    {
        tmp.SetText(dmg.ToString());
        if (!critical)
        {
            textColor = new Color(219f / 255f , 167f / 255f, 20f / 255f);
            tmp.fontSize = 5;
        }
        else
        {
            textColor = Color.red;
            tmp.fontSize = 7;
        }

        tmp.color = textColor;
        
        lifetime = DISAPPEAR_TIMER_MAX;

        moveVector = new Vector3(Random.Range(-.7f, .7f), 1) * 7f;
    }

    private void Update()
    {
        transform.position += moveVector * Time.deltaTime;
        moveVector -= moveVector * 8f * Time.deltaTime;

        if (lifetime > (DISAPPEAR_TIMER_MAX / 2))
        {
            // first half of popup
            float increaseScaleAmount = 1f;
            transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
        }
        else
        {
            // 2nd half
            float decreaseScaleAmount = 1f;
            transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
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
