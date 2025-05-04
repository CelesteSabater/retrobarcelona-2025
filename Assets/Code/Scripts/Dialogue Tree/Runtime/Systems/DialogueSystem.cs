using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using retrobarcelona.Utils.Singleton;
using retrobarcelona.Managers;
using retrobarcelona.UI;
using retrobarcelona.Systems.AudioSystem;
using retrobarcelona.Managers.ControlsManager;
using DialogueTree.Runtime;

namespace retrobarcelona.DialogueTree.Runtime
{
    [Serializable]
    public class StyleFormat
    {
        public String _styleKey;
        public String _style;
    }

    public class DialogueSystem : Singleton<DialogueSystem>
    {
        [Header("General")]
        private string _currentTheme = "";
        private string _lastNPCTheme = "";
        private int _currentAnswer;
        private bool _buttonsFinished;

        [SerializeField] private float InteractTimeout;
        [SerializeField] private float MoveTimeout;
        [SerializeField] private float textSpeed;
        public float _interactTimeoutDelta;
        public float _moveTimeoutDelta;
        
        private bool _inDialogue = true;
        private void SetInDialogue(bool value) => _inDialogue = value;

        private DialogueTree _dialogueTree;
        private DialogueNode _currentNode;
        private StartNode _lastBranchStart;
        private NPCData _npcData;

        private bool _songFinished = false;

        [SerializeField] private StyleFormat[] _styleFormats;
        private const string HTML_ALPHA = "<alpha=#00>";
        private const float MAX_TYPE_TIME = 0.05f;

        public event Action<DialogueTree,NPCData> onSetDialogueNPC;
        public void SetDialogueNPC(DialogueTree tree, NPCData npc)
        {
            if(onSetDialogueNPC != null)
                onSetDialogueNPC(tree, npc);
        }

        private void Start()
        {
            ClearUI();
            GameEvents.current.onSetDialogue += SetInDialogue;
            GameEvents.current.onSongFinished += SongFinished;

            _interactTimeoutDelta = InteractTimeout;
            _moveTimeoutDelta = MoveTimeout;
        }

        private void OnDestroy()
        {
            GameEvents.current.onSetDialogue -= SetInDialogue;
            GameEvents.current.onSongFinished -= SongFinished;
        }

        void SongFinished()
        {
            _songFinished = true;
            GameEvents.current.SetDialogue(true);
            NextLine();
        }

        private void Update()
        {
            if (_inDialogue)
            {
                Interact();
                //Move();
            }
        }

        public void StartDialogue(DialogueTree dialogueTree)
        {
            ClearUI();
            
            _dialogueTree = dialogueTree;
            if (dialogueTree._blackboard._npcData != null)
                _npcData = dialogueTree._blackboard._npcData;

            UIManager.Instance.ActivateDialogue();
            GameEvents.current.SetDialogue(true);
            _currentTheme = AudioSystem.Instance.GetCurrentMusic();

            ChangeCurrentNode(dialogueTree.GetRootNode());
        }

        public void StartDialogue(DialogueTree dialogueTree, float time)
        {
            StartCoroutine(StartDelay(dialogueTree, time));
        }

        public void StartDialogue(DialogueTree dialogueTree, NPCData npcData)
        {
            ClearUI();
            
            _dialogueTree = dialogueTree;
            _npcData = npcData;

            UIManager.Instance.ActivateDialogue();
            GameEvents.current.SetDialogue(true);
            _currentTheme = AudioSystem.Instance.GetCurrentMusic();

            ChangeCurrentNode(dialogueTree.GetRootNode());
        }

        private void EndLines()
        {
            _currentNode.EndDialogue();
        }

        private void SetActive(int _i)
        {
            StartNode node =_currentNode as StartNode;
            if (node == null)
                return;

            if (_i >= node._choices.Count)
                _i = 0;

            if (_i < 0)
                _i = node._choices.Count - 1;

            UIManager.Instance.SetActive(_i);
            _currentAnswer = _i;
        }

        private void Move()
        {
            if (_moveTimeoutDelta >= 0.0f)
            {
                _moveTimeoutDelta -= Time.deltaTime;
                return;
            }

            float direction = ControlsManager.Instance.GetMovementDirection().y;

            if (direction < 0)
            {
                _moveTimeoutDelta = MoveTimeout;
                SetActive(_currentAnswer - 1);
            }
            if (direction > 0)
            {
                _moveTimeoutDelta = MoveTimeout;
                SetActive(_currentAnswer + 1);
            }
        }

