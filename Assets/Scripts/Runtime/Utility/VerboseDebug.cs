using System;
using UnityEngine;

namespace Runtime.Utility {
    public enum LogLevel {
        NORMAL,
        WARNING,
        ERROR,
    }
    
    public static class VerboseDebug {
        public static bool enableVerboseLogging = false;

        public static void Log(string text, LogLevel level = LogLevel.NORMAL) {
            if (!enableVerboseLogging) return;
            
            switch (level) {
                case LogLevel.NORMAL:
                    Debug.Log(text);
                    break;
                case LogLevel.WARNING:
                    Debug.LogWarning(text);
                    break;
                case LogLevel.ERROR:
                    Debug.LogError(text);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, null);
            }
        }
    }
}