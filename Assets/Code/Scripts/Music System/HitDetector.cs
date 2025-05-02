using retrobarcelona.Managers.ControlsManager;
using retrobarcelona.MusicSystem;
using UnityEngine;

public class HitDetector : MonoBehaviour
{
    [SerializeField] private GameObject[] hitEffects; 
    [SerializeField] private float _perfectRange = 0.05f; 
    [SerializeField] private float _goodRange = 0.1f; 
    [SerializeField] private float _badRange = 0.5f; 
    [SerializeField] private Transform[] hitZones;   

    void Update() 
    {
        DetectHits();
    }

    void DetectHits()
    {
        if (ControlsManager.Instance.GetIsLane1())  { ProcessHit(0); }
        if (ControlsManager.Instance.GetIsLane2())  { ProcessHit(1); }
        if (ControlsManager.Instance.GetIsLane3())  { ProcessHit(2); }
        if (ControlsManager.Instance.GetIsLane4())  { ProcessHit(3); }
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

            float distance = Mathf.Abs(hit.transform.position.x - hitZones[lane].position.x);
            GameObject effect = null;

            if (distance <= _perfectRange)
            {
                Debug.Log("Perfect!");
                effect = hitEffects[0];
            }
            else if (distance <= _goodRange)
            {
                Debug.Log("Good!");
                effect = hitEffects[1];
            }
            else
            {
                Debug.Log("Bad!");
                effect = hitEffects[2];
            }

            Destroy(hit.gameObject);
            Instantiate(effect, hitZones[lane].position, Quaternion.identity);
            //GameManager.Instance.AddScore(hitScore); // Asume que hay un GameManager
            hitRegistered = true;
            break; 
        }

        if (!hitRegistered)
        {
            Debug.Log("Miss!");
            //GameManager.Instance.MissNote();
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