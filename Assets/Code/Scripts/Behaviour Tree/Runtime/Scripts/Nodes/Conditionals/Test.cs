namespace Project.BehaviourTree.Runtime
{
    public class Test : ConditionalNode
    {
        public bool test;
        protected override bool Question() => test;
    }
}