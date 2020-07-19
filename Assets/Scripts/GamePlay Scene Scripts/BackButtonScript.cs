using UnityEngine;
using UnityEngine.UI;

public class BackButtonScript : MonoBehaviour
{
    // Serialized Variables
    public GameObject pauseMenuCanvas;

    // Non-Serialized Variables;
    public static bool isGamePaused = false;
    bool canTakeInput = true;
    bool shouldReset = false;
    bool isPauseMenuOpen = false;
    Animator pauseMenuPanelAnimator;

    // Start is called before the first frame update
    void Start()
    {
        isGamePaused = false;
        pauseMenuPanelAnimator = pauseMenuCanvas.GetComponent<Animator>();
        pauseMenuCanvas.SetActive(false);
        isPauseMenuOpen = false;
        canTakeInput = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(shouldReset) {
            if(pauseMenuPanelAnimator.GetCurrentAnimatorStateInfo(0).IsName("PauseMenuPanelIdle")) {
                pauseMenuPanelAnimator.ResetTrigger("shouldOpen");
                pauseMenuPanelAnimator.ResetTrigger("shouldClose");
                pauseMenuPanelAnimator.ResetTrigger("goToIdle");
                shouldReset = false;
                canTakeInput = true;
            }
        }

        if(canTakeInput) {
            if(Input.GetKeyDown(KeyCode.Escape) && !isPauseMenuOpen) {
                Debug.Log("User Pressed Back Button");
                Time.timeScale = 0;
                OpenPauseMenu();
                canTakeInput = false;
            } else if (Input.GetKeyDown(KeyCode.Escape) && isPauseMenuOpen) {
                Debug.Log("User Pressed Back Button");
                ClosePauseMenu();
                canTakeInput = true;
            }
        }

        if(isGamePaused) {
            if(pauseMenuPanelAnimator.GetCurrentAnimatorStateInfo(0).IsName("PauseMenuAnimationEnded")) {
                if(isPauseMenuOpen) {
                    pauseMenuPanelAnimator.ResetTrigger("shouldOpen");
                    pauseMenuPanelAnimator.SetTrigger("goToIdle");
                    shouldReset = true;
                } else {
                    pauseMenuPanelAnimator.ResetTrigger("shouldClose");
                    pauseMenuPanelAnimator.SetTrigger("goToIdle");
                    Time.timeScale = 1;
                    pauseMenuCanvas.SetActive(false);
                    isGamePaused = false;
                    shouldReset = true;
                }
            }
        }
    }

    void OpenPauseMenu() {
        isPauseMenuOpen = true;
        isGamePaused = true;
        pauseMenuCanvas.SetActive(true);
        pauseMenuPanelAnimator.ResetTrigger("goToIdle");
        pauseMenuPanelAnimator.SetTrigger("shouldOpen");
    }

    public void ClosePauseMenu() {
        isPauseMenuOpen = false;
        pauseMenuPanelAnimator.SetTrigger("shouldClose");
    }
}
