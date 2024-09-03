using UnityEngine;

namespace MobaPrototype
{
    [CreateAssetMenu(menuName = "Assets/HotKeyConfiguration")]
    public class HotKeyConfiguration : ScriptableObject
    {
        [field : SerializeField] public KeyCode[] HotKeys { get; private set; }
    }
}