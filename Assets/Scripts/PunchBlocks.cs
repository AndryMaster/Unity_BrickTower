using UnityEngine;

public class PunchBlocks : MonoBehaviour
{
    [SerializeField] private float force;
    [SerializeField] private float radius;
    [SerializeField] private float downValue;
    private GameObject[] _blocks;
    public bool isFirst = true;

    private void GetBlocks() => _blocks = GameObject.FindGameObjectsWithTag("Block");

    public void DoSpaceExplosion(Vector3 center, float delay)
    {
        if (isFirst)
        {
            GetBlocks();
            isFirst = false;
            Vector3 position = new Vector3(1.1f, center.y - downValue, 1.1f); 
            // center + new Vector3(0.25f, -downValue, 0.25f);

            foreach (var block in _blocks)
            {
                Rigidbody rb = block.GetComponent<Rigidbody>();
                rb.isKinematic = false;
                rb.AddExplosionForce(force, position, radius);
            }
            
            Invoke(nameof(AddGravity), delay - 0.9f);
        }
    }

    public void AddGravity()
    {
        foreach (var block in _blocks)
        {
            Rigidbody rb = block.GetComponent<Rigidbody>();
            rb.drag = 0.1f;
            rb.angularDrag = 5f;
            rb.useGravity = true;
        }
    }
}
