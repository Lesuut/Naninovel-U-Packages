using UnityEngine;

[CreateAssetMenu(fileName = "TemplateServiceGeneratorInfo", menuName = "ScriptableObjects/TemplateServiceGeneratorInfo")]
public class TemplateServiceGeneratorInfo : ScriptableObject
{
    [Header("Data")]
    public TextAsset BaseConfigucation;
    public TextAsset BaseState;
    [Header("Service")]
    public TextAsset BaseService;
    public TextAsset BaseServiceConfigucation;
    [Header("Interface")]
    public TextAsset IBaseService;
    public TextAsset IBaseServiceConfigucation;
    [Header("UI")]
    public TextAsset BaseUI;
    public TextAsset BaseUIData;
}
