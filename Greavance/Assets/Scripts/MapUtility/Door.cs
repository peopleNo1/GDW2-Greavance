using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Door : MonoBehaviour
{
    [Header("Door Settings")]
    [SerializeField] private bool _startOpen = false;
    [SerializeField] private float _moveDistance = 3f; // How much it moves
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private Vector2 _moveDirection = Vector2.up; // Direction the door moves. (0, 1) = up, (0, -1) = down, (-1, 0) = left, (1, 0) = right.

    private Vector2 _closedPosition;
    private Vector2 _openPosition;
    private Collider2D _doorCollider;
    private bool _isOpen = false;
    private Coroutine _moveCoroutine;

    private void Awake()
    {
        _doorCollider = GetComponent<Collider2D>();
        
        _closedPosition = transform.position;
        _openPosition = _closedPosition + (_moveDirection.normalized * _moveDistance);
        
        _isOpen = _startOpen;

        if (_startOpen)
        {
            transform.position = _openPosition;
            if (_doorCollider != null)
            {
                _doorCollider.enabled = false;
            }
        }
        else
        {
            transform.position = _closedPosition;
            if (_doorCollider != null)
            {
                _doorCollider.enabled = true;
            }
        }
    }

    public void Open()
    {
        if (_isOpen){return;}
        _isOpen = true;

        if (_doorCollider != null)
        {
            _doorCollider.enabled = false;
        }

        if (_moveCoroutine != null)
        {
            StopCoroutine(_moveCoroutine);
        }
        _moveCoroutine = StartCoroutine(MoveDoor(_openPosition));
    }

    public void Close()
    {
        if (!_isOpen){return;}
        _isOpen = false;
        
        if (_doorCollider != null)
            _doorCollider.enabled = true;
        
        if (_moveCoroutine != null)
            StopCoroutine(_moveCoroutine);
        _moveCoroutine = StartCoroutine(MoveDoor(_closedPosition));
    }

    public void Toggle()
    {
        if (_isOpen)
        {
            Close();
        }
        else
        {
            Open();
        }
    }

    private IEnumerator MoveDoor(Vector2 targetPosition)
    {
        while (Vector2.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(
                transform.position, 
                targetPosition, 
                _moveSpeed * Time.deltaTime
            );
            yield return null;
        }
        
        transform.position = targetPosition;
        _moveCoroutine = null;
    }
}
