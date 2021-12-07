using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class Controller : MonoBehaviour
{
    public RectTransform ItemsRoot = default;
    public Item ItemPrefab = default;
    public InputField ItemNameInput = default;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnElementDropped(ReorderableList.ReorderableListEventStruct ev)
    {
        Debug.Log($"Dropped! - {ev.ToIndex}");
        var newIndex = ev.ToIndex;
        var prevIndex = newIndex - 1;
        var nextIndex = newIndex + 1;

        var prevRank = prevIndex < 0 ? string.Empty : GetItem(prevIndex).Rank;
        var nextRank = nextIndex >= ItemsRoot.childCount ? string.Empty : GetItem(nextIndex).Rank;

        ev.DroppedObject.GetComponent<Item>().Rank = GetRank(prevRank, nextRank);
    }

    public string GetRank(string prevRank, string nextRank)
    {
        const char RANK_MIN = 'a';
        const char RANK_MAX = 'z';

        char? GetSubRank(string rank, int index)
            => rank.Length > index ? rank[index] : null as char?;
        
        var made = string.Empty;

        while (true)
        {
            var subRankPrev = GetSubRank(prevRank, made.Length) ?? RANK_MIN - 1;
            var subRankNext = GetSubRank(nextRank, made.Length) ?? RANK_MAX + 1;

            if ((subRankPrev + subRankNext) / 2 != subRankPrev)
            {
                // 앞놈과 뒷놈 사이에 공간이 충분하다. 자리 차지하고 리턴하자.
                made += (char)((subRankPrev + subRankNext) / 2);
                return made;
            }
            
            // 앞놈과 뒷놈 사이에 공간이 부족하다. 다음 자리로 넘어가자.
            made += (char)subRankPrev;
        }
    }

    public string GetRank(int newIndex)
    {
        var prevIndex = newIndex - 1;
        var nextIndex = newIndex + 1;

        var prevRank = prevIndex < 0 ? string.Empty : GetItem(prevIndex).Rank;
        var nextRank = nextIndex >= ItemsRoot.childCount ? string.Empty : GetItem(nextIndex).Rank;

        return GetRank(prevRank, nextRank);
    }

    public Item GetItem(int index)
    {
        return ItemsRoot.GetChild(index).GetComponent<Item>();
    }
    
    public void AppendItem()
    {
        if (string.IsNullOrEmpty(ItemNameInput.text))
            return;
        var newItem = Instantiate(ItemPrefab, ItemsRoot);
        newItem.Name = ItemNameInput.text;

        newItem.Rank = GetRank(ItemsRoot.childCount - 1);
    }
}
