using UnityEngine;
using retrobarcelona.Utils.Singleton;

namespace retrobarcelona.UI
{
    public class UIManager : Singleton<UIManager>
    {
        [Header("Dialogue")]
        [SerializeField] private GameObject DialogueCanvas;
        [SerializeField] private GameObject DialogueBox;
        [SerializeField] private GameObject ButtonHolder;
        [SerializeField] private GameObject _choiceButtonPrefab;
        
        public void ActivateDialogue()
        {
            DialogueCanvas.SetActive(true);
        }

        public void DisableDialogue()
        {
            DialogueBox.transform.Find("Text").GetComponent<TMPro.TextMeshProUGUI>().text = "";
            DialogueCanvas.SetActive(false);
        }


        public void SetTextDialogue(string text)
        {
            DialogueBox.transform.Find("Text").GetComponent<TMPro.TextMeshProUGUI>().text = text;
        }

        public string GetTextDialogue()
        {
            return DialogueBox.transform.Find("Text").GetComponent<TMPro.TextMeshProUGUI>().text;
        }

        public void AddLetterDialogue(char c)
        {
            DialogueBox.transform.Find("Text").GetComponent<TMPro.TextMeshProUGUI>().text += c;
        }

        public GameObject CreateButtons()
        {
            GameObject go;
            go = Instantiate(_choiceButtonPrefab.transform.gameObject, ButtonHolder.transform.position, ButtonHolder.transform.rotation);
            go.transform.SetParent(ButtonHolder.transform);
            return go;
        }

        public void SetActive(int _i)
        {
            GameObject go;
            if (_i < 0) _i = ButtonHolder.transform.childCount - 1;
            if (_i > ButtonHolder.transform.childCount - 1) _i = 0;
            for (var i = ButtonHolder.transform.childCount - 1; i >= 0; i--)
            {
                go = ButtonHolder.transform.GetChild(i).gameObject;
                go.GetComponentInChildren<TMPro.TextMeshProUGUI>().color = Color.white;
            }
            if (_i < ButtonHolder.transform.childCount)
            {
                go = ButtonHolder.transform.GetChild(_i).gameObject;
                go.GetComponentInChildren<TMPro.TextMeshProUGUI>().color = Color.yellow;
            }
        }

        public void ClearButtons()
        {
            for (var i = ButtonHolder.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(ButtonHolder.transform.GetChild(i).gameObject);
            }
        }
    }
}