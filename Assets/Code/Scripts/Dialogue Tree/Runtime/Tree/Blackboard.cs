using System;
using System.Collections.Generic;
using UnityEngine;

namespace retrobarcelona.DialogueTree.Runtime
{
    [System.Serializable]
    public class Blackboard 
    {
        [Header("NPC Settings")]
        public NPCData _npcData;
        public Animator _animator;
    }
}
