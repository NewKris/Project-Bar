using System;
using Runtime.Player;
using TMPro;
using UnityEngine;

namespace Runtime.Dialogue
{
    public class DialogueBox : MonoBehaviour
    {
        public GameObject dialogueBox;
        public TMP_Text textComponent;
        
        private GameObject _player;

        private void Start()
        {
            _player = FindFirstObjectByType<Camera>().gameObject;
        }

        private void Update()
        {
            if (!_player) return;
            
            dialogueBox.transform.LookAt(_player.transform);
            dialogueBox.transform.eulerAngles = new Vector3(dialogueBox.transform.eulerAngles.x, dialogueBox.transform.eulerAngles.y+180, dialogueBox.transform.eulerAngles.z);
        }
    }
}