using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsController : MonoBehaviour
{
    [SerializeField] private MenuButton _stopButton, _optionsButton, _quitButton, _creditsButton, _returnButton;
    [SerializeField] private Lever _lever;
    [SerializeField] private GameObject _optionsPanel, _creditsPanel;

    private Animator _camAnimator;

    private void Awake()
    {
        _camAnimator = Camera.main.GetComponent<Animator>();   
    }

    private void Start()
    {
        _creditsButton.Pressed.AddListener(ToggleCredits);
        _returnButton.Pressed.AddListener(ReturnToSlot);

        _quitButton.Pressed.AddListener(QuitGame);
        _optionsButton.Pressed.AddListener(OptionsScreen);

        _creditsButton.DisableButton();
        _returnButton.DisableButton();
    }

    private void OptionsScreen(MenuButton button)
    {
        _camAnimator.SetBool("Options", true);
        _camAnimator.SetBool("Return", false);

        _lever.DisableLever();
        _optionsButton.DisableButton();
        _quitButton.DisableButton();

        _creditsButton.EnableButton();
        _returnButton.EnableButton();

        _creditsPanel.SetActive(false);
        _optionsPanel.SetActive(true);
    }

    private void ReturnToSlot(MenuButton button)
    {
        _camAnimator.SetBool("Options", false);
        _camAnimator.SetBool("Return", true);

        _creditsButton.DisableButton();
        _returnButton.DisableButton();

        Invoke(nameof(ResetToSlotState), 1f);

    }

    private void ResetToSlotState()
    {
        _creditsPanel.SetActive(false);
        _optionsPanel.SetActive(false);

        _lever.EnableLever();
        _optionsButton.EnableButton();
        _quitButton.EnableButton();
    }

    private void ToggleCredits(MenuButton button)
    {
        _creditsPanel.SetActive(!_creditsPanel.activeSelf);
        _optionsPanel.SetActive(!_creditsPanel.activeSelf);
        Invoke(nameof(ResetCreditsButton), 0.2f);
    }

    private void ResetCreditsButton() => _creditsButton.EnableButton();


    private void QuitGame(MenuButton button)
    {
        Invoke(nameof(Quit), 0.5f);
    }

    private void Quit() => Application.Quit();
}
