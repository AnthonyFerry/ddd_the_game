using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

    public string id;

    [Header("What is this menu title ?")]
    public string title;

    [Header("This text object will display the title just above.")]
    public Text titleText;

    [Header("If I press return, which menu should be shown ?")]
    public Menu parentMenu;

    [Header("In which GameObject should I create my buttons ?")]
    public Transform buttonContainer;

    [Header("Where the camera is supposed to look when i'm activated ?")]
    public Transform cameraTarget;

    public bool visible {  get { return _isVisible; } }

    bool _isVisible = false;
    CanvasGroup _cg;
    Animator _animator;

    void Start() {
        _cg = GetComponent<CanvasGroup>();
        _animator = GetComponent<Animator>();
    }

    void Update() {
        _isVisible = _cg.interactable && _cg.blocksRaycasts && _cg.alpha == 1;
    }

    public void Show() {
        Initialize();
        if (!_isVisible)
        {
            _animator.SetTrigger("Show");
            Camera.main.GetComponent<CameraMovement>().MoveToPosition(cameraTarget, 1f);
        }
    }

    public void Hide() {
        if (_isVisible)
            _animator.SetTrigger("Hide");
    }

    public void Initialize() {
        if (titleText != null)
            titleText.text = title;

        if (buttonContainer != null)
        {
            var buttons = buttonContainer.GetComponentInChildren<Transform>();
            foreach (Transform button in buttons)
                Destroy(button.gameObject);
        }
    }
}