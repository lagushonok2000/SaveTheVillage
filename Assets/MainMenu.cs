using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _rulesButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private Button _exitRulesButton;
    [SerializeField] private GameObject _rulesPanel;
    void Start()
    {
        _playButton.onClick.AddListener(PlayButton);
        _rulesButton.onClick.AddListener(RulesButton);
        _exitButton.onClick.AddListener(ExitButton);
        _exitRulesButton.onClick.AddListener(ExitRulesButton);
    }
    
    private void PlayButton()
    {
        SceneManager.LoadScene(1);
    }

    private void RulesButton()
    {
        _rulesPanel.SetActive(true);
    }

    private void ExitButton()
    {
        Application.Quit();
    }

    private void ExitRulesButton()
    {
        _rulesPanel.SetActive(false);
    }
}
