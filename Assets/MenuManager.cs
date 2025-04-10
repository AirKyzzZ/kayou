using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject menuPanel;
    public Button openMenuButton; 
    public Button closeMenuButton; 
    public Slider volumeSlider; 
    public GameManager gameManager; 
    private bool isMenuOpen = false;

    public void Start()
    {
        openMenuButton.onClick.AddListener(OpenMenu);
        closeMenuButton.onClick.AddListener(CloseMenu);
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void OpenMenu()
    {
        menuPanel.SetActive(true);
        openMenuButton.gameObject.SetActive(false);
    }

    public void CloseMenu()
    {
        menuPanel.SetActive(false);
        openMenuButton.gameObject.SetActive(true);
    }

    public void SetVolume(float value)
    {
        AudioListener.volume = value; 
    }
}