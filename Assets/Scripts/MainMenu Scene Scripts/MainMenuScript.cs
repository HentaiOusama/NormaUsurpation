using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
   // Serialized Variables
    public BackgroundData backgrounds;
    public int FPSTarget;
    public float defaultCameraOrthogonalSize;
    public float refrenceWidth, refrenceHeight;
    public GameObject MainMenuPanel, OptionsPanel;
    public Slider FShipMovementSensitivitySlider;
    public Toggle BGSoundToggle, SFXToggle;
    public Animator OP_PanelAnimator;
    public AudioSource bgAudioSource;



    // Non-Serialized Variables
    LBRTValues viewableScaleConstrains;
    float resizeRatio; // Only for canvases. No need to resize other stuff
    bool isMainMenuPanelOpen = true, isOptionsPanelOpen = false;
    StreamWriter optionsWriter;
    public static int bgSoundEnabled, SFXSoundEnabled, fShipMovementSensitivity;
    bool shouldResetTriggers = true;

    
    
    


    // Start is called before the first frame update
    void Start()
    {
        // Setting and Getting PlayerPerfs and setting slider values
        if(PlayerPrefs.GetInt("FirstRun") == 0) {
            PlayerPrefs.SetInt("FirstRun", 1);
            PlayerPrefs.SetInt("bgSoundEnabled", 1);
            PlayerPrefs.SetInt("SFXSoundEnabled", 1);
            PlayerPrefs.SetInt("FShipMovementSensitivity", 5);
        }
        bgSoundEnabled = PlayerPrefs.GetInt("bgSoundEnabled");
        SFXSoundEnabled = PlayerPrefs.GetInt("SFXSoundEnabled");
        fShipMovementSensitivity = PlayerPrefs.GetInt("FShipMovementSensitivity");
        FShipMovementSensitivitySlider.value = fShipMovementSensitivity;
        if(bgSoundEnabled == 1) {
            BGSoundToggle.isOn = true;
        } else {
            BGSoundToggle.isOn = false;
        }
        if(SFXSoundEnabled == 1) {
            SFXToggle.isOn = true;
        } else {
            SFXToggle.isOn = false;
        }



        // Handling MM_OP Panels
        isMainMenuPanelOpen = true;
        isOptionsPanelOpen = false;
        MainMenuPanel.SetActive(true);
        OptionsPanel.SetActive(false);
    
        
        
        // Setting Background and Adjusting Camera size
        setBackground(backgrounds.startBgIndex);
        float screenHeight = Screen.currentResolution.height;
        float screenWidth = Screen.currentResolution.width;
        float refrenceAspect = refrenceWidth/refrenceHeight;
        float screenAspect = screenWidth/screenHeight;
        Debug.Log("Main Menu");
        Debug.Log("refrenceWidth = " + refrenceWidth + " refrenceHeight = " + refrenceHeight + " refrenceAspect = " + refrenceAspect);
        Debug.Log("screenWidth = " + screenWidth + " screenHeight = " + screenHeight + " screenAspect = " + screenAspect);
        Debug.Log("cameraAspect = " + Camera.main.aspect);
        if(screenAspect >= 1) {
            Camera.main.orthographicSize = defaultCameraOrthogonalSize*refrenceAspect*screenAspect;
            resizeRatio = (float) System.Math.Round(((refrenceAspect*screenAspect)/4) + 0.75f, 3);
        } else {
            Camera.main.orthographicSize = defaultCameraOrthogonalSize*refrenceAspect/screenAspect;
            resizeRatio = (float) System.Math.Round(((refrenceAspect/screenAspect)/4) + 0.75f, 3);
        }
        Debug.Log("Resize Ratio = " + resizeRatio);
        // Sets the scale wise constrains of workspace on the background
        viewableScaleConstrains = new LBRTValues(-backgrounds.baseBackgroundWidth/2, -Camera.main.orthographicSize, 
                                                backgrounds.baseBackgroundWidth/2, Camera.main.orthographicSize);



        // Disables VSync for custom Frame Rate and sets custom FPS
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = FPSTarget;
    }




    // Update is called once per frame
    void Update()
    {
        // For FPS
        if(Application.targetFrameRate != FPSTarget) {
            Application.targetFrameRate = FPSTarget;
        }



        // Handling Actions when specific Panels are open
        if(isOptionsPanelOpen) {
            if(BGSoundToggle.isOn) {
                PlayerPrefs.SetInt("bgSoundEnabled", 1);
                bgAudioSource.volume = 0.5f;
            } else {
                PlayerPrefs.SetInt("bgSoundEnabled", 0);
                bgAudioSource.volume = 0;
            }
            if(SFXToggle.isOn) {
                PlayerPrefs.SetInt("SFXSoundEnabled", 1);
            } else {
                PlayerPrefs.SetInt("SFXSoundEnabled", 0);
            }
            PlayerPrefs.SetInt("FShipMovementSensitivity", (int) FShipMovementSensitivitySlider.value);
            if(Input.GetKeyDown(KeyCode.Escape)) {
                closeOPAndOpenMMP();
            }
        }

        if(isMainMenuPanelOpen) {
            if(Input.GetKeyDown(KeyCode.Escape)) {
                Application.Quit();
            }
        }



        // Handling Animator
        if(shouldResetTriggers) {
            OP_PanelAnimator.ResetTrigger("shouldOpen");
            OP_PanelAnimator.ResetTrigger("shouldClose");
            shouldResetTriggers = false;
        }

        if(OP_PanelAnimator.GetCurrentAnimatorStateInfo(0).IsName("MM_OP_Idle")) {
            if(isMainMenuPanelOpen) {
                isMainMenuPanelOpen = false;
                isOptionsPanelOpen = true;
                shouldResetTriggers = true;
                OP_PanelAnimator.SetTrigger("goBelow");
            } else if(isOptionsPanelOpen) {
                isMainMenuPanelOpen = true;
                isOptionsPanelOpen = false;
                shouldResetTriggers = true;
                OP_PanelAnimator.SetTrigger("goUp");
            }
        }        
    }


    // This function fixes the size of backgroud quad and sets it 
    public void setBackground(int index) {
        if( backgrounds.BGMaterialList[index].width != backgrounds.baseBackgroundWidth) {
            backgrounds.BGMaterialList[index].height = backgrounds.baseBackgroundWidth*backgrounds.BGMaterialList[index].height/backgrounds.BGMaterialList[index].width;
            backgrounds.BGMaterialList[index].width = backgrounds.baseBackgroundWidth;
        }
        gameObject.transform.localScale = new Vector3(backgrounds.BGMaterialList[index].width, backgrounds.BGMaterialList[index].height, 5);
    }

    
    public void closeMMPAndOpenOP() {
        OP_PanelAnimator.SetTrigger("shouldOpen");
    }

    public void closeOPAndOpenMMP() {
        OP_PanelAnimator.SetTrigger("shouldClose");
        bgAudioSource.volume = (float) PlayerPrefs.GetInt("bgSoundLevel")/10;
    }
}