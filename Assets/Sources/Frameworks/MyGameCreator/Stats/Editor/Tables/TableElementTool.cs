﻿using System;
using Sources.Frameworks.MyGameCreator.Core.Editor.Common;
using UnityEditor;
using UnityEditor.UIElements;

namespace Sources.Frameworks.MyGameCreator.Stats.Editor.Tables
{
    public class TableElementTool : PropertyElement
    {
        // EVENTS: --------------------------------------------------------------------------------

        public event Action<SerializedProperty> EventChangeValue;
        
        // CONSTRUCTOR: ---------------------------------------------------------------------------
        
        public TableElementTool(SerializedProperty property) 
            : base(property, property.displayName, false)
        {
            
        }
        
        // OVERRIDE METHODS: ----------------------------------------------------------------------

        protected override void CreateBody()
        {
            PropertyField field = new PropertyField(this.m_Property);
            field.RegisterValueChangeCallback(changeEvent =>
            {
                this.EventChangeValue?.Invoke(changeEvent.changedProperty);
            });
            
            this.m_Body.Add(field);
            this.m_Body.Bind(this.m_Property.serializedObject);
        }

    }
}