using FMOD.Studio;

namespace Runtime.Audio {
    public struct FmodParameter {
        public string parameterName;
        public object value;
        
        public static FmodParameter NoLooping => new () { parameterName = "Looping", value = 0 };

        public void AddParameterToInstance(EventInstance instance) {
            switch (value) {
                case string label:
                    instance.setParameterByNameWithLabel(parameterName, label);
                    break;
                case float floatValue:
                    instance.setParameterByName(parameterName, floatValue);
                    break;
                case int intValue:
                    instance.setParameterByName(parameterName, intValue);
                    break;
            }
        }
    }
}