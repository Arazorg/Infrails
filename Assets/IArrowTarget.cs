using UnityEngine;

public interface IArrowTarget
{
    public Transform ArrowSpawnPoint { get; }

    public void ArrowHitEffect();
}
