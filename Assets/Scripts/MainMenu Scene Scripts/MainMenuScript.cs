using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
   // Serialized Variables
    public BackgroundData backgrounds;
    public int FPSTarget;
    public float defaultCameraOrthogonalSize;
    public float refrenceWidth;
    public float refrenceHeight;


    // Non-Serialized Variables
    LBRTValues viewableScaleConstrains;
    float resizeRatio; // Only for canvases. No need to resize other stuff



    // Start is called before the first frame update
    void Start()
    {
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
    }


    // This function fixes the size of backgroud quad and sets it 
    public void setBackground(int index) {
        if( backgrounds.BGMaterialList[index].width != backgrounds.baseBackgroundWidth) {
            backgrounds.BGMaterialList[index].height = backgrounds.baseBackgroundWidth*backgrounds.BGMaterialList[index].height/backgrounds.BGMaterialList[index].width;
            backgrounds.BGMaterialList[index].width = backgrounds.baseBackgroundWidth;
        }
        gameObject.transform.localScale = new Vector3(backgrounds.BGMaterialList[index].width, backgrounds.BGMaterialList[index].height, 5);
    }

}
