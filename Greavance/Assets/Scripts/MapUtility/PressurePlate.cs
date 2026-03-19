using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PressurePlate : MonoBehaviour
{
    [Header("Door Settings")]
    [SerializeField] private GameObject[] _doorsToOpen;
    [SerializeField] private bool _openDoors = true;

    [Header("Plate Settings")]
    [SerializeField] private bool _staysPressed = false;
    [SerializeField] private float _pressDuration = 2f;
    [SerializeField] private string[] _triggerTags = new string[] {"Player"};

    [Header("Visual Feedback")]
    [SerializeField] private Sprite _pressedSprite;
    [SerializeField] private Sprite _unpressedSprite;
    [SerializeField] private GameObject _pressedEffect;

    private SpriteRenderer _spriteRenderer;
    private int _objectsOnPlate = 0;
    private bool _isPressed = false;
    private Coroutine _resetCoroutine;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsValidTrigger(other.gameObject))
        {
            _objectsOnPlate++;

            if (!_isPressed)
            {
                ActivatePlate();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (IsValidTrigger(other.gameObject))
        {
            _objectsOnPlate--;

            if (_objectsOnPlate <= 0 && _isPressed && !_staysPressed)
            {
                DeactivatePlate();
            }
        }
    }

    private bool IsValidTrigger(GameObject obj)
    {
        foreach (string tag in _triggerTags)
        {
            if (obj.CompareTag(tag))
                return true;
        }
        return false;
    }

    private void ActivatePlate()
    {
        _isPressed = true;

        if (_spriteRenderer != null && _pressedSprite != null)
        {
            _spriteRenderer.sprite = _pressedSprite;
        }
        if (_pressedEffect != null)
        {
            Instantiate(_pressedEffect, transform.position, Quaternion.identity);
        }

        foreach (GameObject door in _doorsToOpen)
        {
            if (door != null)
            {
                Door doorScript = door.GetComponent<Door>();
                if (doorScript != null)
                {
                    if (_openDoors)
                    {
                        doorScript.Open();
                    }
                    else
                    {
                        doorScript.Close();
                    }
                }
            }
        }

        if (!_staysPressed && _resetCoroutine != null)
        {
            StopCoroutine(_resetCoroutine);
        }
    }

    private void DeactivatePlate()
    {
        _isPressed = false;

        if (_spriteRenderer != null && _unpressedSprite != null)
        {
            _spriteRenderer.sprite = _unpressedSprite;
        }

        foreach (GameObject door in _doorsToOpen)
        {
            if (door != null)
            {
                Door doorScript = door.GetComponent<Door>();
                if (doorScript != null)
                {
                    if (_openDoors)
                        doorScript.Close();
                    else
                        doorScript.Open();
                }
            }
        }
    }

    public void PressAndHold(float duration)
    {
        if (_resetCoroutine != null)
        {
            StopCoroutine(_resetCoroutine);

            ActivatePlate();
            _resetCoroutine = StartCoroutine(AutoReleaseAfter(duration));
        }
    }

    private IEnumerator AutoReleaseAfter(float duration)
    {
        yield return new WaitForSeconds(duration);
        if (_objectsOnPlate <= 0)
        {
            DeactivatePlate();
        }
        _resetCoroutine = null;
    }
}
