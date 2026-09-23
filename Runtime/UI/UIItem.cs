using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityExtensions;

public class UIItem<TData> : MonoBehaviour, IUIListItem<TData>
{

    [Header("Animation")]
    [ColorLabel] public Ease AnimationEase = Ease.OutBack;
    [ColorLabel(1f, 0.65f, 0f)] public float AnimationDelayStep = 0.1f;
    [ColorLabel(1f, 0.65f, 0f)] public float AnimationDuration = 0.35f;
    public virtual void Bind(TData data, int index, bool play = true)
    {
        PlayAnimation(index, play);
    }

    public virtual void PlayAnimation(int index, bool play = true)
    {
        AnimationExtensions.PlayAppear(transform, index, AnimationEase, play ? AnimationDelayStep : 0, play ? AnimationDuration : 0);
    }
}
public interface IUIListItem<in TData>
{
    void Bind(TData data, int index, bool play = true);
    void PlayAnimation(int index, bool play = true);
}

public class UIReusableList<TData, TItem> where TItem : Component
{
    private readonly TItem prefab;
    private readonly Transform parent;

    private readonly List<TItem> items = new();

    public IReadOnlyList<TItem> Items => items;

    public UIReusableList(TItem prefab, Transform parent)
    {
        this.prefab = prefab;
        this.parent = parent;

        CacheExistingChildren();
    }

    public void CacheExistingChildren()
    {
        items.Clear();

        for (int i = 0; i < parent.childCount; i++)
        {
            if (parent.GetChild(i).TryGetComponent<TItem>(out var item))
            {
                if (!item.gameObject.activeSelf)
                    continue;

                GameObjectExtensions.ActiveObj(item.gameObject, false);
                items.Add(item);
            }
        }
    }

    public void Refresh(IList<TData> dataList, bool play = true)
    {
        EnsureCapacity(dataList.Count);

        for (int i = 0; i < items.Count; i++)
        {
            GameObjectExtensions.ActiveObj(items[i].gameObject, i < dataList.Count);
        }

        for (int i = 0; i < dataList.Count; i++)
        {
            var item = items[i];

            GameObjectExtensions.ActiveObj(item.gameObject, true);

            if (item is IUIListItem<TData> bindable)
            {
                bindable.Bind(dataList[i], i, play);
            }
        }
    }

    public void HideAll()
    {
        foreach (var item in items)
        {
            GameObjectExtensions.ActiveObj(item.gameObject, false);
        }
    }

    private void EnsureCapacity(int count)
    {
        while (items.Count < count)
        {
            var item = Object.Instantiate(prefab, parent);
            GameObjectExtensions.ActiveObj(item.gameObject, false);
            items.Add(item);
        }
    }
}

