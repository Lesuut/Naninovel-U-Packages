using UnityEngine;

[CreateAssetMenu(fileName = "TemplateServiceGeneratorInfo", menuName = "ScriptableObjects/TemplateServiceGeneratorInfo")]
public class TemplateServiceGeneratorInfo : ScriptableObject
{
    [Header("Data")]
    public TextAsset BaseConfigucation;
    public TextAsset BaseConfigucationAPI;
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
    public TextAsset BaseUINoSystem;
    public TextAsset BaseUIDataNoSystem;
    [Header("Command")]
    public TextAsset BaseCommand;
    public TextAsset BaseCommandEmpty;
    [Header("Editor")]
    public TextAsset BaseSettings;
    public TextAsset SyntaxHighlighter;
}