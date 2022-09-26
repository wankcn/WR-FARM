using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 单独挂载在Player身上，做碰撞相关
/// </summary>
public class TriggerItemFader : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        ItemFader[] itemFaders = other.GetComponentsInChildren<ItemFader>();
        if (itemFaders.Length > 0)
        {
            foreach (var n in itemFaders)
                n.FadeOut();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        ItemFader[] itemFaders = other.GetComponentsInChildren<ItemFader>();
        if (itemFaders.Length > 0)
        {
            foreach (var n in itemFaders)
                n.FadeIn();
        }
    }
}