using UnityEngine;

namespace Project.BehaviourTree.Runtime
{
    public class BehaviourTreeRunner : MonoBehaviour
    {
        public BehaviourTree _tree;

        public BehaviourTree GetTree() => _tree;

        protected virtual void Start()
        {
            if (_tree != null)
            {
                _tree.RestartTree();
                _tree = _tree.Clone();
                
                _tree.Bind(gameObject);
            }
        }

        protected virtual  void Update()
        {
            if (_tree != null) _tree.Update();
        }
    }
}
