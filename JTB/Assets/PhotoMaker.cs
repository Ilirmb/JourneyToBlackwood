using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoMaker : MonoBehaviour
{
    private Camera thisCamera;
    private void Awake()
    {
        thisCamera = this.GetComponent<Camera>();
    }
    /// <summary>
    /// Takes a Sprite photograph
    /// </summary>
    public Sprite TakePhotograph(int imageWidth, int imageHeight)
    {
        Debug.Log("Taking Profile Picture");
        RenderTexture rendTexture = new RenderTexture(imageWidth, imageHeight, 24);
        thisCamera.targetTexture = rendTexture;

        Texture2D photoTexture = new Texture2D(imageWidth, imageHeight, TextureFormat.RGB24, false);
        thisCamera.Render();

        // The Render Texture in RenderTexture.active is the one
        // that will be read by ReadPixels.
        RenderTexture.active = rendTexture;
        photoTexture.ReadPixels(new Rect(0, 0, imageWidth, imageHeight),0,0);
        photoTexture.Apply();
        thisCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rendTexture);

        return Sprite.Create(photoTexture,new Rect(0,0,imageWidth,imageHeight),new Vector2(0,0));
    }
}
