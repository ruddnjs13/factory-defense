using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    [RequireComponent(typeof(CanvasRenderer))]
    public class UILineRenderer : MaskableGraphic
    {
        [SerializeField] private float thickness = 5f;
        [SerializeField] private float segmentLength = 10f; // 필요시 점선 구현용
        public Color lineColor = Color.white;

        [field: SerializeField] 
        public RectTransform[] Points { get; set; } = new RectTransform[2];
        
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            if (Points == null || Points.Length < 2)
                return;

            // ✅ offset 제거 (Overlay 모드에서는 불필요)
            Vector3 offset = Vector3.zero;

            float globalDistance = 0f; // 점선 패턴 이어짐 유지용 (segmentLength 사용 시)

            for (int i = 0; i < Points.Length - 1; i++)
            {
                Vector3 start = Points[i].anchoredPosition;
                Vector3 end = Points[i + 1].anchoredPosition;

                Vector3 dir = (end - start).normalized;
                float distance = Vector3.Distance(start, end);

                float localPos = 0f;

                while (localPos < distance)
                {
                    // 점선 패턴 계산
                    float patternPos = (globalDistance + localPos) % segmentLength;
                    float remain = Mathf.Min(segmentLength - patternPos, distance - localPos);

                    Vector3 segStart = start + dir * localPos;
                    Vector3 segEnd = segStart + dir * remain;

                    AddSegment(segStart, segEnd, vh, offset);
                    localPos += remain;
                }

                globalDistance += distance;
            }
        }

        private void AddSegment(Vector3 point1, Vector3 point2, VertexHelper vh, Vector3 offset)
        {
            int startIndex = vh.currentVertCount;
            CreateLineSegment(point1, point2, vh, offset);
            vh.AddTriangle(startIndex, startIndex + 1, startIndex + 2);
            vh.AddTriangle(startIndex + 2, startIndex + 1, startIndex + 3);
        }

        private void CreateLineSegment(Vector3 point1, Vector3 point2, VertexHelper vh, Vector3 offset)
        {
            UIVertex vertex = UIVertex.simpleVert;
            vertex.color = lineColor;

            Vector2 dir = (point2 - point1).normalized;
            Vector2 normal = new Vector2(-dir.y, dir.x);
            Vector2 halfThickness = normal * (thickness * 0.5f);

            // ✅ offset 제거
            Vector3 v1 = point1 - (Vector3)halfThickness - offset;
            Vector3 v2 = point1 + (Vector3)halfThickness - offset;
            Vector3 v3 = point2 - (Vector3)halfThickness - offset;
            Vector3 v4 = point2 + (Vector3)halfThickness - offset;

            vertex.position = v1; vertex.uv0 = new Vector2(0, 0); vh.AddVert(vertex);
            vertex.position = v2; vertex.uv0 = new Vector2(0, 1); vh.AddVert(vertex);
            vertex.position = v3; vertex.uv0 = new Vector2(1, 0); vh.AddVert(vertex);
            vertex.position = v4; vertex.uv0 = new Vector2(1, 1); vh.AddVert(vertex);
        }
    }
}
