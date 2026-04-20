using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoadDirection
{
    North,
    South,
    East,
    West
}

public class SmartRoad : MonoBehaviour
{
    // =========================================
    // ROAD IDENTITY
    // =========================================
    [Header("Road Identity")]
    public RoadDirection roadDirection;

    // =========================================
    // LANE SETTINGS
    // =========================================
    [Header("Lane Settings")]
    [Min(1)] public int laneCount = 2;
    public float laneWidth = 3f;

    // =========================================
    // DIRECTION CONTROL
    // =========================================
    [Header("Direction Reference (Optional)")]
    public Transform rightReference;

    // =========================================
    // INTERNAL CACHE
    // =========================================
    private Vector3 roadForward;
    private Vector3 roadRight;
    private float[] laneOffsets;

    // =========================================
    // UNITY EVENTS
    // =========================================
    void Awake()
    {
        Initialize();
    }

    void OnValidate()
    {
        Initialize();
    }

    // =========================================
    // INITIALIZATION
    // =========================================
    void Initialize()
    {
        if (laneCount <= 0) laneCount = 1;

        // ===== FORWARD =====
        roadForward = transform.forward;
        roadForward.y = 0;

        if (roadForward == Vector3.zero)
        {
            Debug.LogWarning($"{name} forward = ZERO!");
            roadForward = Vector3.forward;
        }

        roadForward.Normalize();

        // ===== RIGHT =====
        if (rightReference != null)
        {
            roadRight = rightReference.right;
        }
        else
        {
            roadRight = transform.right;
        }

        roadRight.y = 0;

        if (roadRight == Vector3.zero)
        {
            Debug.LogWarning($"{name} right = ZERO!");
            roadRight = Vector3.right;
        }

        roadRight.Normalize();

        // ===== LANES =====
        CacheLanes();
    }

    // =========================================
    // LANE CACHE
    // =========================================
    void CacheLanes()
    {
        laneOffsets = new float[laneCount];

        float totalWidth = laneCount * laneWidth;
        float start = -totalWidth / 2f + laneWidth / 2f;

        for (int i = 0; i < laneCount; i++)
        {
            laneOffsets[i] = start + i * laneWidth;
        }
    }

    // =========================================
    // BASIC GETTERS
    // =========================================
    public Vector3 GetForward()
    {
        return roadForward;
    }

    public Vector3 GetRight()
    {
        return roadRight;
    }

    // =========================================
    // LANE OFFSET
    // =========================================
    public float GetLaneOffset(int lane)
    {
        if (laneOffsets == null || laneOffsets.Length == 0)
        {
            Debug.LogError($"{name} laneOffsets NULL!");
            return 0;
        }

        lane = Mathf.Clamp(lane, 0, laneOffsets.Length - 1);
        return laneOffsets[lane];
    }

    // =========================================
    // LANE DETECTION
    // =========================================
    public int GetClosestLane(Vector3 pos)
    {
        if (laneOffsets == null || laneOffsets.Length == 0)
            return 0;

        float lateral = Vector3.Dot(pos - transform.position, roadRight);

        int bestLane = 0;
        float bestDist = float.MaxValue;

        for (int i = 0; i < laneOffsets.Length; i++)
        {
            float dist = Mathf.Abs(lateral - laneOffsets[i]);

            if (dist < bestDist)
            {
                bestDist = dist;
                bestLane = i;
            }
        }

        return bestLane;
    }

    // =========================================
    // POSITION HELPERS
    // =========================================
    public Vector3 GetProjectedCenter(Vector3 pos)
    {
        Vector3 to = pos - transform.position;
        float forwardAmount = Vector3.Dot(to, roadForward);

        return transform.position + roadForward * forwardAmount;
    }

    public Vector3 GetLaneWorldPos(int lane, Vector3 refPos)
    {
        Vector3 center = GetProjectedCenter(refPos);
        return center + roadRight * GetLaneOffset(lane);
    }

    // =========================================
    // SNAP SYSTEM
    // =========================================
    public Vector3 SnapToRoad(Vector3 pos, int lane)
    {
        Vector3 center = GetProjectedCenter(pos);
        return center + roadRight * GetLaneOffset(lane);
    }

    // =========================================
    // DEBUG VISUAL (OPTIONAL TAPI BAGUS)
    // =========================================
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + GetForward() * 5f);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + GetRight() * 5f);
    }
}





