using System;
using System.Collections;
using System.Collections.Generic;
using retrobarcelona.Utils.Singleton;
using UnityEngine;
using retrobarcelona.Managers;
using retrobarcelona.Systems.AudioSystem;
using retrobarcelona.Managers.ControlsManager;

namespace retrobarcelona.DialogueTree.Runtime
{
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

        private DialogueTree _dialogueTree;
        private DialogueNode _currentNode;
        private StartNode _lastBranchStart;
        private NPCData _npcData;

        private const string HTML_ALPHA = "<color=#00000000>";
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

            _interactTimeoutDelta = InteractTimeout;
            _moveTimeoutDelta = MoveTimeout;
        }

        private void Update()
        {
            if (GameManager.Instance.InDialogue())
            {
                Interact();
                Move();
            }
        }

        public void StartDialogue(DialogueTree dialogueTree)
        {
            ClearUI();
            
            _dialogueTree = dialogueTree;
            if (dialogueTree._blackboard._npcData != null)
                _npcData = dialogueTree._blackboard._npcData;

            UIManager.Instance.ActivateDialogue(_npcData._npcName,_npcData._npcAvatar);
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

            UIManager.Instance.ActivateDialogue(_npcData.name,_npcData._npcAvatar);
            GameEvents.current.SetDialogue(true);
            _currentTheme = AudioSystem.Instance.GetCurrentMusic();

            ChangeCurrentNode(dialogueTree.GetRootNode());
        }

        private void EndLines()
        {
            SetActive(0);
            AudioSystem.Instance.StillSpeaking = false;
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

            float yDir = ControlsManager.Instance.GetMovementDirection().y;
            if (yDir < 0)
            {
                _moveTimeoutDelta = MoveTimeout;
                SetActive(_currentAnswer - 1);
            }
            if (yDir > 0)
            {
                _moveTimeoutDelta = MoveTimeout;
                SetActive(_currentAnswer + 1);
            }
        }

        private void Interact()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
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
            UIManager.Instance.SetName(_npcData._npcName); 

            bool textFinished = IsFinished();

            if (textFinished)
            {
                NextLine();
            }
            else
            {
                UIManager.Instance.SetTextDialogue(_currentNode._text);
                InstantCreateButtons();
                EndLines();
            }
        }

        bool IsFinished()
        {
            string displayedText = UIManager.Instance.GetTextDialogue();
            string text = UIManager.Instance.GetTextDialogue();
            text = text.Split(HTML_ALPHA)[0];
            bool b = text == _currentNode._text;
            StartNode start = _currentNode as StartNode;
            if (start != null) 
                b = b && _buttonsFinished; 

            return b; 
        }

        void NextLine()
        {
            switch (_currentNode)
            {                
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

            AudioSystem.Instance.SpeakWordsOnLoop();
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

            AudioSystem.Instance.SpeakWordsOnLoop();
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
            int alphaIndex = 0;

            foreach (char c in originalText.ToCharArray())
            {
                //UIManager.Instance.AddLetterDialogue(c);
                alphaIndex++;
                UIManager.Instance.SetTextDialogue(originalText);
                displayedText = UIManager.Instance.GetTextDialogue().Insert(alphaIndex, HTML_ALPHA);
                UIManager.Instance.SetTextDialogue(displayedText);
                yield return new WaitForSeconds(MAX_TYPE_TIME/textSpeed);
            }

            EndLines();
        }

        IEnumerator StartDelay(DialogueTree dialogueTree, float time)
        {
            yield return new WaitForSeconds(time);
            StartDialogue(dialogueTree);
        }
    }
}
