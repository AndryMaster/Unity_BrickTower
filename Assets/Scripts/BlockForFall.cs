using UnityEngine;

public class BlockForFall : MonoBehaviour
{
    private const float _massMul = 0.01f;
    
    void Awake()
    {
        Rigidbody _rigidBody = GetComponent<Rigidbody>();
        Vector3 pos = transform.position;
        _rigidBody.mass = pos.x * pos.y * pos.z * _massMul;
        // Destroy(gameObject, 15f);
    }

    private void Update()
    {
        if (transform.position.y < 0)
        {
            Destroy(gameObject, 3f);
        }
    }
}
