using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(SpriteRenderer))]
public class ItemFader : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        // 渐变效果，修改SpriteRenderer下color的alpha值
        _spriteRenderer = GetComponent<SpriteRenderer>();
        
    }

    /// <summary>
    /// 逐渐恢复颜色
    /// </summary>
    public void FadeIn()
    {
        Color targetColor = new Color(1, 1, 1, 1);
        _spriteRenderer.DOColor(targetColor, Settings.FadeDuration);
    }

    /// <summary>
    /// 逐渐颜色半透明
    /// </summary>
    public void FadeOut()
    {
        Color targetColor = new Color(1, 1, 1, Settings.TargetAlpha);
        _spriteRenderer.DOColor(targetColor, Settings.FadeDuration);
    }
}