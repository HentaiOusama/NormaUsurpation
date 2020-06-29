using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonClick : MonoBehaviour
{
    public int buttonId;

    private Animator menuButtonAnimator;
    bool shouldReset = false;

    void Start() {
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
            }
        }
    }

    public void buttonClicked() {
        menuButtonAnimator.SetTrigger("didClick");
    }

    public void PlayGame() {
        SceneManager.LoadScene(1);
    }


    // Not yet complete
    public void OpenOptions() {

    }
}
