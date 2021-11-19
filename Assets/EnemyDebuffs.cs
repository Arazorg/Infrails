using System.Collections;
using UnityEngine;

public class EnemyDebuffs : MonoBehaviour
{
    [SerializeField] private GameObject _dizzinesEffect;

    private Enemy _enemy;
    private Coroutine _stunningCoroutine;
    private Coroutine _bleedingCoroutine;
    private float _stunningFinishTime;
    private float _bleedingFinishTime;

    public void StartStunning()
    {
        float stunningDuration = 1.5f;
        if (_stunningCoroutine == null)
        {
            _dizzinesEffect.SetActive(true);
            _stunningFinishTime = Time.time + stunningDuration;
            _stunningCoroutine = StartCoroutine(Stunning());
        }
        else
        {
            _stunningFinishTime = Time.time + stunningDuration;
        }
    }

    public void StartBleeding()
    {
        float bleedingDuration = 3f;
        if (_bleedingCoroutine == null)
        {
            _bleedingFinishTime = Time.time + bleedingDuration;
            _bleedingCoroutine = StartCoroutine(Bleeding());
        }
        else
        {
            _bleedingFinishTime = Time.time + bleedingDuration;
        }
    }

    private void Start()
    {
        _enemy = GetComponent<Enemy>();
        _dizzinesEffect.transform.localPosition = (_enemy.Data as AttackingEnemyData).DizzinesOffset;
    }

    private IEnumerator Stunning()
    {
        while (Time.time < _stunningFinishTime)
            yield return null;

        _dizzinesEffect.SetActive(false);
        _stunningCoroutine = null;
    }

    private IEnumerator Bleeding()
    {
        while (Time.time < _bleedingFinishTime)
        {
            int bleedingDamage = 1;
            float timeBetweenDamage = 0.75f;
            yield return new WaitForSeconds(timeBetweenDamage);
            _enemy.GetDamage(bleedingDamage);
        }

        _bleedingCoroutine = null;
    }
}
