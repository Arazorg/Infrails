using UnityEngine;

public class Trolley : MonoBehaviour
{
    public void Init(TrolleyData data)
    {
        GetComponent<Animator>().runtimeAnimatorController = data.AnimatorController;
        GetComponent<TrolleyMovement>().Init(data.Speed, data.SpeedDebuffByEnemyLaser);       
    }
}

