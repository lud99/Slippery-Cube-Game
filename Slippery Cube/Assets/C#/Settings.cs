using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour {

    public AudioMixer audioMixer;
    public GameObject preset, graphics, physics, vsync, fpsLimit, particles, lights, 
    aa, colorGrading, ambientOcclusion, dof, vignette, grain, bloom, motionBlur, masterSlider, 
    musicSlider, sfxSlider, showCoins, showProgressBar, showDeaths, autoRestart, touchFeedback, 
    autoNextLevel;
    string qualityLevel, fpsLimitString, vsyncString, ppString;
    Camera cam;
    PostProcessingSettings postSettings;

    //Start
    void Start()
    {
        postSettings = Camera.main.GetComponent<PostProcessingSettings>();

        //Create PlayerPrefs files if none exists (settings)
        if (!PlayerPrefs.HasKey("Quality"))
        {
            Preset(2);
            /*PlayerPrefs.SetInt("Quality", 0); //Graphics
            PlayerPrefs.SetInt("Physics", 60); //Physics
            PlayerPrefs.SetInt("FPSLimit", -1); //FPS Limit
            PlayerPrefs.SetInt("VSync", 0); //VSync
            PlayerPrefs.SetInt("ColorGrading", 0); //ColorGrading
            PlayerPrefs.SetInt("AmbientOcclusion", 0); //Ambient Occlusion
            PlayerPrefs.SetInt("AntiAliasing", 0); //AA
            PlayerPrefs.SetInt("MotionBlur", 0); //Motion Blur
            PlayerPrefs.SetInt("Vignette", 0); //Vignette
            PlayerPrefs.SetInt("Grain", 0); //Grain
            PlayerPrefs.SetInt("Bloom", 0); //Bloom
            PlayerPrefs.SetInt("DoF", 0); //DoF
            PlayerPrefs.SetInt("AntiAliasing", 0); //AA
            PlayerPrefs.SetInt("Particles", 0); //Particles
            PlayerPrefs.SetInt("Lights", 0); //Lights*/
            PlayerPrefs.SetInt("ShowCoins", 1); //Show coins
            PlayerPrefs.SetInt("ShowProgressBar", 1); //Show progress bar
            PlayerPrefs.SetInt("ShowDeaths", 0); //Show deaths
            PlayerPrefs.SetFloat("MasterVolume", 0f); //Master volume
            PlayerPrefs.SetFloat("MusicVolume", 0f); //Music volume
            PlayerPrefs.SetFloat("SfxVolume", 0f); //SFX volume
            PlayerPrefs.SetInt("MenuMusic", 0); //Menu music
        }
        //Load settings from file when loading scene
        UpdateSettings();
        if (graphics != null) UpdateUI();
    }

    //Set Game Master Volume
    public void SetMasterVolume(float volume)
    {
        PlayerPrefs.SetFloat("MasterVolume", volume);
        UpdateSettings();
    }
    //Set Music Volume
    public void SetMusicVolume(float volume)
    {
        PlayerPrefs.SetFloat("MusicVolume", volume);
        UpdateSettings();
    }
    //Set SFX Volume
    public void SetSFXVolume(float volume)
    {
        PlayerPrefs.SetFloat("SfxVolume", volume);
        UpdateSettings();
    }

    //Show coin counter
    public void ShowCoins(bool toggle)
    {
        if (toggle) PlayerPrefs.SetInt("ShowCoins", 1); else PlayerPrefs.SetInt("ShowCoins", 0);
    }
    //Show progress bar
    public void ShowProgressBar(bool toggle)
    {
        if (toggle) PlayerPrefs.SetInt("ShowProgressBar", 1); else PlayerPrefs.SetInt("ShowProgressBar", 0);
    }
    //Show death counter in level
    public void ShowDeaths(bool toggle)
    {
        if (toggle) PlayerPrefs.SetInt("ShowDeaths", 1); else PlayerPrefs.SetInt("ShowDeaths", 0);
    }
    //Touch feedback
    public void TouchFeedback(bool toggle)
    {
        if (toggle) PlayerPrefs.SetInt("TouchFeedback", 1); else PlayerPrefs.SetInt("TouchFeedback", 0);
    }
    //Auto next level
    public void AutoNextLevel(bool toggle)
    {
        if (toggle) PlayerPrefs.SetInt("AutoNextLevel", 1); else PlayerPrefs.SetInt("AutoNextLevel", 0);
    }
    //Auto restart
    public void AutoRestart(bool toggle)
    {
        if (toggle) PlayerPrefs.SetInt("AutoRestart", 1); else PlayerPrefs.SetInt("AutoRestart", 0);
    }

    //Preset
    public void Preset(int preset)
    {
        PlayerPrefs.SetInt("Physics", 60); //Physics

        if (preset == 0) //Low
        {
            PostPreset(preset);
            Particles(0);
            SetQuality(preset);
            Lights(0);
            VSync(0);
        }
        if (preset == 1) //Medium
        {
            PostPreset(preset);
            Particles(0);
            SetQuality(preset);
            Lights(1);
            VSync(0);
        }
        if (preset == 2) //High
        {
            PostPreset(preset);
            Particles(1);
            SetQuality(preset);
            Lights(1);
            VSync(0);
        }
        UpdateUI();
    }

    //PostProcessing Preset
    void PostPreset(int preset)
    {
        if (preset == 0) //Low
        {
            AA(0);
            AmbientOcclusion(0);
            ColorGrading(0);
            Vignette(0);
            Grain(0);
            Bloom(0);
            //MotionBlur(0);
            //Dof(0);
        }
        if (preset == 1) //Medium
        {
            AA(1);
            AmbientOcclusion(1);
            ColorGrading(1);
            Vignette(1);
            Grain(1);
            Bloom(1);
            //MotionBlur(0);
            //Dof(0);
        }
        if (preset == 2) //High
        {
            AA(3);
            AmbientOcclusion(1);
            ColorGrading(1);
            Vignette(1);
            Grain(1);
            Bloom(1);
            //MotionBlur(0);
            //Dof(1);
        }
    }

    //Set Graphics Quality
    public void SetQuality (int qualityIndex)
    {
        PlayerPrefs.SetInt("Quality", qualityIndex);
        UpdateSettings();
    }

    //Set AntiAlisaing
    public void AA(int aa)
    {
        PlayerPrefs.SetInt("AntiAliasing", aa);
        postSettings.AntiAliasing();
    }

    //Set Ambient Occlusion
    public void AmbientOcclusion(int ao)
    {
        PlayerPrefs.SetInt("AmbientOcclusion", ao);
        postSettings.AmbientOcclusion();
    }

    //Set Color grading
    public void ColorGrading(int toggle)
    {
        PlayerPrefs.SetInt("ColorGrading", toggle);
        postSettings.ColorGrading();
    }

    //Set Bloom
    public void Bloom(int toggle)
    {
        PlayerPrefs.SetInt("Bloom", toggle);
        postSettings.Bloom();
    }

    //Set Motion blur
    public void MotionBlur(int toggle)
    {
        PlayerPrefs.SetInt("MotionBlur", toggle);
        postSettings.MotionBlur();
    }

    //Set Dof
    public void Dof(int toggle)
    {
        PlayerPrefs.SetInt("DoF", toggle);
        postSettings.DoF();
    }

    //Set Vignett
    public void Vignette(int toggle)
    {
        PlayerPrefs.SetInt("Vignette", toggle);
        postSettings.Vignette();
    }

    //Set Grain
    public void Grain(int toggle)
    {
        PlayerPrefs.SetInt("Grain", toggle);
        postSettings.Grain();
    }

    //Physics
    public void Physics(int level)
    {
        if (level == 0) PlayerPrefs.SetInt("Physics", 60); //Medium
        if (level == 1) PlayerPrefs.SetInt("Physics", 100); //High

        UpdateSettings();
    }

    //V Sync
    public void VSync(int vsync)
    {
        PlayerPrefs.SetInt("VSync", vsync);
        UpdateSettings();
    }

    //FPS Limit
    public void TargetFrameRate(int fps)
    {
        if (fps == 0) PlayerPrefs.SetInt("FPSLimit", -1);
        if (fps == 1) PlayerPrefs.SetInt("FPSLimit", 30);
        if (fps == 2) PlayerPrefs.SetInt("FPSLimit", 60);

        UpdateSettings();
    }

    //Toggle Particles
    public void Particles(int toggle)
    {
        PlayerPrefs.SetInt("Particles", toggle);
    }

    //Toggle Lights
    public void Lights(int toggle)
    {
        PlayerPrefs.SetInt("Lights", toggle);
    }

    //Reset Settings / Save
    public void ResetSettings()
    {
        PlayerPrefs.DeleteAll();
        postSettings.Awake();
        System.GC.Collect();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //Reload scene to update UI and apply settings
    }

    //Apply PostProcessing Settings
    void UpdateSettings()
    {
        //Apply Graphics
        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("Quality"));
        //Apply Vsync
        QualitySettings.vSyncCount = PlayerPrefs.GetInt("VSync");
        //Apply FPS Limit
        Application.targetFrameRate = PlayerPrefs.GetInt("FPSLimit");
        //Apply Physics
        float physics = 1f / PlayerPrefs.GetInt("Physics");
        Time.fixedDeltaTime = physics;

        //Set volume
        audioMixer.SetFloat("MasterVolume", PlayerPrefs.GetFloat("MasterVolume"));
        audioMixer.SetFloat("MusicVolume", PlayerPrefs.GetFloat("MusicVolume"));
        audioMixer.SetFloat("SFXVolume", PlayerPrefs.GetFloat("SfxVolume"));
    }

    //Update UI
    void UpdateUI()
    {
        //Update volume sliders to display the correct values
        masterSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("MasterVolume");
        musicSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("MusicVolume");
        sfxSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("SfxVolume");

        //Update dropdown values to current settings
        //Preset
        preset.GetComponent<Dropdown>().value = 3;
        //Graphics
        graphics.GetComponent<Dropdown>().value = PlayerPrefs.GetInt("Quality");
        aa.GetComponent<Dropdown>().value = PlayerPrefs.GetInt("AntiAliasing");
        ambientOcclusion.GetComponent<Dropdown>().value = PlayerPrefs.GetInt("AmbientOcclusion");
        particles.GetComponent<Dropdown>().value = PlayerPrefs.GetInt("Particles");
        //Physics
        if (PlayerPrefs.GetInt("Physics") == 60) physics.GetComponent<Dropdown>().value = 0;
        if (PlayerPrefs.GetInt("Physics") == 100) physics.GetComponent<Dropdown>().value = 1;
        //Lights
        lights.GetComponent<Dropdown>().value = PlayerPrefs.GetInt("Lights");
        bloom.GetComponent<Dropdown>().value = PlayerPrefs.GetInt("Bloom");
        //PostProcessing
        colorGrading.GetComponent<Dropdown>().value = PlayerPrefs.GetInt("ColorGrading");
        vignette.GetComponent<Dropdown>().value = PlayerPrefs.GetInt("Vignette");
        grain.GetComponent<Dropdown>().value = PlayerPrefs.GetInt("Grain");
        motionBlur.GetComponent<Dropdown>().value = PlayerPrefs.GetInt("MotionBlur");
        dof.GetComponent<Dropdown>().value = PlayerPrefs.GetInt("DoF");
        //FPS Limit / VSync
        vsync.GetComponent<Dropdown>().value = PlayerPrefs.GetInt("VSync");
        fpsLimit.GetComponent<Dropdown>().value = PlayerPrefs.GetInt("FPSLimit");

        //Update toggle values to current settings
        if (PlayerPrefs.GetInt("ShowCoins") == 0) showCoins.GetComponent<Toggle>().isOn = false; else showCoins.GetComponent<Toggle>().isOn = true;
        if (PlayerPrefs.GetInt("ShowDeaths") == 0) showDeaths.GetComponent<Toggle>().isOn = false; else showDeaths.GetComponent<Toggle>().isOn = true;
        if (PlayerPrefs.GetInt("ShowProgressBar") == 0) showProgressBar.GetComponent<Toggle>().isOn = false; else showProgressBar.GetComponent<Toggle>().isOn = true;
        if (PlayerPrefs.GetInt("AutoRestart") == 0) autoRestart.GetComponent<Toggle>().isOn = false; else autoRestart.GetComponent<Toggle>().isOn = true;
        if (PlayerPrefs.GetInt("AutoNextLevel") == 0) autoNextLevel.GetComponent<Toggle>().isOn = false; else autoNextLevel.GetComponent<Toggle>().isOn = true;
        if (PlayerPrefs.GetInt("TouchFeedback") == 0) touchFeedback.GetComponent<Toggle>().isOn = false; else touchFeedback.GetComponent<Toggle>().isOn = true;
    }
}