        private void Interact()
        {
            StartBranch node = _currentNode as StartBranch;

            if (ControlsManager.Instance.GetIsLane4() && _interactTimeoutDelta <= 0)
            {
                if (node != null) 
                    if (node._choices.Count < 1)
                        return;

                _currentAnswer = 0;
                _interactTimeoutDelta = InteractTimeout;
                InteractKey();
            } else if (ControlsManager.Instance.GetIsLane3() && _interactTimeoutDelta <= 0)
            {
                if (node != null) 
                    if (node._choices.Count < 2)
                        return;

                _currentAnswer = 1;
                _interactTimeoutDelta = InteractTimeout;
                InteractKey();
            } else if (ControlsManager.Instance.GetIsLane2() && _interactTimeoutDelta <= 0)
            {
                if (node != null) 
                    if (node._choices.Count < 3)
                        return;

                _currentAnswer = 2;
                _interactTimeoutDelta = InteractTimeout;
                InteractKey();
            } else if (ControlsManager.Instance.GetIsLane1() && _interactTimeoutDelta <= 0)
            {
                if (node != null) 
                    if (node._choices.Count < 4)
                        return;

                _currentAnswer = 3;
                _interactTimeoutDelta = InteractTimeout;
                InteractKey();
            }

            if (_interactTimeoutDelta >= 0.0f)
            {
                _interactTimeoutDelta -= Time.deltaTime;
            }
        }

        private void InteractKey()
        {
            StopAllCoroutines();

            bool textFinished = IsFinished();

            if (textFinished)
            {
                NextLine();
            }
            else
            {
                string originalText = _currentNode._text;
                foreach (var style in _styleFormats)
                    originalText = originalText.Replace(style._styleKey, style._style);

                UIManager.Instance.SetTextDialogue(originalText);
                InstantCreateButtons();
                EndLines();
            }
        }

        bool IsFinished()
        {
            string displayedText = UIManager.Instance.GetTextDialogue();
            string text = UIManager.Instance.GetTextDialogue();
            text = text.Split(HTML_ALPHA)[0];
            string originalText = _currentNode._text;

            foreach (var style in _styleFormats)
                originalText = originalText.Replace(style._styleKey, style._style);

            bool b = text == originalText;
            StartNode start = _currentNode as StartNode;
            if (start != null) 
                b = b && _buttonsFinished; 

            return b; 
        }

        void NextLine()
        {
            switch (_currentNode)
            {         
                case ChangeScene _node:
                    break;
                case Song _node:
                    if (_songFinished)
                        ChangeCurrentNode(_node._child);
                    break;       
                case EndNode _node:
                    EndDialogue(_node);
                    break;
                case StartNode _node:
                    ChangeCurrentWithoutTyping(_node._choices[_currentAnswer]);
                    NextLine();
                    break;
                case RootNode _node:
                    ChangeCurrentNode(_node._child);
                    break;
                case TextNode _node:
                    ChangeCurrentNode(_node._child);
                    break;
                case ActionNode _node:
                    ChangeCurrentNode(_node._child);
                    break;
            }     
        }

        void EndDialogue(EndNode node)
        {
            switch (node)
            {                
                case EndDialogue _node:
                    ExitDialogue();
                    break;
                case EndBranch _node:
                    if (_lastBranchStart != null)
                        ChangeCurrentNode(_lastBranchStart);
                    else
                        ExitDialogue();
                    break;
            }    
        }

        public void ExitDialogue()
        {
            if (_currentTheme != "" && _lastNPCTheme != "")
                AudioSystem.Instance.PlayMusic(_currentTheme); 

            ClearUI();

            if (_dialogueTree._nextDialogueTree != null)
                SetDialogueNPC(_dialogueTree, _npcData);
            GameEvents.current.SetDialogue(false);
        }

        void ClearUI()
        {
            UIManager.Instance.DisableDialogue();
            _currentAnswer = 0;
            _currentTheme = "";
            UIManager.Instance.ClearButtons();
        }

        void ChangeCurrentNode(DialogueNode node)
        {
            _currentNode = node;  
            UIManager.Instance.SetTextDialogue(string.Empty);
            UIManager.Instance.ClearButtons();
             
            StartNode start = node as StartNode;
            if (start != null) 
                if (start._returnOnEndBranch)
                    _lastBranchStart = start;

            _currentNode.StartDialogue();

            if (_currentNode._npcTheme != "")
            {
                _lastNPCTheme = _currentNode._npcTheme;
                AudioSystem.Instance.PlayMusic(_lastNPCTheme);
            }
                
            CreateButtons();
            if (_currentNode._text != string.Empty)
                StartCoroutine(TypeLine());
            else
            {
                EndLines();
                NextLine();  
            }
                       
        }

        void ChangeCurrentWithoutTyping(DialogueNode node)
        {
            _currentNode = node;  
            UIManager.Instance.SetTextDialogue(string.Empty);
            UIManager.Instance.ClearButtons();
             
            StartNode start = node as StartNode;
            if (start != null) 
                if (start._returnOnEndBranch)
                    _lastBranchStart = start;

            _currentNode.StartDialogue();

            if (_currentNode._npcTheme != null)
                AudioSystem.Instance.PlayMusic(_currentNode._npcTheme);       
        }

