using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllusionWall : MonoBehaviour {
    private SpriteRenderer sprite;
    private bool fadeOut = false;
    private float referenceFloat;


    private void Start()
    {
        sprite = this.GetComponent<SpriteRenderer>();
        fadeOut = false;
        referenceFloat = 0f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            fadeOut = true;
        }
    }

    private void Update()
    {
        if (fadeOut)
        {
            float color = Mathf.SmoothDamp(sprite.color.a, 0f, ref referenceFloat, 1f);
            sprite.color = new Color(1, 1, 1, color);
            if(sprite.color.a == 0f)
            {
                fadeOut = false;
            }
        }
    }
}
