using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingSettings : MonoBehaviour
{
    PostProcessVolume volume;
    PostProcessLayer layer;
    public bool onlyOneCamera = true;
    public bool dontDestroyOnLoad = true;

    //Start of game
    public void Awake()
    {
       if (dontDestroyOnLoad) DontDestroyOnLoad(this); //Be active in all scenes 

        //Destroy this camera if there are more than 1 currently active
        if (onlyOneCamera)
        {
            if (GameObject.FindGameObjectsWithTag("MainCamera").Length > 1) Destroy(this.gameObject);
        } else //Destroy other camera
        {
            if (GameObject.Find("Main Camera")) Destroy(GameObject.Find("Main Camera"));
        }
        

        //Get post processing components
        volume = GetComponent<PostProcessVolume>();
        layer = GetComponent<PostProcessLayer>();

        //Update all post processing effects on start
        AntiAliasing();
        AmbientOcclusion();
        ColorGrading();
        Bloom();
        Vignette();
        Grain();
        DoF();
        MotionBlur();
    }

    //Update AntiAliasing
    public void AntiAliasing()
    {
        //Off
        if (PlayerPrefs.GetInt("AntiAliasing") == 0) layer.antialiasingMode = PostProcessLayer.Antialiasing.None;
        //FXAA
        else if (PlayerPrefs.GetInt("AntiAliasing") == 1)
        {
            layer.antialiasingMode = PostProcessLayer.Antialiasing.FastApproximateAntialiasing;
            layer.fastApproximateAntialiasing.fastMode = true;
        }
        //SMAA
        else if (PlayerPrefs.GetInt("AntiAliasing") == 2) layer.antialiasingMode = PostProcessLayer.Antialiasing.SubpixelMorphologicalAntialiasing;
        //TAA
        else if (PlayerPrefs.GetInt("AntiAliasing") == 3) layer.antialiasingMode = PostProcessLayer.Antialiasing.TemporalAntialiasing;
    }
    //Update Ambient Occlusion
    public void AmbientOcclusion()
    {
        AmbientOcclusion ambientOcclusionLayer;
        volume.profile.TryGetSettings(out ambientOcclusionLayer);
        //Enable or disable
        if (PlayerPrefs.GetInt("AmbientOcclusion") == 0) ambientOcclusionLayer.enabled.value = false; else ambientOcclusionLayer.enabled.value = true;
    }
    //Update Color grading
    public void ColorGrading()
    {
        ColorGrading colorGradingLayer;
        volume.profile.TryGetSettings(out colorGradingLayer); //Get Color grading
        //Enable or disable
        if (PlayerPrefs.GetInt("ColorGrading") == 0) colorGradingLayer.enabled.value = false; else colorGradingLayer.enabled.value = true;
    }
    //Update Bloom
    public void Bloom()
    {
        Bloom bloomLayer;
        volume.profile.TryGetSettings(out bloomLayer); //Get Bloom
        //Enable or disable
        if (PlayerPrefs.GetInt("Bloom") == 0) bloomLayer.enabled.value = false; else bloomLayer.enabled.value = true;
    }
    //Update Vignette
    public void Vignette()
    {
        Vignette vignetteLayer;
        volume.profile.TryGetSettings(out vignetteLayer); //Get Vignette
        //Enable or disable
        if (PlayerPrefs.GetInt("Vignette") == 0) vignetteLayer.enabled.value = false; else vignetteLayer.enabled.value = true;
    }
    //Update Grain
    public void Grain()
    {
        Grain grainLayer;
        volume.profile.TryGetSettings(out grainLayer); //Get Grain
        //Enable or disable
        if (PlayerPrefs.GetInt("Grain") == 0) grainLayer.enabled.value = false; else grainLayer.enabled.value = true;
    }
    //Update DoF
    public void DoF()
    {
        DepthOfField dofLayer;
        volume.profile.TryGetSettings(out dofLayer); //Get DoF
        //Enable or disable
        if (PlayerPrefs.GetInt("DoF") == 0) dofLayer.enabled.value = false; else dofLayer.enabled.value = true;
    }
    //Update Motion blur
    public void MotionBlur()
    {
        MotionBlur motionBlurLayer;
        volume.profile.TryGetSettings(out motionBlurLayer); //Get Motion blur
        //Enable or disable
        if (PlayerPrefs.GetInt("MotionBlur") == 0) motionBlurLayer.enabled.value = false; else motionBlurLayer.enabled.value = true;
    }
}
