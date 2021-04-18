using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class WaterBehaviour : MonoBehaviour
{
    public float effectAmplitude = 2.0f;
    public float effectReduction = 0.98f;
    public float effectSpread = 0.1f;

    private bool effectEnabled = false;

    private float effectStart = 0;

    private Material GetMaterial() 
    {
        return GetComponent<Renderer>().sharedMaterial;
    }
    void Update()
    {
        if(effectEnabled){
            var material = GetMaterial();
            var effectAmplitude =material.GetFloat("_EffectAmplitude");
            var effectRadius = material.GetFloat("_EffectRadius");
            
            if (effectAmplitude > 0.01) {
                effectAmplitude *= effectReduction;
                effectRadius += effectSpread;
            } else {
                effectAmplitude = 0;
                effectRadius = 0;
                effectEnabled = false;
            }
            material.SetFloat("_EffectAmplitude", effectAmplitude);
            material.SetFloat("_EffectRadius", effectRadius);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.isTrigger) {
            return ;
        }

        RaycastHit2D hit;
        var rigidbody = other.GetComponent<Rigidbody2D>();
        var pos2D = new Vector2(other.transform.position.x, other.transform.position.y);
        if (hit = Physics2D.Raycast(pos2D, rigidbody.velocity))
        {
            var position = new Vector3(hit.point.x, hit.point.y, transform.position.y);
            var material = GetMaterial();
            material.SetVector("_EffectPosition", hit.point);
            material.SetFloat("_EffectAmplitude", effectAmplitude);
            material.SetFloat("_EffectRadius",  0.5f);
            //material.SetFloat("_EffectStart", 0);
            effectEnabled = true;
        }
     }
}
