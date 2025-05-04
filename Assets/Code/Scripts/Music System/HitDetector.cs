using System;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using retrobarcelona.Managers;
using retrobarcelona.Managers.ControlsManager;
using retrobarcelona.MusicSystem;
using retrobarcelona.Systems.AudioSystem;
using retrobarcelona.UI;
using UnityEngine;

namespace retrobarcelona.MusicSystem
{
    public class HitDetector : MonoBehaviour
    {
        [SerializeField] private GameObject[] hitEffects; 
        [SerializeField] private String[] _hitSounds; 
        [SerializeField] private float _perfectRange = 0.05f; 
        [SerializeField] private float _goodRange = 0.1f; 
        [SerializeField] private float _badRange = 0.5f; 
        [SerializeField] private Transform[] hitZones;   
        private bool _inDialogue = true;
        private void SetInDialogue(bool b) => WaitSetInDialogue(b);
        private async UniTask WaitSetInDialogue(bool value)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            _inDialogue = value;
        }

        private void Start()
        {
            GameEvents.current.onSetDialogue += SetInDialogue;
        }

        private void Oestroy()
        {
            GameEvents.current.onSetDialogue -= SetInDialogue;
        }

        void Update() 
        {
            DetectHits();
        }

        void DetectHits()
        {
            if (_inDialogue) 
            {
                int l = 0;
                if (ControlsManager.Instance.GetIsLane1())  { l = 1; }
                if (ControlsManager.Instance.GetIsLane2())  { l = 2; }
                if (ControlsManager.Instance.GetIsLane3())  { l = 3; }
                if (ControlsManager.Instance.GetIsLane4())  { l = 4; }

                if (l != 0) { Instantiate(hitEffects[l], hitZones[l-1].position, Quaternion.identity).transform.SetParent(hitZones[l].transform); }
                return;
            }
            
            if (ControlsManager.Instance.GetIsLane1())  
            { 
                ProcessHit(0); 
            }
            if (ControlsManager.Instance.GetIsLane2())  
            { 
                ProcessHit(1); 
            }
            if (ControlsManager.Instance.GetIsLane3())  
            { 
                ProcessHit(2); 
            }
            if (ControlsManager.Instance.GetIsLane4())  
            { 
                ProcessHit(3); 
            }
        }

        void ProcessHit(int lane)
        {
            bool hitRegistered = false;
            Collider2D[] hits = Physics2D.OverlapCircleAll(hitZones[lane].position, _badRange);
            
            foreach (Collider2D hit in hits)
            {
                Note note = hit.GetComponent<Note>();
                if (note == null)
                    continue;

                NoteHitTiming timing;

                float distance = Mathf.Abs(hit.transform.position.x - hitZones[lane].position.x);

                if (distance <= _perfectRange)
                    timing = NoteHitTiming.Correct;
                else if (distance <= _goodRange)
                    timing = NoteHitTiming.AlmostCorrect;
                else
                    timing = NoteHitTiming.AlmostIncorrect;

                if (timing == NoteHitTiming.Correct)
                {
                    Transform parent = hit.transform.parent;
                    GameObject go = Instantiate(hitEffects[lane+1], hitZones[lane].position, Quaternion.identity).gameObject;
                    go.transform.SetParent(parent);
                    go.transform.position = hit.transform.position;
                }
                else
                {
                    Transform parent = hit.transform.parent;
                    GameObject go = Instantiate(hitEffects[0], hitZones[lane].position, Quaternion.identity).gameObject;
                    go.transform.SetParent(parent);
                    go.transform.position = hit.transform.position;
                }  

                Destroy(hit.gameObject);
                
                AudioSystem.Instance.PlaySFX(_hitSounds[lane], Vector3.zero);
                SistemaDePuntos.Instance.CalcularPuntos(timing);
                hitRegistered = true;
                break; 
            }

            if (!hitRegistered)
            {
                NoteHitTiming timing = NoteHitTiming.Incorrect;
                SistemaDePuntos.Instance.CalcularPuntos(timing);
                AudioSystem.Instance.PlaySFX("BrokenGuitarSound", Vector3.zero);
            }
        }

        private void OnDrawGizmos()
        {
            foreach (Transform hitZone in hitZones)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(hitZone.position, _badRange);
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(hitZone.position, _goodRange);
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(hitZone.position, _perfectRange);
            }
        }
    }
}