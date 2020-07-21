using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtonClick : MonoBehaviour
{
    // Serialized Variables
    public int buttonId;
    public GameObject background;

    private Animator menuButtonAnimator;
    bool shouldReset = false;
    MainMenuScript mainMenuScript;

    void Start() {
        mainMenuScript = background.GetComponent<MainMenuScript>();
        menuButtonAnimator = gameObject.GetComponent<Animator>();
        menuButtonAnimator.ResetTrigger("didClick");
        menuButtonAnimator.ResetTrigger("goToIdle");
    }

    void Update() {
        if(shouldReset) {
            if(menuButtonAnimator.GetCurrentAnimatorStateInfo(0).IsName("MenuButtonIdle")) {
                menuButtonAnimator.ResetTrigger("didClick");
                menuButtonAnimator.ResetTrigger("goToIdle");
                shouldReset = false;
            }
        }

        if(menuButtonAnimator.GetCurrentAnimatorStateInfo(0).IsName("MenuButtonAnimationEnded")) {
            menuButtonAnimator.SetTrigger("goToIdle");
            menuButtonAnimator.ResetTrigger("didClick");
            shouldReset = true;
            
            switch(buttonId) {
                
                case 0:
                    PlayGame();
                    break;
                
                case 1:
                    OpenOptions();
                    break;

                case 2:
                    CloseApp();
                    break;


                case 5:
                    closeOptions();
                    break;
            }
        }
    }

    public void buttonClicked() {
        menuButtonAnimator.SetTrigger("didClick");
    }

    public void PlayGame() {
        SceneManager.LoadScene(1);
    }

    public void OpenOptions() {
        mainMenuScript.closeMMPAndOpenOP();
    }

    public void CloseApp() {
        Application.Quit();
    }

    public void closeOptions() {
        mainMenuScript.closeOPAndOpenMMP();
    }
}