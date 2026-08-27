namespace Runtime.Settings {
    public class SensitivitySlider : SettingsSlider {
        protected override string KeyName => SettingBlackBoard.CAMERA_SENSITIVITY_KEY;
        
        protected override void ApplySetting(float value) {
            SettingBlackBoard.CameraSensitivity = value;
        }
    }
}