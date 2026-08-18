using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

public static class CreateStateMenu
{
    [MenuItem("Assets/Create/Player/New State", priority = 3)]
    public static void CreateNewState()
    {
        string templatePath = "Assets/Scripts/Player/Movement/StateMachine/States/Template/TemplateState.cs";
        ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "NewState.cs");
    }
}