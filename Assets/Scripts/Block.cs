using UnityEngine;

namespace DefaultNamespace
{
    public enum BlockType { Air, Dirt, Marble, Gold }
    
    public class Block : MonoBehaviour
    {
        public BlockType blockType = BlockType.Dirt;
    }
}