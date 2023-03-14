using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Player _player;

    [SerializeField] private Image _joystickArea;
    [SerializeField] private Image _joystickBackground;
    [SerializeField] private Image _joystick;

    private Vector2 _startPosition;
    private Vector2 _inputVector;

    [SerializeField] private Color _activeJoystickColor;
    [SerializeField] private Color _inActiveJoystickColor;

    private bool _isJoystickActive = false;

    void Start()
    {
        JoystickClick();

        _startPosition = _joystickBackground.rectTransform.anchoredPosition;
    }

    private void Update()
    {
        if (_inputVector.x != 0 || _inputVector.y != 0)
        {
            _player.Move(new Vector3(_inputVector.x, 0, _inputVector.y));
            _player.Rotate(new Vector3(_inputVector.x, 0, _inputVector.y));
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 joystickPosition;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_joystickBackground.rectTransform, 
            eventData.position, null, out joystickPosition))
        {
            joystickPosition.x = (joystickPosition.x * 2 / _joystickBackground.rectTransform.sizeDelta.x);
            joystickPosition.y = (joystickPosition.y * 2 / _joystickBackground.rectTransform.sizeDelta.y);

            _inputVector = new Vector2(joystickPosition.x, joystickPosition.y);

            _inputVector = (_inputVector.magnitude > 1f) ? _inputVector.normalized : _inputVector;

            _joystick.rectTransform.anchoredPosition = new Vector2(_inputVector.x * (_joystickBackground.rectTransform.sizeDelta.x / 2), 
                _inputVector.y * (_joystickBackground.rectTransform.sizeDelta.y / 2));
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _player.Run(true);

        JoystickClick();

        Vector2 joystickBackgroundPosition;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_joystickArea.rectTransform, 
            eventData.position, null, out joystickBackgroundPosition))
        {
            _joystickBackground.rectTransform.anchoredPosition = new Vector2(joystickBackgroundPosition.x, joystickBackgroundPosition.y);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _player.Run(false);

        _joystickBackground.rectTransform.anchoredPosition = _startPosition;

        JoystickClick();

        _inputVector = Vector2.zero;
        _joystick.rectTransform.anchoredPosition = Vector2.zero;
    }

    private void JoystickClick()
    {
        if (!_isJoystickActive)
        {
            _joystick.color = _activeJoystickColor;
            _joystickBackground.color = _activeJoystickColor;
            _isJoystickActive = true;
        }
        else
        {
            _joystick.color = _inActiveJoystickColor;
            _joystickBackground.color = _inActiveJoystickColor;
            _isJoystickActive = false;
        }
    }
}
