using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Type = System.Type;
using System.Reflection;
using System.Linq;
using MJGame;


public abstract class AttributeAction<TAttr> : AttributeActionBase where TAttr : Attribute
{
    private TAttr m_Attribute;
    protected TAttr attribute => m_Attribute;


    protected override void OnSceneGUI(SerializedProperty property, Attribute attribute)
    {
        m_Attribute = (TAttr)attribute;
        OnSceneGUI(property);
    }

    protected abstract void OnSceneGUI(SerializedProperty property);
}