using System;
using TMPro;
using UnityEngine;

namespace Runtime.Gameplay {
    public class GameOverSubtitle : MonoBehaviour {
        public static GameOverReason reason;

        public string otherReasonSubtitle;
        public string zeroSatisfactionSubtitle;
        public string wrongTargetSubtitle;
        
        private void Start() {
            GetComponent<TextMeshProUGUI>().text = ReasonToSubtitle(reason);
        }

        private string ReasonToSubtitle(GameOverReason gameOverReason) {
            return gameOverReason switch {
                GameOverReason.None => otherReasonSubtitle,
                GameOverReason.Satisfaction => zeroSatisfactionSubtitle,
                GameOverReason.WrongTarget => wrongTargetSubtitle,
                _ => throw new ArgumentOutOfRangeException(nameof(gameOverReason), gameOverReason, null)
            };
        }
    }
}