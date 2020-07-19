using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuButtonClick : MonoBehaviour
{
    // Serialized Variables
    public int buttonId;
    public Animator pauseMenuButtonAnimator;
    public GameObject background;

    // Non-Serialized Variables
    bool shouldReset = false;

    void Start() {
        pauseMenuButtonAnimator.ResetTrigger("didClick");
        pauseMenuButtonAnimator.ResetTrigger("goToIdle");
    }

    void Update() {
        if(shouldReset) {
            if(pauseMenuButtonAnimator.GetCurrentAnimatorStateInfo(0).IsName("PauseMenuIdle")) {
                pauseMenuButtonAnimator.ResetTrigger("didClick");
                pauseMenuButtonAnimator.ResetTrigger("goToIdle");
                shouldReset = false;
            }
        }

        if(pauseMenuButtonAnimator.GetCurrentAnimatorStateInfo(0).IsName("PauseMenuButtonAnimationEnded")) {
            pauseMenuButtonAnimator.SetTrigger("goToIdle");
            pauseMenuButtonAnimator.ResetTrigger("didClick");
            shouldReset = true;
            
            switch(buttonId) {
                
                case 0:
                    Resume();
                    break;
                
                case 1:
                    Restart();
                    break;

                case 2:
                    GoToMainMenu();
                    break;
            }
        }
    }

    public void buttonClicked() {
        Debug.Log("Button Clicked");
        pauseMenuButtonAnimator.SetTrigger("didClick");
    }

    public void GoToMainMenu() {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }


    // Not yet complete
    public void Resume() {
        background.GetComponent<BackButtonScript>().ClosePauseMenu();
    }

    public void Restart() {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}