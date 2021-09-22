using UnityEngine;

public class Trolley : MonoBehaviour
{
    private TrolleyData _data;
    private Animator animator;
    private TrolleyMovement _trolleyMovement;

    public void Init(TrolleyData data)
    {
        _data = data;
        animator = GetComponent<Animator>();
        _trolleyMovement = GetComponent<TrolleyMovement>();
        animator.runtimeAnimatorController = _data.AnimatorController;
        _trolleyMovement.Speed = _data.Speed;
    }
}

