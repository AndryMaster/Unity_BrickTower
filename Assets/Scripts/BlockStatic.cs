using UnityEngine;

public class BlockStatic : MonoBehaviour
{
    void Awake()
    {
        Vector3 scl = transform.localScale;
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.mass = Mathf.Pow(scl.x * scl.y * scl.z, 0.5f);
    }
}
