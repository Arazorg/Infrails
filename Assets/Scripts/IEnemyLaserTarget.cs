using UnityEngine;

public interface IEnemyLaserTarget
{
    public Transform LaserAttackPoint { get; }

    public Transform Transform { get; }

    public bool IsVisible { get; }

    void StartLaserInteraction();

    void StopLaserInteraction();
}
