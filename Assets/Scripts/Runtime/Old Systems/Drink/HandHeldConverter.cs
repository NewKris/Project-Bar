using FMODUnity;
using Runtime.Audio;
using Runtime.Drinks.Converting;
using UnityEngine;

namespace Runtime.Old_Systems.Drink {
    public class HandHeldConverter : MonoBehaviour, IConverter {
        public Conversion[] conversions;
        public EventReference convertSound;
        
        public void Convert(IConvertable convertable) {
            convertable.ConvertIngredients(conversions);
            SfxManager.PlayOneShot(new OneShotConfig() {
                eventReference =  convertSound,
                attachedGameObject = gameObject
            });
        }

        private void OnValidate() {
            for (var i = 0; i < conversions.Length; i++) {
                conversions[i].UpdateName();
            }
        }
    }
}