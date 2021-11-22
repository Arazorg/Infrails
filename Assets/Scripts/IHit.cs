using UnityEngine;

public interface IHit
{
    abstract void Accept(Transform target);

    abstract void Accept(Transform target, IDebuffVisitor hitableVisitor);
}
