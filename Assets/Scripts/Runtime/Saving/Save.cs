using System;

namespace Runtime.Saving {
    [Serializable]
    public class Save {
        public static Save activeSave;
        
        public int saveSlotIndex;
    }
}
