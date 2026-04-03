using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DoorFade : MonoBehaviour
{
    public float _fadeDuration = 1.5f;
    public bool _disableOnFadeComplete = true;
    public float _disableDelay = 0.1f;

    private SpriteRenderer _spriteRenderer;
    private bool _isFading = false;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        CloseAndDisappear();
    }

    public void CloseAndDisappear()
    {
        if (!_isFading)
        {
            StartCoroutine(FadeOutAndDisable());
        }
    }

    private IEnumerator FadeOutAndDisable()
    {
        _isFading = true;

        float elapsedTime = 0f;
        Color startColor = _spriteRenderer.color;

        while (elapsedTime < _fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / _fadeDuration);
            _spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }

        _spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, 0f);

        if (_disableOnFadeComplete)
        {
            yield return new WaitForSeconds(_disableDelay);
            gameObject.SetActive(false);
        }
    }
}
