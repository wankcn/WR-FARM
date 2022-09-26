using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SpriteRenderer))]
public class ItemFader : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        // 渐变效果，修改SpriteRenderer下color的alpha值
        _spriteRenderer = GetComponent<SpriteRenderer>();
        
        // 1.可以
    }
}