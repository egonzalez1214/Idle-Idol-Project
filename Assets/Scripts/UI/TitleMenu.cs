using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TitleMenu : MonoBehaviour {
    [SerializeField] private string gameSceneName = "Game"; // your main scene name
    [SerializeField] private TMP_Text versionLabel;

    void Start() {
        if (versionLabel)
            versionLabel.text = $"v{Application.version}";
    }

    public void OnStartGame() {
        SceneManager.LoadScene(gameSceneName);
    }

    public void OnQuitGame() {
        Application.Quit();
    }
}