//========== holy grail =============

//using UnityEngine;
//public class SmartRoad : MonoBehaviour
//{
//    // =========================
//    // LANE SETTINGS
//    // =========================

//    [Header("Lane Settings")]
//    [Min(1)]
//    public int laneCount = 2;

//    public float laneWidth = 3f;

//    // =========================
//    // DEBUG
//    // =========================

//    [Header("Debug")]
//    public bool drawDebug = true;
//    public float debugLength = 20f;

//    // =========================
//    // INTERNAL DIRECTION CACHE
//    // =========================

//    Vector3 roadForward;
//    Vector3 roadRight;

//    // =========================
//    // UNITY EVENTS
//    // =========================

//    void Awake()
//    {
//        CalculateDirection();
//    }

//    void OnValidate()
//    {
//        CalculateDirection();
//    }

//    // =========================
//    // DIRECTION CALCULATION
//    // =========================

//    void CalculateDirection()
//    {
//        // ambil arah forward dari rotasi object
//        roadForward = transform.forward;

//        roadForward.y = 0f;
//        roadForward.Normalize();

//        // hitung right dari forward
//        roadRight = Vector3.Cross(Vector3.up, roadForward);

//        roadRight.Normalize();
//    }

//    // =========================
//    // PUBLIC GETTERS
//    // =========================

//    public Vector3 GetForward()
//    {
//        return roadForward;
//    }

//    public Vector3 GetRight()
//    {
//        return roadRight;
//    }

//    // =========================
//    // LANE OFFSET
//    // =========================

//    public float GetLaneOffset(int laneIndex)
//    {
//        laneIndex = Mathf.Clamp(laneIndex, 0, laneCount - 1);

//        float totalWidth = laneCount * laneWidth;

//        float leftEdge =
//            -totalWidth / 2f + laneWidth / 2f;

//        return leftEdge + laneIndex * laneWidth;
//    }

//    // =========================
//    // ROAD PROJECTION
//    // =========================

//    public Vector3 GetProjectedCenterPosition(Vector3 worldPosition)
//    {
//        Vector3 toPoint =
//            worldPosition - transform.position;

//        float forwardAmount =
//            Vector3.Dot(toPoint, roadForward);

//        return transform.position +
//               roadForward * forwardAmount;
//    }

//    // =========================
//    // LATERAL OFFSET
//    // =========================

//    public float GetLateralOffset(Vector3 worldPosition)
//    {
//        Vector3 toPoint =
//            worldPosition - transform.position;

//        return Vector3.Dot(
//            toPoint,
//            roadRight
//        );
//    }

//    // =========================
//    // CLOSEST LANE
//    // =========================

//    public int GetClosestLaneIndex(Vector3 worldPosition)
//    {
//        float lateralOffset =
//            GetLateralOffset(worldPosition);

//        float closestDistance = float.MaxValue;

//        int closestLane = 0;

//        for (int i = 0; i < laneCount; i++)
//        {
//            float laneOffset =
//                GetLaneOffset(i);

//            float distance =
//                Mathf.Abs(lateralOffset - laneOffset);

//            if (distance < closestDistance)
//            {
//                closestDistance = distance;
//                closestLane = i;
//            }
//        }

//        return closestLane;
//    }

//    // =========================
//    // GET LANE WORLD POSITION
//    // =========================

//    public Vector3 GetLaneWorldPosition(int laneIndex, Vector3 worldPosition)
//    {
//        Vector3 center =
//            GetProjectedCenterPosition(worldPosition);

//        float offset =
//            GetLaneOffset(laneIndex);

//        return center +
//               roadRight * offset;
//    }

//    // =========================
//    // DEBUG GIZMOS
//    // =========================

//    private void OnDrawGizmos()
//    {
//        if (!drawDebug)
//            return;

//        CalculateDirection();

//        // road center
//        Gizmos.color = Color.yellow;

//        Gizmos.DrawLine(
//            transform.position - roadForward * debugLength,
//            transform.position + roadForward * debugLength
//        );

//        // lane lines
//        for (int i = 0; i < laneCount; i++)
//        {
//            float offset = GetLaneOffset(i);

//            Vector3 laneStart =
//                transform.position +
//                roadRight * offset -
//                roadForward * debugLength;

