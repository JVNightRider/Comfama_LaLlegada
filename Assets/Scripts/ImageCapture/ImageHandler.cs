using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageHandler : MonoBehaviour
{
    [SerializeField]
    private RenderTexture rtPostal;

    byte[] GetRTBytes() 
    {
        Texture2D tex = new Texture2D(rtPostal.width, rtPostal.height, TextureFormat.RGB24, false);
        RenderTexture.active = rtPostal;
        tex.ReadPixels(new Rect(0,0, rtPostal.width, rtPostal.height), 0,0);
        tex.Apply();
        byte[] textureBytes = tex.EncodeToPNG();
        Destroy(tex);
        return textureBytes;
    }

    public void SaveImage(string path) 
    {
        System.IO.File.WriteAllBytes(path, GetRTBytes());
    }

}
