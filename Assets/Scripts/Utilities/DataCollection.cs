using System;
using UnityEngine;

/**
 * 所有的自定义类，结构体
 */
[Serializable]
public class ItemDetails
{
    /// <summary>
    /// 物品ID
    /// </summary>
    public int itemID;

    public string itemName;

    public Sprite itemIcon;

    /// <summary>
    /// 物品类型，枚举 种子，可交易，或者家具等
    /// </summary>
    public ItemType itemType;

    /// <summary>
    /// 在世界地图上产生时显示的图片，区别于icon没有描边
    /// </summary>
    public Sprite itemOnWorldSprite;

    /// <summary>
    /// 详情
    /// </summary>
    public string itemDescription;

    /// <summary>
    /// 上下左右与斜方向可以在多少网格范围内使用
    /// </summary>
    public int itemUseRadius;

    /// <summary>
    /// 是否可以被拾取
    /// </summary>
    public bool canPickedup;

    /// <summary>
    /// 是否可丢弃
    /// </summary>
    public bool canDropped;

    /// <summary>
    /// 是否可举起
    /// </summary>
    public bool canCarried;

    /// <summary>
    /// 物品价格
    /// </summary>
    public int itemPrice;

    /// <summary>
    /// 表示物品可售卖时折扣的百分比
    /// </summary>
    [Range(0, 1)] public float sellPercentage;
}