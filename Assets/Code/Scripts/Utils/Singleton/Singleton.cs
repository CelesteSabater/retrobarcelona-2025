using UnityEngine;

namespace retrobarcelona.Utils.Singleton
{
    public abstract class Singleton<T> : StaticInstance<T> where T : MonoBehaviour
    {
        protected override void Awake()
        {
            if (Instance != null) 
                Destroy(gameObject);
            else
                base.Awake();
        }
    }
}