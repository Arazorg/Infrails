using UnityEngine;

public class DestroyOnAnimationEnd : MonoBehaviour
{
    public void DestroyParent()
    {
        var parent = transform.parent.gameObject;
        Destroy(parent);
    }
}
