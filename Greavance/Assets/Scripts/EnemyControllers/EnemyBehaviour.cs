using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class EnemyBehaviour : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float leftBoundary = -5f;
    [SerializeField] private float rightBoundary = 5f;
    [SerializeField] private float speed = 2f;
    [SerializeField] private bool switchSides = true; // True = right, False = left
    
    [Header("References")]
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;

    [Header("Border Controls")]
    private Vector2 startingPos;
    private float currentTargetX;

    void Start()
    {
        startingPos = transform.position;
        Debug.Log($"Enemy spawn at position: {startingPos}");

        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();

        if (rb == null)
        {
            Debug.LogError("RB not found");
            return;
        }
        
        if (boxCollider == null)
        {
            Debug.LogError("Collider not found");
            return;
        }
        UpdateTargetPosition();
    }

    void FixedUpdate()
    {
        if (rb != null)
        {
            MoveEnemy();
        }
    }

    void MoveEnemy()
    {
        float currentX = transform.position.x;
        float direction = switchSides ? 1f : -1f;
        
        Vector2 newPosition = rb.position + new Vector2(direction * speed * Time.fixedDeltaTime, 0);
        rb.MovePosition(newPosition);
        
        if (switchSides && currentX >= rightBoundary)
        {
            Debug.Log($"Enemy reached right boundary at {transform.position}");
            switchSides = false;
        }
        else if (!switchSides && currentX <= leftBoundary)
        {
            Debug.Log($"Enemy reached left boundary at {transform.position}");
            switchSides = true;
        }
    }

    void UpdateTargetPosition()
    {
        currentTargetX = switchSides ? rightBoundary : leftBoundary;
    }
}