        private void CreateButtons()
        {
            StartBranch node = _currentNode as StartBranch;
            if (node == null) 
                return;

            UIManager.Instance.ClearButtons();
            _buttonsFinished = false;
            for (var i = 0; i <= node._choices.Count - 1; i++)
            {
                GameObject go;
                go = UIManager.Instance.CreateButtons();
                if (node._choices[i]._text != string.Empty)
                    StartCoroutine(TypeLineButton(go, node._choices[i]));
            }
            _buttonsFinished = true;
        }

        void InstantCreateButtons()
        {
            StartBranch node = _currentNode as StartBranch;
            if (node == null) 
                return;

            UIManager.Instance.ClearButtons();
            for (var i = 0; i <= node._choices.Count - 1; i++)
            {
                GameObject go;
                go = UIManager.Instance.CreateButtons();
                go.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = node._choices[i]._text;
            }
            _buttonsFinished = true;
            EndLines();
        }

        public void TypeLine(string text)
        {
            if (text == null)
                return;
            
            if (text == "")
                return;

            StartCoroutine(TypeLine2(text));
        }

        IEnumerator TypeLineButton(GameObject go, DialogueNode node)
        {
            string originalText = node._text; 
            string displayedText = ""; 
            int alphaIndex = 0;

            foreach (char c in originalText.ToCharArray())
            {
                alphaIndex++;
                go.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = originalText;
                displayedText = go.GetComponentInChildren<TMPro.TextMeshProUGUI>().text.Insert(alphaIndex, HTML_ALPHA);
                go.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = displayedText;
                yield return new WaitForSeconds(MAX_TYPE_TIME/textSpeed);
            }

            EndLines();
        }

        IEnumerator TypeLine()
        {
            string originalText = _currentNode._text; 
            string displayedText = "";
            bool isTag = false;

            foreach (var style in _styleFormats)
            {
                originalText = originalText.Replace(style._styleKey, style._style);
            }

            for(int i = 0; i < originalText.Length; i++)
            {
                String firstHalf = originalText.Substring(0, i+1);
                String secondHalf = originalText.Substring(i+1);
                secondHalf = secondHalf.Replace(">", ">"+HTML_ALPHA);
                secondHalf = secondHalf.Replace("<sprite=0>", "<sprite=0 color=#FFFFFF00>");
                secondHalf = secondHalf.Replace("<sprite=1>", "<sprite=1 color=#FFFFFF00>");
                secondHalf = secondHalf.Replace("<sprite=2>", "<sprite=2 color=#FFFFFF00>");
                secondHalf = secondHalf.Replace("<sprite=3>", "<sprite=3 color=#FFFFFF00>");

                if (originalText[i] == '<')
                    isTag = true;
                else if (originalText[i] == '>')
                    isTag = false; 

                displayedText = firstHalf + HTML_ALPHA + secondHalf;

                if (!isTag)
                {
                    AudioSystem.Instance.SpeakWord();
                    UIManager.Instance.SetTextDialogue(displayedText);
                    yield return new WaitForSeconds(MAX_TYPE_TIME/textSpeed);
                }   
            }

            EndLines();
        }

        IEnumerator TypeLine2(string originalText)
        {
            string displayedText = "";
            bool isTag = false;

            foreach (var style in _styleFormats)
            {
                originalText = originalText.Replace(style._styleKey, style._style);
            }

            for(int i = 0; i < originalText.Length; i++)
            {
                String firstHalf = originalText.Substring(0, i+1);
                String secondHalf = originalText.Substring(i+1);
                secondHalf = secondHalf.Replace(">", ">"+HTML_ALPHA);
                secondHalf = secondHalf.Replace("<sprite=0>", "<sprite=0 color=#FFFFFF00>");
                secondHalf = secondHalf.Replace("<sprite=1>", "<sprite=1 color=#FFFFFF00>");
                secondHalf = secondHalf.Replace("<sprite=2>", "<sprite=2 color=#FFFFFF00>");
                secondHalf = secondHalf.Replace("<sprite=3>", "<sprite=3 color=#FFFFFF00>");

                if (originalText[i] == '<')
                    isTag = true;
                else if (originalText[i] == '>')
                    isTag = false; 

                displayedText = firstHalf + HTML_ALPHA + secondHalf;

                if (!isTag)
                {
                    AudioSystem.Instance.SpeakWord();
                    UIManager.Instance.SetTextDialogue(displayedText);
                    yield return new WaitForSeconds(MAX_TYPE_TIME/textSpeed);
                }   
            }
        }

        IEnumerator StartDelay(DialogueTree dialogueTree, float time)
        {
            yield return new WaitForSeconds(time);
            StartDialogue(dialogueTree);
        }
    }
}
