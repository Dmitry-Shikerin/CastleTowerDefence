using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using Sources.Frameworks.Utils.Reflections;
using UnityEngine;

namespace Sources.Frameworks.MyGameCreator.Triggers.Implementation.Common
{
    public class TriggerBase : View
    {
        [OnValueChanged(nameof(SetTypeByName))]
        [ValueDropdown(nameof(GetFilteredNames))] 
        [SerializeField] private string _typeName;
        
        private Type _type;
        private Component _component;

        private Type Type => _type ??= GetTypeByName();
        
        public void SetType(Type type) =>
            _type = type;

        public T Get<T>()
            where T : Component
        {
            if (_component == null)
                throw new NullReferenceException();
            
            if (_component is not T concrete)
                throw new InvalidCastException();
            
            return concrete;
        }

        protected void GetComponent(Component other, Action action)
        {
            if (other.TryGetComponent(Type, out Component component))
            {
                _component = component;
                action?.Invoke();
                
                return;
            }

            Component nextComponent = other.GetComponentInChildren(Type);

            if (nextComponent != null)
            {
                _component = nextComponent;
                action?.Invoke();
            }
        }    
        
        protected void GetComponent(GameObject other, Action action)
        {
            if (other.TryGetComponent(Type, out Component component))
            {
                _component = component;
                Debug.Log(component.ToString());
                action?.Invoke();
                
                return;
            }

            Component nextComponent = other.GetComponentInChildren(Type);

            if (nextComponent != null)
            {
                _component = nextComponent;
                action?.Invoke();
            }
        }

        private void SetTypeByName() =>
            _type = GetTypeByName();

        private Type GetTypeByName() =>
            ReflectionUtils.GetFilteredTypeList<View>()
                .FirstOrDefault(type => type.Name == _typeName)
            ?? throw new NullReferenceException($"Type {nameof(_type)} not found");


        private List<string> GetFilteredNames()
        {
            return ReflectionUtils.GetFilteredTypeList<View>()
                .Select(type => type.Name)
                .ToList();
        }
    }
}