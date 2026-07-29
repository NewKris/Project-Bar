using System;
using UnityEngine;

namespace Runtime {
    [Serializable]
    public class CustomHandleColors {
        public Color YAxisColor => new Color(yAxisHandleColor.r, yAxisHandleColor.g, yAxisHandleColor.b, 1f);
        public Color XAxisColor => new Color(xAxisHandleColor.r, xAxisHandleColor.g, xAxisHandleColor.b, 1f);
        public Color ZAxisColor => new Color(zAxisHandleColor.r, zAxisHandleColor.g, zAxisHandleColor.b, 1f);
        public Color XYPlaneColor => new Color(xyPlaneHandleColor.r, xyPlaneHandleColor.g, xyPlaneHandleColor.b, 1f);
        public Color YZPlaneColor => new Color(yzPlaneHandleColor.r, yzPlaneHandleColor.g, yzPlaneHandleColor.b, 1f);
        public Color XZPlaneColor => new Color(xzPlaneHandleColor.r, xzPlaneHandleColor.g, xzPlaneHandleColor.b, 1f);
        
        [ColorUsage(false, false)] [SerializeField] private Color yAxisHandleColor;
        [ColorUsage(false, false)] [SerializeField] private Color xAxisHandleColor;
        [ColorUsage(false, false)] [SerializeField] private Color zAxisHandleColor;
        [ColorUsage(false, false)] [SerializeField] private Color xyPlaneHandleColor;
        [ColorUsage(false, false)] [SerializeField] private Color yzPlaneHandleColor;
        [ColorUsage(false, false)] [SerializeField] private Color xzPlaneHandleColor;
    }
}