//            Vector3 laneEnd =
//                transform.position +
//                roadRight * offset +
//                roadForward * debugLength;

//            Gizmos.color = Color.cyan;

//            Gizmos.DrawLine(laneStart, laneEnd);
//        }
//    }
//}

//================================================================
//using UnityEngine;

//public class SmartRoad : MonoBehaviour
//{
//    [Header("Lane Settings")]
//    [Min(1)]
//    public int laneCount = 2;

//    public float laneWidth = 3f;

//    [Header("Debug")]
//    public bool drawDebug = true;
//    public float debugLength = 20f;

//    // =========================
//    // LANE OFFSET (CENTER-BASED)
//    // =========================
//    public float GetLaneOffset(int laneIndex)
//    {
//        laneIndex = Mathf.Clamp(laneIndex, 0, laneCount - 1);

//        float totalWidth = (laneCount - 1) * laneWidth;
//        float leftEdge = -totalWidth / 2f;

//        return leftEdge + (laneIndex * laneWidth);
//    }

//    // =========================
//    // DAPATKAN POSISI DI TENGAH JALAN
//    // =========================
//    public Vector3 GetProjectedCenterPosition(Vector3 worldPosition)
//    {
//        Vector3 toPoint = worldPosition - transform.position;

//        float forwardAmount = Vector3.Dot(
//            toPoint,
//            transform.forward
//        );

//        return transform.position +
//               transform.forward * forwardAmount;
//    }

//    // =========================
//    // CARI LANE TERDEKAT
//    // =========================
//    public int GetClosestLaneIndex(Vector3 worldPosition)
//    {
//        Vector3 toPoint = worldPosition - transform.position;

//        float lateralOffset = Vector3.Dot(
//            toPoint,
//            transform.right
//        );

//        float closestDistance = float.MaxValue;
//        int closestLane = 0;

//        for (int i = 0; i < laneCount; i++)
//        {
//            float laneOffset = GetLaneOffset(i);
//            float distance = Mathf.Abs(lateralOffset - laneOffset);

//            if (distance < closestDistance)
//            {
//                closestDistance = distance;
//                closestLane = i;
//            }
//        }

//        return closestLane;
//    }

//    // =========================
//    // GIZMO DEBUG
//    // =========================
//    private void OnDrawGizmos()
//    {
//        if (!drawDebug)
//            return;

//        Gizmos.color = Color.yellow;

//        // Draw center line
//        Gizmos.DrawLine(
//            transform.position - transform.forward * debugLength,
//            transform.position + transform.forward * debugLength
//        );

//        // Draw lane lines
//        for (int i = 0; i < laneCount; i++)
//        {
//            float offset = GetLaneOffset(i);

//            Vector3 laneStart =
//                transform.position +
//                transform.right * offset -
//                transform.forward * debugLength;

//            Vector3 laneEnd =
//                transform.position +
//                transform.right * offset +
//                transform.forward * debugLength;

//            Gizmos.color = Color.cyan;
//            Gizmos.DrawLine(laneStart, laneEnd);
//        }
//    }
//}








//using UnityEngine;

//public class SmartRoad : MonoBehaviour
//{
//    [Header("Lane Settings")]
//    [Min(1)]
//    public int laneCount = 2;

//    public float laneWidth = 3f;

//    // Mengembalikan offset relatif terhadap CENTER jalan
//    public float GetLaneOffset(int laneIndex)
//    {
//        laneIndex = Mathf.Clamp(laneIndex, 0, laneCount - 1);

//        float totalWidth = (laneCount - 1) * laneWidth;
//        float leftEdge = -totalWidth / 2f;

//        return leftEdge + (laneIndex * laneWidth);
//    }
//}




//using UnityEngine;

//public class SmartRoad : MonoBehaviour
//{
//    [Header("Lane Settings")]
//    public int laneCount = 2;
//    public float laneWidth = 3f;

//    // Hitung posisi X untuk lane tertentu
//    public float GetLanePositionX(int laneIndex)
//    {
//        // Untuk 2 lane:
//        // lane 0 = kiri
//        // lane 1 = kanan

//        float totalWidth = (laneCount - 1) * laneWidth;
//        float leftEdge = -totalWidth / 2f;

//        return leftEdge + (laneIndex * laneWidth);
//    }
//}
