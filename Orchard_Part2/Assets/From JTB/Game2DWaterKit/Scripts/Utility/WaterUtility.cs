namespace Game2DWaterKit.Utils
{
    using UnityEngine;

    internal static class WaterUtility
    {
        private static SimpleFixedSizedList<Vector2> _outputPoints = new SimpleFixedSizedList<Vector2>(8);
        internal static bool ClipPointsAgainstEdge(SimpleFixedSizedList<Vector2> points, bool isHorizontalEdge, bool keepInside, float edgeValue)
        {
            int inputPointsCount = points.Count;

            if (inputPointsCount < 1)
                return false;

            _outputPoints.Clear();

            // vertical edge (right/left) => coord = 0 (vector2[0] = vector2.x)
            // horizontal edge (top/bottom) => coord = 1 (vector2[1] = vector2.y)
            int coord = isHorizontalEdge ? 1 : 0;

            Vector2 previousPoint = points[inputPointsCount - 1];
            bool isPreviousPointInside = keepInside ? (previousPoint[coord] > edgeValue) : (previousPoint[coord] < edgeValue);

            bool areInputPointsUnchanged = isPreviousPointInside;

            for (int i = 0; i < inputPointsCount; i++)
            {
                Vector2 currentPoint = points[i];
                bool isCurrentPointInside = keepInside ? (currentPoint[coord] > edgeValue) : (currentPoint[coord] < edgeValue);

                if (isCurrentPointInside != isPreviousPointInside)
                {
                    //intersection
                    Vector2 dir = currentPoint - previousPoint;
                    float x = !isHorizontalEdge ? edgeValue : previousPoint.x + (dir.x / dir.y) * (edgeValue - previousPoint.y);
                    float y = isHorizontalEdge ? edgeValue : previousPoint.y + (dir.y / dir.x) * (edgeValue - previousPoint.x);
                    _outputPoints.Add(new Vector2(x, y));

                    areInputPointsUnchanged = false;
                }

                if (isCurrentPointInside)
                    _outputPoints.Add(currentPoint);

                previousPoint = currentPoint;
                isPreviousPointInside = isCurrentPointInside;
            }

            points.CopyFrom(_outputPoints);

            return areInputPointsUnchanged;
        }
    }

    internal class SimpleFixedSizedList<T>
    {
        private T[] _elements;
        private int _count;

        internal SimpleFixedSizedList(int size)
        {
            _elements = new T[size];
            _count = 0;
        }
        
        internal int Count { get { return _count; } }

        internal T this[int index]
        {
            get
            {
                //if (index < 0 || index > _count - 1)
                //    Debug.LogError("Index is out of range!");

                return _elements[index];
            }
        }

        internal void Add(T point)
        {
            //if (_count == _points.Length)
            //    Debug.LogError("Max size reached!");

            _elements[_count] = point;
            _count++;
        }

        internal void Clear()
        {
            _count = 0;
        }

        internal void CopyFrom(SimpleFixedSizedList<T> points)
        {
            //if (points._count > _count)
            //    Debug.LogError("Source array is larger than destination!");

            _count = points._count;
            for (int i = 0; i < _count; i++)
            {
                _elements[i] = points[i];
            }
        }
    }
}
