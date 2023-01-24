using UnityEngine;
using DG.Tweening;

public class BlockMove : MonoBehaviour
{
    public Move move;
    public float distAdd = 0.55f;
    public float distMul = 0.6f;
    public float speedMul;
    
    internal float lengthAxis;
    
    private Vector3 _point1;
    private Vector3 _point2;
    private float secondsMove;
    private Ease _movingType = Ease.InOutSine;  // Ease.Linear
    // https://camo.githubusercontent.com/d1bd2ce0cebbf0a007994e329c0a5485b0947d29b698d303ac7eb951c0ef7a81/68747470733a2f2f6b696b69746f2e6769746875622e696f2f747765656e2e6c75612f696d672f747765656e2d66616d696c6965732e706e67

    void Start()
    {
        Vector3 position = transform.position;
        Vector3 scale = transform.localScale;
        
        if (move is Move.ZMove)
        {
            _point1 = position + new Vector3(0f, 0f, scale.z * distMul + distAdd);
            _point2 = position + new Vector3(0f, 0f, scale.z * -distMul - distAdd);
            lengthAxis = scale.z;
        } 
        else if (move is Move.XMove)
        {
            _point1 = position + new Vector3(scale.x * distMul + distAdd, 0f, 0f);
            _point2 = position + new Vector3(scale.x * -distMul - distAdd, 0f, 0f);
            lengthAxis = scale.x;
        }
        
        secondsMove = (Vector3.Distance(_point1, _point2) / 2.75f + 0.35f) * (1f + speedMul / 100f);
        // Debug.Log(secondsMove);
        transform.position = _point2;
        DoMovingSequenceLoop();
    }

    void DoMovingSequenceLoop()
    {
        if (move != Move.StopMove)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(transform.DOMove(_point1, secondsMove).SetEase(_movingType));
            sequence.Append(transform.DOMove(_point2, secondsMove).SetEase(_movingType));
            sequence.AppendCallback(DoMovingSequenceLoop);
        }
    }
}
