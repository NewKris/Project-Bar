using System.Collections;
using System.Threading.Tasks;

namespace Runtime.Scene_Handling {
    public interface IGameSceneStartupTask {
        public IEnumerator StartupTask();
    }
}