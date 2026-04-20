using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowAdaptive : MonoBehaviour
{
    [Header("Target Player")]
    public Transform target;

    [Header("Smooth Follow Settings")]
    [Range(0.01f, 1f)]
    public float smoothSpeed = 0.125f;
    public Vector3 baseOffset = new Vector3(0f, 8f, -10f);

    [Header("Adaptive Camera Settings")]
    public float maxExtraHeight = 3f;
    public float maxExtraDistance = 4f;
    public float detectionRadius = 10f;
    public LayerMask wideAreaLayer; // Lebih efisien dari CompareTag

    private Vector3 currentOffset;
    private Quaternion fixedRotation;

    // =========================
    // START
    // =========================
    void Start()
    {
        if (target == null)
        {
            Debug.LogWarning("CameraFollowAdaptive: Target belum diset!");
            enabled = false;
            return;
        }

        currentOffset = baseOffset;
        fixedRotation = Quaternion.Euler(25f, -45f, 0f);
    }

    // =========================
    // LATE UPDATE
    // =========================
    void LateUpdate()
    {
        if (target == null) return;

        bool isWideArea = false;

        // Deteksi hanya layer tertentu (lebih ringan)
        Collider[] hits = Physics.OverlapSphere(
            target.position,
            detectionRadius,
            wideAreaLayer
        );

        if (hits.Length > 0)
            isWideArea = true;

        Vector3 desiredOffset = baseOffset;

        if (isWideArea)
        {
            desiredOffset += new Vector3(0f, maxExtraHeight, -maxExtraDistance);
        }

        currentOffset = Vector3.Lerp(
            currentOffset,
            desiredOffset,
            Time.deltaTime * 2f
        );

        Vector3 desiredPosition = target.position + currentOffset;

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed
        );

        transform.rotation = fixedRotation;
    }

    // =========================
    // DEBUG GIZMO
    // =========================
    void OnDrawGizmosSelected()
    {
        if (target == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(target.position, detectionRadius);
    }
}


//using UnityEngine;
//public class CameraFollowAdaptive : MonoBehaviour
//{
//    [Header("Target Player")]
//    public Transform target;

//    [Header("Smooth Follow Settings")]
//    public float smoothSpeed = 0.125f;
//    public Vector3 baseOffset = new Vector3(0f, 8f, -10f);

//    [Header("Adaptive Camera Settings")]
//    public float maxExtraHeight = 3f;     // Tambahan tinggi maksimal
//    public float maxExtraDistance = 4f;   // Tambahan jarak maksimal
//    public float detectionRadius = 10f;   // Radius deteksi area luas (misal perempatan)

//    private Vector3 currentOffset;

//    void Start()
//    {
//        currentOffset = baseOffset;
//    }

//    void LateUpdate()
//    {
//        if (target == null) return;

//        // ====== Bagian Deteksi Area ======
//        Collider[] areaObjects = Physics.OverlapSphere(target.position, detectionRadius);
//        bool isWideArea = false;

//        foreach (var obj in areaObjects)
//        {
//            if (obj.CompareTag("WideArea")) // Tag area lebar seperti perempatan
//            {
//                isWideArea = true;
//                break;
//            }
//        }

//        // ====== Transisi Offset Kamera ======
//        Vector3 desiredOffset = baseOffset;

//        if (isWideArea)
//        {
//            // Kamera sedikit naik dan mundur
//            desiredOffset += new Vector3(0, maxExtraHeight, -maxExtraDistance);
//        }

//        currentOffset = Vector3.Lerp(currentOffset, desiredOffset, Time.deltaTime * 2f);

//        // ====== Posisikan Kamera ======
//        Vector3 desiredPosition = target.position + currentOffset;
//        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
//        transform.position = smoothedPosition;

//        // Sudut tetap
//        transform.rotation = Quaternion.Euler(25f, -45f, 0f);
//    }

//    // ====== Visualisasi di Scene ======
//    void OnDrawGizmosSelected()
//    {
//        if (target != null)
//        {
//            Gizmos.color = Color.yellow;
//            Gizmos.DrawWireSphere(target.position, detectionRadius);
//        }
//    }
//}




//using UnityEngine;

//public class CameraFollow : MonoBehaviour
//{
//    public Transform target; // Player
//    public float smoothSpeed = 0.125f;
//    public Vector3 offset;   // Jarak kamera dari player

//    void LateUpdate()
//    {
//        if (target == null) return;

//        Vector3 desiredPosition = target.position + offset;
//        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
//        transform.position = smoothedPosition;

//        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
//        transform.rotation = Quaternion.Euler(25f, -45f, 0f);
//    }
//}
