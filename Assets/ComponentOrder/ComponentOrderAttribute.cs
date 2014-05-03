using System;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

public class ComponentOrderAttribute : System.Attribute
{
    public uint order;
    public Type type = null;
    public OrderType orderType = OrderType.Number;
    
    public ComponentOrderAttribute(uint order)
    {
        this.order = order;
        orderType = OrderType.Number;
    }

    public ComponentOrderAttribute(Type type)
    {
        this.type = type;
        orderType = OrderType.Type;
    }

    public enum OrderType
    {
        Type,
        Number,
    }

}


#if UNITY_EDITOR

[InitializeOnLoad]
public class ComponentOrder
{
    static string lastInstanceIDs = "";
    static ComponentOrder()
    {
        EditorApplication.update += () =>
        {
            if (Selection.gameObjects.Length == 0)
                return;

            foreach (var components in Selection.gameObjects.Select(gameObject => gameObject.GetComponents<Component>()))
            {
                if (lastInstanceIDs == string.Join(",", components.Select(c => c.GetInstanceID().ToString()).ToArray()))
                    return;
                for (var i = 1; i < components.Length; i++)
                {
                    var component = components[i];
                    var attribute = GetComponentOrderAttribute(component.GetType());

                    if (attribute == null)
                        continue;

                    if (attribute.orderType == ComponentOrderAttribute.OrderType.Number)
                    {
                        if (attribute.order < i)
                        {
                            if (components[i - 1].GetType() == component.GetType())
                                continue;

                            var _attribute = GetComponentOrderAttribute(components[i - 1].GetType());

                            if (_attribute != null && _attribute.order <= attribute.order)
                                continue;

                            ComponentUtility.MoveComponentUp(component);
                        }
                        else if (attribute.order > i)
                        {
                            if (i != components.Length - 1)
                            {
                                var _attribute = GetComponentOrderAttribute(components[i + 1].GetType());

                                if (_attribute != null && _attribute.order >= attribute.order)
                                    continue;
                            }

                            ComponentUtility.MoveComponentDown(component);
                        }
                    }
                    else if (attribute.orderType == ComponentOrderAttribute.OrderType.Type)
                    {
                        Component comp = null;
                        int index = 0;
                        for (int j = 0; j < components.Length; j++)
                        {
                            if (components[j].GetType() == attribute.type)
                            {
                                index = j;
                                comp = components[j];
                            }
                        }

                        if (comp)
                        {
                            if (index < i && index + 1 != i)
                            {
                                var _attribute = GetComponentOrderAttribute(components[i - 1].GetType());

                                if (_attribute != null && _attribute.order <= attribute.order)
                                    continue;

                                ComponentUtility.MoveComponentUp(component);
                            }
                            else if (index > i)
                            {
                                if (i != components.Length - 1)
                                {
                                    var _attribute = GetComponentOrderAttribute(components[i + 1].GetType());

                                    if (_attribute != null && _attribute.order >= attribute.order)
                                        continue;
                                }

                                ComponentUtility.MoveComponentDown(component);
                            }
                        }
                    }
                }
                lastInstanceIDs = string.Join(",", components.Select(c => c.GetInstanceID().ToString()).ToArray());
            }
        };
    }

    static ComponentOrderAttribute GetComponentOrderAttribute(Type type)
    {
        return type.GetCustomAttributes(typeof(ComponentOrderAttribute), true).Cast<ComponentOrderAttribute>().FirstOrDefault();
    }

}

#endif