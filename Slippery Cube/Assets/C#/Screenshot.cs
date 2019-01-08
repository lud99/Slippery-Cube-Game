using System;
using System.IO;
using System.Collections;
using UnityEngine;

public class Screenshot : MonoBehaviour
{
    public static Screenshot instance;

    Camera cam;

    //Get camera component
    public void Start()
    {
        instance = this;
        cam = GetComponent<Camera>();
    }

    //Take screenshot
    IEnumerator TakeScreenshot(int width, int height)
    {
        //Wait for end of frame (take screenshot of post processing and ui)
        yield return new WaitForEndOfFrame();

        //Create render texture to render screenshot onto
        cam.targetTexture = RenderTexture.GetTemporary(width, height, 16); //Set render texture to render screenshot on
        RenderTexture renderTexture = cam.targetTexture; //Set camera render texture

        //Get screenshot result
        Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false); //Saved screenshot rendertexture
        Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
        renderResult.ReadPixels(rect, 0, 0); //Read screenshot pixels

        //Save screenshot
        byte[] byteArray = renderResult.EncodeToPNG(); //Encode to png
        string path = Application.persistentDataPath + "/Screenshots/" + DateTime.Now.ToString("yyyy-MM-dd_hh.mm.ss") + ".png"; //File path
        string dirPath = Application.persistentDataPath + "/Screenshots"; //Folder path

        if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath); //Create 'Screenshots' folder if it doesn't exist
        File.WriteAllBytes(path, byteArray); //Save screenshot
        
        //Check if succesfull
        if (File.Exists(path)) //If saving was succesfull
        {
            Debug.Log("Saved screenshot to " + path);
        } else //If saving failed
        {
            Debug.Log("Could not save screenshot");
        }
        
        //Remove render texture
        RenderTexture.ReleaseTemporary(renderTexture); //Remove render texture
        cam.targetTexture = null; //Set rendertexture to none
    }

    //Take screenshot function
    public static void TakeScreenshot_Static(int width, int height)
    {
        instance.StartCoroutine(instance.TakeScreenshot(width, height));
    }
}
