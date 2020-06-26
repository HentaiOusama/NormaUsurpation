using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonClick : MonoBehaviour
{
    public void PlayGame() {
        SceneManager.LoadScene(1);
    }
}
