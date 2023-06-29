using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "Game/Cursor Data", fileName = "New Cursor")]
    public class CursorData : ScriptableObject
    {
        public Texture2D cursor;
        public Vector2 hotSpot;
    }
}