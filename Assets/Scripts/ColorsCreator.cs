using UnityEngine;
using Random = UnityEngine.Random;

public class ColorsCreator : MonoBehaviour
{
    [SerializeField] private Material[] standartMaterials;

    private int _currentIdx;
    private Color _colorFrom;
    private Color _colorTo;

    void Awake()
    {
        _colorTo = GetRandomColor();
        ChangeColorsGamma();
    }

    public Material GetColor()
    {
        Material material = standartMaterials[_currentIdx];
        material.color = Color.Lerp(_colorFrom, _colorTo, (float)_currentIdx / standartMaterials.Length);
        
        SkipMaterialToNext();
        return material;
    }

    private void SkipMaterialToNext()
    {
        if (_currentIdx < standartMaterials.Length - 1)
        {
            _currentIdx++;
            return;
        }
        _currentIdx = 0;
        ChangeColorsGamma();
    }

    private Color GetRandomColor() => Color.HSVToRGB(Random.Range(0f, 1f), Random.Range(0.5f, 1f), Random.Range(0.65f, 1f));

    private void ChangeColorsGamma()
    {
        _colorFrom = _colorTo;
        _colorTo = GetRandomColor();
    }
}
