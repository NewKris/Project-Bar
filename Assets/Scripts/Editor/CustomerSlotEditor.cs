using UnityEngine;
using Runtime.Customers;
using UnityEditor;

namespace Editor {
    [CustomEditor(typeof(CustomerSlot))]
    public class CustomerSlotEditor : UnityEditor.Editor {
        public void OnSceneGUI()
        {
            CustomerSlot slot = (CustomerSlot)target;

            slot.customerSpawnPosition = CreatePositionHandles
            (
                slot.customerSpawnPosition,
                slot.spawnPositionHandleColors.YAxisColor,
                slot.spawnPositionHandleColors.XAxisColor,
                slot.spawnPositionHandleColors.ZAxisColor
            );
            
            slot.customerOrderPosition = CreatePositionHandles
            (
                slot.customerOrderPosition,
                slot.orderPositionHandleColors.YAxisColor,
                slot.orderPositionHandleColors.XAxisColor,
                slot.orderPositionHandleColors.ZAxisColor
            );
            
            slot.customerExitPosition = CreatePositionHandles
            (
                slot.customerExitPosition,
                slot.exitPositionHandleColors.YAxisColor,
                slot.exitPositionHandleColors.XAxisColor,
                slot.exitPositionHandleColors.ZAxisColor
            );
            
            
        }

        private Vector3 CreatePositionHandles(Vector3 position, Color yColor, Color xColor, Color zColor) {
            Vector3 newPosition = position;
            
            Handles.color = yColor;
            newPosition = Handles.Slider(newPosition, Vector3.up);
            
            Handles.color = xColor;
            newPosition = Handles.Slider(newPosition, Vector3.right);

            Handles.color = zColor;
            newPosition = Handles.Slider(newPosition, Vector3.forward);

            return newPosition;
        }
    }
}