using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueSO", menuName = "ScriptableObjects/DialogueSO", order = 1)]
public class DialogueSO : ActionSO
{
    [SerializeField] private List<Dialogue> dialogues= new List<Dialogue>();
    public List<Dialogue> Dialogues => dialogues;

    [Serializable]    
    public struct Dialogue
    {
        [SerializeField] private string m_DialogueText;
        [SerializeField] private AudioClip m_AudioClip;
        public string DialogueText => m_DialogueText;
        public AudioClip AudioClip => m_AudioClip;

    }
}
