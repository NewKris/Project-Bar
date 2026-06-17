namespace Runtime
{
    public sealed class GameModifiers
    {
        private float _activeTimeScale;
        public float ActiveTimeScale => _activeTimeScale;

        public void SetTimeScale(float timeScale)
        {
            _activeTimeScale = timeScale;
        }
        
        // Singleton logic
        private static GameModifiers _instance;

        private GameModifiers()
        {
            _activeTimeScale = 1.0f;
        }

        public static GameModifiers GetInstance()
        {
            if (_instance == null)
            {
                _instance = new GameModifiers();
            }
            return _instance;
        }
    }
}
