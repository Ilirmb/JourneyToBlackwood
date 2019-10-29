using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelReset :MonoBehaviour , IPointerClickHandler
{
    public void OnPointerClick(PointerEventData data)
    {
        // reload the scene
        loadScene.ReloadCurrentScene();
    }


    private void Update()
    {
    }
}
