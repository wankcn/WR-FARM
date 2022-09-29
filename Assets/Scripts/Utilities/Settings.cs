using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings
{
    // Player移动到遮挡物后的半透明效果
    public const float FadeDuration = 0.35f;
    public const float TargetAlpha = 0.45f;

    // 编辑器使用
    public static string DefaultIconInItemEditor = "Assets/FarmRes/Art/Items/Icons/icon_Game.png";
    public static string ItemEditorPath = "Assets/Editor/UIBuilder/ItemEditor.uxml";
    public static string ItemRowTemplatePath = "Assets/Editor/UIBuilder/ItemRowTemplate.uxml";
}