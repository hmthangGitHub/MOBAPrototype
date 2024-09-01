using UnityEngine;

namespace UIView
{
    public static class GameObjectExtenstion
    {
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : MonoBehaviour
        {
            if (!gameObject.TryGetComponent<T>(out var component))
            {
                component = gameObject.AddComponent<T>();
            }

            return component;
        }
    }
}