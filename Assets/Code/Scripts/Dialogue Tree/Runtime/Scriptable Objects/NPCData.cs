using UnityEngine;

namespace retrobarcelona.DialogueTree.Runtime
{
    [CreateAssetMenu(fileName = "NewNPC", menuName = "Tree/DialogueTree/NPC Data")]
    public class NPCData : ScriptableObject
    {
        public string _npcName;
        public Sprite _npcAvatar;
    }
}