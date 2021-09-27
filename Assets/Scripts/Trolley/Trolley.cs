using UnityEngine;

public class Trolley : MonoBehaviour
{
    private TrolleyData _data;
    private Animator _animator;
    private TrolleyMovement _trolleyMovement;

    public void Init(TrolleyData data)
    {
        _data = data;
        _animator = GetComponent<Animator>();
        _trolleyMovement = GetComponent<TrolleyMovement>();
        _animator.runtimeAnimatorController = _data.AnimatorController;
        _trolleyMovement.Speed = _data.Speed;
    }
}

