using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(ColorsCreator))]
public class MainSpawner : MonoBehaviour
{
    [SerializeField] private GameObject BlockPrefab;
    [SerializeField] private GameObject BlockMovingPrefab;
    [SerializeField] private GameObject BlockForFallPrefab;
    [SerializeField] private GameObject PlanePerfect;
    
    [SerializeField] private Transform CameraCenter;
    [SerializeField] private AudioRecorder Recorder;
    [SerializeField] private PunchBlocks PunchBlocks;
    [SerializeField] private SaveSerial Saver;

    public float height;
    public float minPerfectDist = 0.1f;
    public float endDelay = 4f;
    
    private int _comboClick;
    private int _heightClick;
    private Vector3 _currentCenter;
    private Vector3 _lastBlockScale;
    private Move _currentMove = Move.ZMove;
    private Material _planeMaterial;
    private Material _currentMaterial;
    private GameObject _currentBlock;
    private ColorsCreator _colorsCreator;
    private bool _isRunning = true;
    
    void Start()
    {
        _colorsCreator = GetComponent<ColorsCreator>();

        _planeMaterial = PlanePerfect.GetComponentInChildren<MeshRenderer>().sharedMaterial;
        
        _lastBlockScale = GameObject.Find("PlatformBlock").transform.localScale;
        _currentCenter = new Vector3(_lastBlockScale.x/2, -height/2, _lastBlockScale.z/2);
    }
    
    public void DoStepClick()
    {
        if (_currentBlock)
        {
            SplitBlock();
            Saver.GetParamsOnClick(_heightClick, _comboClick);
        }
        else if (!_isRunning && PunchBlocks.isFirst)
        {
            Saver.RestartGame(endDelay);
            PunchBlocks.DoSpaceExplosion(_currentCenter, endDelay);
        }
        
        if (_isRunning) CreateMovingBlock();
    }

    private void CreateMovingBlock()
    {
        _currentMaterial = _colorsCreator.GetColor();

        _currentCenter.y += height;
        BlockMovingPrefab.GetComponent<BlockMove>().speedMul = _heightClick;
        _currentBlock = Instantiate(BlockMovingPrefab, _currentCenter, Quaternion.identity);
        _currentBlock.transform.localScale = new Vector3(_lastBlockScale.x, height, _lastBlockScale.z);
        _currentBlock.GetComponent<MeshRenderer>().material = _currentMaterial;
        _currentBlock.GetComponent<BlockMove>().move = _currentMove;

        CameraCenter.transform.DOMove(CameraCenter.transform.position + new Vector3(0f, height, 0f), 0.45f).SetEase(Ease.InOutQuad);
        transform.position += new Vector3(0f, height, 0f);

        switch (_currentMove)
        {
            case (Move.ZMove):
                _currentMove = Move.XMove; break;
            case (Move.XMove):
                _currentMove = Move.ZMove; break;
        }
    }

    private void SplitBlock()
    {
        _heightClick += 1;

        bool perfectClick = false;
        bool badClick = false;
        float divClick = Vector3.Distance(_currentBlock.transform.position, _currentCenter);
        float lengthAxis = _currentBlock.GetComponent<BlockMove>().lengthAxis;
        float corrMinPerfectDist = minPerfectDist * 0.65f + lengthAxis / 20;

        if (divClick < corrMinPerfectDist) perfectClick = true;
        else if (divClick >= lengthAxis) badClick = true;
        
        Vector3 divCenter = (_currentBlock.transform.position - _currentCenter);
        Vector3 currentScale = _currentBlock.transform.localScale;

        // Creating static block and plane
        GameObject block = null;
        if (!badClick)
        {
            block = CreateStaticBlock(perfectClick, divCenter, currentScale);
            block.GetComponent<MeshRenderer>().material = _currentMaterial;
        }
        if (perfectClick) CreatePlanePerfect(0.45f, currentScale);

        // Creating fall block
        if (!perfectClick)
        {
            GameObject fallBlock = CreateFallBlock(badClick, divCenter, currentScale);
            fallBlock.GetComponent<MeshRenderer>().material = _currentMaterial;
            Rigidbody fallRigidbody = fallBlock.GetComponent<Rigidbody>();
            fallRigidbody.AddForce(divCenter * 125f, ForceMode.Acceleration);
            
            if (block != null) 
                Physics.IgnoreCollision(fallBlock.GetComponent<Collider>(), block.GetComponent<Collider>());
        }
        
        // Combo
        if (perfectClick) _comboClick += 1;
        else _comboClick = 0;
        
        // Audio
        if (perfectClick) Recorder.PlayPerfect(_comboClick);
        else if (badClick) Recorder.PlayLong();
        else Recorder.PlayCutAndVibrate();
        
        // Finally
        _lastBlockScale = BlockPrefab.transform.localScale;
        if (badClick)
        {
            _isRunning = false;
            Saver.GetParamsOnEnd(_heightClick);
        }
        if (!perfectClick) _currentCenter = _currentBlock.transform.position - (divCenter / 2);

        Destroy(_currentBlock);
    }

    private GameObject CreateStaticBlock(bool isPerfectClick, Vector3 divCenter, Vector3 currentScale)
    {
        if (isPerfectClick) 
        {
            BlockPrefab.transform.localScale = currentScale;
            _currentBlock.transform.position = _currentCenter;
            Debug.Log("Perfect");
            return Instantiate(BlockPrefab, _currentCenter, Quaternion.identity);
        }
        BlockPrefab.transform.localScale = (currentScale - divCenter.Abs());
        Vector3 blockPosition = _currentBlock.transform.position - (divCenter / 2);
        return Instantiate(BlockPrefab, blockPosition, Quaternion.identity);
    }

    private GameObject CreateFallBlock(bool badClick, Vector3 divCenter, Vector3 currentScale)
    {
        Vector3 newCenter = _currentBlock.transform.position;
        Vector3 fallPos = _currentCenter + (divCenter / 2);
        Vector3 fallScale = currentScale;
        
        if (_currentMove == Move.XMove && !badClick) fallScale.z = divCenter.z;
        else if (_currentMove == Move.ZMove && !badClick) fallScale.x = divCenter.x;

        if (badClick) fallPos = newCenter;
        else if (_currentMove == Move.XMove)
        {
            if (_currentCenter.z < newCenter.z) fallPos.z += currentScale.z / 2f;
            else fallPos.z -= currentScale.z / 2f;
        }
        else if (_currentMove == Move.ZMove)
        {
            if (_currentCenter.x < newCenter.x) fallPos.x += currentScale.x / 2f;
            else fallPos.x -= currentScale.x / 2f;
        }
        
        BlockForFallPrefab.transform.localScale = fallScale.Abs();
        return Instantiate(BlockForFallPrefab, fallPos, Quaternion.identity);
    }

    private void CreatePlanePerfect(float duration, Vector3 scale)
    {
        PlanePerfect.transform.localScale = scale * 1.17f;
        Vector3 position = _currentCenter - new Vector3(0, height / 2, 0);
        Color32 color = _planeMaterial.color;
        color.a = 0;
        
        GameObject plane = Instantiate(PlanePerfect, position, Quaternion.identity);
        Destroy(plane, duration);

        Sequence sequence = DOTween.Sequence();
        sequence.Append(plane.transform.DOScale(scale * 1.1f + new Vector3(0.25f, 0.25f, 0.25f), duration));
        sequence.Insert(0f, _planeMaterial.DOColor(color, duration));
        sequence.AppendCallback(() => {
            color.a = 255;
            _planeMaterial.DOColor(color, 0.01f);
        });
    }
}
