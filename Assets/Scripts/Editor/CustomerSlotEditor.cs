using UnityEngine;
using UnityEditor;
using Assets.Scripts.Runtime.Customers.Spawning;

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
                slot.spawnPositionHandleColors.ZAxisColor,
                slot.spawnPositionHandleColors.XYPlaneColor,
                slot.spawnPositionHandleColors.YZPlaneColor,
                slot.spawnPositionHandleColors.XZPlaneColor
            );
            
            slot.customerOrderPosition = CreatePositionHandles
            (
                slot.customerOrderPosition,
                slot.orderPositionHandleColors.YAxisColor,
                slot.orderPositionHandleColors.XAxisColor,
                slot.orderPositionHandleColors.ZAxisColor,
                slot.orderPositionHandleColors.XYPlaneColor,
                slot.orderPositionHandleColors.YZPlaneColor,
                slot.orderPositionHandleColors.XZPlaneColor
            );
            
            slot.customerExitPosition = CreatePositionHandles
            (
                slot.customerExitPosition,
                slot.exitPositionHandleColors.YAxisColor,
                slot.exitPositionHandleColors.XAxisColor,
                slot.exitPositionHandleColors.ZAxisColor,
                slot.exitPositionHandleColors.XYPlaneColor,
                slot.exitPositionHandleColors.YZPlaneColor,
                slot.exitPositionHandleColors.XZPlaneColor
            );
            
            
        }

        private Vector3 CreatePositionHandles(Vector3 position, Color yColor, Color xColor, Color zColor, Color xyColor, Color zyColor, Color xzColor) {
            Vector3 newPosition = position;
            
            Handles.color = yColor;
            newPosition = Handles.Slider(newPosition, Vector3.up);
            
            Handles.color = xColor;
            newPosition = Handles.Slider(newPosition, Vector3.right);

            Handles.color = zColor;
            newPosition = Handles.Slider(newPosition, Vector3.forward);
            
            Handles.color = xyColor;
            newPosition = Handles.Slider2D(
                0,
                newPosition, 
                (Vector3.right + Vector3.up).normalized,
                Vector3.forward, 
                Vector3.right,
                Vector3.up,
                0.5f,
                Handles.RectangleHandleCap,
                new Vector2(0.2f, 0.2f)
            );
            
            Handles.color = zyColor;
            newPosition = Handles.Slider2D(
                1,
                newPosition, 
                (Vector3.forward + Vector3.up).normalized,
                Vector3.right, 
                Vector3.forward,
                Vector3.up,
                0.5f,
                Handles.RectangleHandleCap,
                new Vector2(0.2f, 0.2f)
            );
            
            Handles.color = xzColor;
            newPosition = Handles.Slider2D(
                2,
                newPosition,
                 
                (Vector3.right + Vector3.forward).normalized,
                Vector3.up, 
                Vector3.right,
                Vector3.forward,
                0.5f,
                Handles.RectangleHandleCap,
                new Vector2(0.2f, 0.2f)
            );

            return newPosition;
        }
    }
}