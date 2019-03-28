/**
 * 自定义ScrollRect测试
 */

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class CustomScrollRectTest : MonoBehaviour
{
    /// <summary>
    /// 测试自定义的Scroll
    /// </summary>
    public CustomScrollRect Scroller;
    /// <summary>
    /// 模拟背包数据
    /// </summary>
    class Item { public int id; public int num; }
    private List<Item> mItemList = new List<Item>();

    /// <summary>
    /// 模拟1000个背包数据
    /// </summary>
    private void Start()
    {
        for (var i = 0; i < 1000; i++)
        {
            mItemList.Add(new Item() { id = i, num = Random.Range(1, 1000) });
        }
        Scroller.Total = mItemList.Count;
        Scroller.UpdateItem = UpdateItem;
        Scroller.OnValueChanged(Vector2.zero);
    }
    
    /// <summary>
    /// 单项Item刷新数据
    /// </summary>
    /// <param name="item"></param>
    private void UpdateItem(CustomScrollRectIndex item)
    {
        var index = item.Index;
        var itemData = mItemList[index];
        var id = item.transform.Find("Id").GetComponent<Text>();
        id.text = itemData.id.ToString();
        var num = item.transform.Find("Num").GetComponent<Text>();
        num.text = itemData.num.ToString();
        var addBtn = item.transform.Find("Add").GetComponent<Button>();
        addBtn.onClick.RemoveAllListeners();
        addBtn.onClick.AddListener(() => { Scroller.AddItem(index); });
        var deleteBtn = item.transform.Find("Delete").GetComponent<Button>();
        deleteBtn.onClick.RemoveAllListeners();
        deleteBtn.onClick.AddListener(() => { Scroller.DeleteItem(index); });
    }
}
