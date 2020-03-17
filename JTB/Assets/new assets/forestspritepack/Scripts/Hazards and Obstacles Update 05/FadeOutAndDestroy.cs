using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutAndDestroy : MonoBehaviour
{
    public float fadeOutTimer;
    SpriteRenderer render;

    void Start()
    {
        render = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Color fade = render.color;
        fade.a = Mathf.MoveTowards(fade.a, 0, 1/fadeOutTimer * Time.deltaTime);
        render.color = fade;


        if (fade.a <= 0)
            gameObject.SetActive(false);
    }
}
