using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class HealthBarHandler : MonoBehaviour
{
    [SerializeField] private Image _redBar;
    [SerializeField] private float _lerpSpeed;
    private EnemyController _eh;
    private PlayerController _ph;

    private void Awake()
    {
        _eh = GetComponentInParent<EnemyController>();
        _ph = GetComponentInParent<PlayerController>();
        if (_eh != null) _eh.OnHealthPrcChange += _handleChange;
        if (_ph != null) _ph.OnHealthPrcChange += _handleChange;
    }

    private void _handleChange(float pct)
    {
        StartCoroutine(_healthChange(pct));
    }

    private IEnumerator _healthChange(float pct)
    {
        float prePct = _redBar.fillAmount;
        float elapsed = 0;

        while (elapsed < _lerpSpeed)
        {
            elapsed += Time.deltaTime;
            _redBar.fillAmount = Mathf.Lerp(prePct, pct, elapsed / _lerpSpeed);
            yield return null;
        }
        _redBar.fillAmount = pct;
    }

    private void LateUpdate()
    {
        if (_eh != null) transform.LookAt(FindObjectOfType<Camera>().transform);
    }

}
