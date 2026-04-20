using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    // =========================================
    // MOVEMENT SETTINGS
    // =========================================
    [Header("Movement")]
    public float speed = 10f;
    public float laneSpeed = 8f;

    // =========================================
    // INTERNAL STATE
    // =========================================
    private Rigidbody rb;
    private SmartRoad currentRoad;

    private float vertical;

    private int lane;
    private float laneOffset;
    private float targetLaneOffset;

    private bool inIntersection;
    private bool canMove = true;

    // =========================================
    // UNITY EVENTS
    // =========================================
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

    }

    void Update()
    {
        vertical = Input.GetAxisRaw("Vertical");

        if (!canMove || inIntersection || currentRoad == null) return;

        // Lane switching
        if (Input.GetKeyDown(KeyCode.A))
        {
            lane = Mathf.Max(0, lane - 1);
            UpdateLane();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            lane = Mathf.Min(currentRoad.laneCount - 1, lane + 1);
            UpdateLane();
        }
    }

    void FixedUpdate()
    {
        if (currentRoad == null) return;
        if (!canMove || inIntersection) return;

        Move();
    }

    // =========================================
    // MOVEMENT CORE
    // =========================================
    void Move()
    {
        Vector3 forward = currentRoad.GetForward();

        // AUTO FIX arah supaya W selalu maju
        if (Vector3.Dot(forward, transform.forward) < 0)
        {
            forward = -forward;
        }

        Vector3 right = currentRoad.GetRight();

        Vector3 move = forward * vertical * speed * Time.fixedDeltaTime;
        Vector3 pos = rb.position + move;

        Vector3 center = currentRoad.GetProjectedCenter(pos);

        laneOffset = Mathf.Lerp(
            laneOffset,
            targetLaneOffset,
            laneSpeed * Time.fixedDeltaTime
        );

        Vector3 final = center + right * laneOffset;

        rb.MovePosition(new Vector3(final.x, rb.position.y, final.z));

        // ROTASI IKUT ARAH JALAN
        rb.MoveRotation(Quaternion.LookRotation(forward));
    }

    // =========================================
    // LANE MANAGEMENT
    // =========================================
    void UpdateLane()
    {
        if (currentRoad == null) return;

        targetLaneOffset = currentRoad.GetLaneOffset(lane);
    }

    // =========================================
    // ROAD MANAGEMENT
    // =========================================
    public void SetRoad(SmartRoad road)
    {
        if (road == null)
        {
            Debug.LogError("SetRoad(NULL) called!");
            return;
        }

        currentRoad = road;

        lane = road.GetClosestLane(transform.position);
        targetLaneOffset = road.GetLaneOffset(lane);
        laneOffset = targetLaneOffset;

        Debug.Log($"ROAD SET: {road.name}, Lane: {lane}");
    }

    public SmartRoad GetRoad() => currentRoad;
    public int GetLane() => lane;

    // =========================================
    // MOVEMENT CONTROL
    // =========================================
    public void StopMovement()
    {
        canMove = false;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    public void ResumeMovement()
    {
        canMove = true;
    }

    // =========================================
    // INTERSECTION STATE
    // =========================================
    public void SetIntersection(bool state)
    {
        inIntersection = state;

        if (!state)
        {
            vertical = Input.GetAxisRaw("Vertical");
            UpdateLane();
            Debug.Log("INTERSECTION EXIT - Movement Resumed");
        }
    }
}




////=========================
//// Refresh SmartRoad
//// =========================
//public void RefreshCurrentRoad()
//{
//    Collider[] hits = Physics.OverlapSphere(transform.position, 2f);

//    foreach (Collider col in hits)
//    {
//        SmartRoad road = col.GetComponent<SmartRoad>();

//        if (road != null)
//        {
//            currentRoad = road;

//            currentLane = road.GetClosestLaneIndex(rb.position);

//            targetLaneOffset = road.GetLaneOffset(currentLane);
//            currentLaneOffset = targetLaneOffset;

//            return;
//        }
//    }
//}



//using UnityEngine;

//[RequireComponent(typeof(Rigidbody))]
//public class PlayerMovement : MonoBehaviour
//{
//    [Header("Forward Movement")]
//    public float forwardSpeed = 10f;
//    public bool canControl = true;

//    [Header("Lane Movement")]
//    public float laneChangeSpeed = 8f;

//    private Rigidbody rb;
//    private SmartRoad currentRoad;

//    private int currentLane = 0;
//    private float currentLaneOffset = 0f;
//    private float targetLaneOffset = 0f;

//    private float verticalInput;

//    private TurnAreaTrigger currentTurnArea;

//    // =========================
//    // START
//    // =========================
//    void Start()
//    {
//        rb = GetComponent<Rigidbody>();

//        rb.constraints = RigidbodyConstraints.FreezeRotation;
//        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
//        rb.interpolation = RigidbodyInterpolation.Interpolate;
//    }

//    // =========================
//    // UPDATE (INPUT)
//    // =========================
//    void Update()
//    {
//        verticalInput = canControl ? Input.GetAxis("Vertical") : 0f;

//        if (canControl)
//        {
//            HandleLaneInput();
//        }

//        HandleTurnInput();
//        HandleLaneInput();
//    }

//    void HandleLaneInput()
//    {
//        if (currentRoad == null) return;

//        if (Input.GetKeyDown(KeyCode.A))
//        {
//            if (currentLane > 0)
//            {
//                currentLane--;
//                UpdateTargetLane();
//            }
//        }

//        if (Input.GetKeyDown(KeyCode.D))
//        {
//            if (currentLane < currentRoad.laneCount - 1)
//            {
//                currentLane++;
//                UpdateTargetLane();
//            }
//        }
//    }

//    void HandleTurnInput()
//    {
//        if (currentTurnArea == null) return;

//        if (Input.GetKeyDown(KeyCode.A))
//        {
//            currentTurnArea.TurnLeft(this);
//        }

//        if (Input.GetKeyDown(KeyCode.D))
//        {
//            currentTurnArea.TurnRight(this);
//        }

//        if (Input.GetKeyDown(KeyCode.W))
//        {
//            currentTurnArea.GoStraight(this);
//        }
//    }

//    // =========================
//    // PHYSICS
//    // =========================
//    void FixedUpdate()
//    {
//        if (!canControl)
//        {
//            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
//            return;
//        }

//        HandleForward();
//        HandleLaneMovement();
//    }

//    // =========================
//    // FORWARD
//    // =========================
//    void HandleForward()
//    {
//        Vector3 forwardMove =
//            transform.forward * verticalInput * forwardSpeed;

//        rb.velocity = new Vector3(
//            forwardMove.x,
//            rb.velocity.y,
//            forwardMove.z
//        );
//    }

//    // =========================
//    // LANE MOVEMENT
//    // =========================
//    void HandleLaneMovement()
//    {
//        if (currentRoad == null) return;

//        currentLaneOffset = Mathf.MoveTowards(
//            currentLaneOffset,
//            targetLaneOffset,
//            laneChangeSpeed * Time.fixedDeltaTime
//        );

//        Vector3 roadCenter = currentRoad.GetProjectedCenterPosition(rb.position);

//        Vector3 finalPosition =
//            roadCenter +
//            currentRoad.transform.right * currentLaneOffset;

//        rb.MovePosition(new Vector3(
//            finalPosition.x,
//            rb.position.y,
//            finalPosition.z
//        ));
//    }

//    void UpdateTargetLane()
//    {
//        if (currentRoad == null) return;

//        targetLaneOffset = currentRoad.GetLaneOffset(currentLane);
//    }

//    // =========================
//    // TRIGGER DETECTION
//    // =========================
//    private void OnTriggerEnter(Collider other)
//    {
//        Debug.Log("Trigger hit: " + other.name);

//        // =========================
//        // SMART ROAD DETECTION
//        // =========================
//        SmartRoad road = other.GetComponent<SmartRoad>();

//        if (road != null)
//        {
//            Debug.Log("SmartRoad detected");

//            currentRoad = road;

//            currentLane = road.GetClosestLaneIndex(rb.position);

//            targetLaneOffset = currentRoad.GetLaneOffset(currentLane);
//            currentLaneOffset = targetLaneOffset;

//            return;
//        }

//        // =========================
//        // TURN AREA DETECTION
//        // =========================
//        TurnAreaTrigger turn = other.GetComponent<TurnAreaTrigger>();

//        if (turn != null)
//        {
//            Debug.Log("TurnArea detected");

//            currentTurnArea = turn;
//            turn.PlayerEntered(this);
//        }
//    }

//    private void OnTriggerExit(Collider other)
//    {
//        SmartRoad road = other.GetComponent<SmartRoad>();

//        if (road != null && road == currentRoad)
//        {
//            currentRoad = null;
//        }

//        TurnAreaTrigger turn = other.GetComponent<TurnAreaTrigger>();

//        if (turn != null && turn == currentTurnArea)
//        {
//            turn.PlayerExit(this);
//            currentTurnArea = null;
//        }
//    }

//    // =========================
//    // STOP MOVEMENT (NEW)
//    // =========================
//    public void StopMovement()
//    {
//        canControl = false;

//        rb.velocity = Vector3.zero;
//        rb.angularVelocity = Vector3.zero;
//    }

//    // =========================
//    // RESUME MOVEMENT
//    // =========================
//    public void ResumeMovement()
//    {
//        canControl = true;
//    }

//    // =========================
//    // Refresh SmartRoad
//    // =========================
//    public void RefreshCurrentRoad()
//    {
//        Collider[] hits = Physics.OverlapSphere(transform.position, 2f);

//        foreach (Collider col in hits)
//        {
//            SmartRoad road = col.GetComponent<SmartRoad>();

//            if (road != null)
//            {
//                currentRoad = road;

//                currentLane = road.GetClosestLaneIndex(rb.position);

//                targetLaneOffset = road.GetLaneOffset(currentLane);
//                currentLaneOffset = targetLaneOffset;

//                return;
//            }
//        }
//    }
//}


//using UnityEngine;

//[RequireComponent(typeof(Rigidbody))]
//public class PlayerMovement : MonoBehaviour
//{
//    [Header("Forward Movement")]
//    public float forwardSpeed = 10f;
//    public bool canControl = true;

//    [Header("Lane Movement")]
//    public float laneChangeSpeed = 8f;

//    private Rigidbody rb;
//    private SmartRoad currentRoad;

//    private int currentLane = 0;
//    private float currentLaneOffset = 0f;
//    private float targetLaneOffset = 0f;

//    private float verticalInput;

//    private TurnAreaTrigger currentTurnArea;

//    // =========================
//    // START
//    // =========================
//    void Start()
//    {
//        rb = GetComponent<Rigidbody>();

//        rb.constraints = RigidbodyConstraints.FreezeRotation;
//        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
//        rb.interpolation = RigidbodyInterpolation.Interpolate;
//    }

//    // =========================
//    // UPDATE (INPUT)
//    // =========================
//    void Update()
//    {
//        if (!canControl)
//        {
//            verticalInput = 0f;
//            return;
//        }

//        verticalInput = Input.GetAxis("Vertical");

//        HandleLaneInput();
//        HandleTurnInput();
//    }

//    void HandleLaneInput()
//    {
//        if (currentRoad == null) return;

//        if (Input.GetKeyDown(KeyCode.A))
//        {
//            if (currentLane > 0)
//            {
//                currentLane--;
//                UpdateTargetLane();
//            }
//        }

//        if (Input.GetKeyDown(KeyCode.D))
//        {
//            if (currentLane < currentRoad.laneCount - 1)
//            {
//                currentLane++;
//                UpdateTargetLane();
//            }
//        }
//    }

//    void HandleTurnInput()
//    {
//        if (currentTurnArea == null) return;

//        if (Input.GetKeyDown(KeyCode.A))
//        {
//            currentTurnArea.TurnLeft(this);
//        }

//        if (Input.GetKeyDown(KeyCode.D))
//        {
//            currentTurnArea.TurnRight(this);
//        }

//        if (Input.GetKeyDown(KeyCode.W))
//        {
//            currentTurnArea.GoStraight(this);
//        }
//    }

//    // =========================
//    // PHYSICS
//    // =========================
//    void FixedUpdate()
//    {
//        if (!canControl)
//        {
//            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
//            return;
//        }

//        HandleForward();
//        HandleLaneMovement();
//    }

//    // =========================
//    // FORWARD
//    // =========================
//    void HandleForward()
//    {
//        Vector3 forwardMove =
//            transform.forward * verticalInput * forwardSpeed;

//        rb.velocity = new Vector3(
//            forwardMove.x,
//            rb.velocity.y,
//            forwardMove.z
//        );
//    }

//    // =========================
//    // LANE MOVEMENT
//    // =========================
//    void HandleLaneMovement()
//    {
//        if (currentRoad == null) return;

//        currentLaneOffset = Mathf.MoveTowards(
//            currentLaneOffset,
//            targetLaneOffset,
//            laneChangeSpeed * Time.fixedDeltaTime
//        );

//        Vector3 roadCenter = currentRoad.GetProjectedCenterPosition(rb.position);

//        Vector3 finalPosition =
//            roadCenter +
//            currentRoad.transform.right * currentLaneOffset;

//        rb.MovePosition(new Vector3(
//            finalPosition.x,
//            rb.position.y,
//            finalPosition.z
//        ));
//    }

//    void UpdateTargetLane()
//    {
//        if (currentRoad == null) return;

//        targetLaneOffset = currentRoad.GetLaneOffset(currentLane);
//    }

//    // =========================
//    // TRIGGER DETECTION
//    // =========================
//    //private void OnTriggerEnter(Collider other)
//    //{
//    //    SmartRoad road = other.GetComponent<SmartRoad>();

//    //    if (road != null)
//    //    {
//    //        currentRoad = road;

//    //        currentLane = road.GetClosestLaneIndex(rb.position);

//    //        targetLaneOffset = currentRoad.GetLaneOffset(currentLane);
//    //        currentLaneOffset = targetLaneOffset;

//    //        return;
//    //    }

//    //    TurnAreaTrigger turn = other.GetComponent<TurnAreaTrigger>();

//    //    if (turn != null)
//    //    {
//    //        currentTurnArea = turn;
//    //        turn.PlayerEntered(this);
//    //    }
//    //}
//    private void OnTriggerEnter(Collider other)
//    {
//        Debug.Log("Trigger hit: " + other.name);

//        // =========================
//        // SMART ROAD DETECTION
//        // =========================
//        SmartRoad road = other.GetComponent<SmartRoad>();

//        if (road != null)
//        {
//            Debug.Log("SmartRoad detected");

//            currentRoad = road;

//            currentLane = road.GetClosestLaneIndex(rb.position);

//            targetLaneOffset = currentRoad.GetLaneOffset(currentLane);
//            currentLaneOffset = targetLaneOffset;

//            return;
//        }

//        // =========================
//        // TURN AREA DETECTION
//        // =========================
//        TurnAreaTrigger turn = other.GetComponent<TurnAreaTrigger>();

//        if (turn != null)
//        {
//            Debug.Log("TurnArea detected");

//            currentTurnArea = turn;
//            turn.PlayerEntered(this);
//        }
//    }
//    private void OnTriggerExit(Collider other)
//    {
//        SmartRoad road = other.GetComponent<SmartRoad>();

//        if (road != null && road == currentRoad)
//        {
//            currentRoad = null;
//        }

//        TurnAreaTrigger turn = other.GetComponent<TurnAreaTrigger>();

//        if (turn != null && turn == currentTurnArea)
//        {
//            turn.PlayerExit(this);
//            currentTurnArea = null;
//        }
//    }
//}


//using UnityEngine;

//[RequireComponent(typeof(Rigidbody))]
//public class PlayerMovement : MonoBehaviour
//{
//    [Header("Forward Movement")]
//    public float forwardSpeed = 10f;
//    public bool canControl = true;

//    [Header("Lane Movement")]
//    public float laneChangeSpeed = 8f;

//    private Rigidbody rb;
//    private SmartRoad currentRoad;

//    private int currentLane = 0;
//    private float currentLaneOffset = 0f;
//    private float targetLaneOffset = 0f;

//    private float verticalInput;

//    // =========================
//    // START
//    // =========================
//    void Start()
//    {
//        rb = GetComponent<Rigidbody>();

//        rb.constraints = RigidbodyConstraints.FreezeRotation;
//        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
//        rb.interpolation = RigidbodyInterpolation.Interpolate;
//    }

//    // =========================
//    // UPDATE (INPUT ONLY)
//    // =========================
//    void Update()
//    {
//        if (!canControl)
//        {
//            verticalInput = 0f;
//            return;
//        }

//        verticalInput = Input.GetAxis("Vertical");

//        if (currentRoad == null)
//            return;

//        if (Input.GetKeyDown(KeyCode.A))
//        {
//            if (currentLane > 0)
//            {
//                currentLane--;
//                UpdateTargetLane();
//            }
//        }

//        if (Input.GetKeyDown(KeyCode.D))
//        {
//            if (currentLane < currentRoad.laneCount - 1)
//            {
//                currentLane++;
//                UpdateTargetLane();
//            }
//        }
//    }

//    // =========================
//    // FIXED UPDATE (PHYSICS)
//    // =========================
//    void FixedUpdate()
//    {
//        if (!canControl || currentRoad == null)
//        {
//            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
//            return;
//        }

//        HandleForward();
//        HandleLaneMovement();
//    }

//    // =========================
//    // FORWARD MOVEMENT
//    // =========================
//    void HandleForward()
//    {
//        Vector3 forwardMove =
//            transform.forward * verticalInput * forwardSpeed;

//        rb.velocity = new Vector3(
//            forwardMove.x,
//            rb.velocity.y,
//            forwardMove.z
//        );
//    }

//    // =========================
//    // LANE MOVEMENT
//    // =========================
//    void HandleLaneMovement()
//    {
//        currentLaneOffset = Mathf.MoveTowards(
//            currentLaneOffset,
//            targetLaneOffset,
//            laneChangeSpeed * Time.fixedDeltaTime
//        );

//        Vector3 roadCenter = currentRoad.GetProjectedCenterPosition(rb.position);

//        Vector3 finalPosition =
//            roadCenter +
//            currentRoad.transform.right * currentLaneOffset;

//        rb.MovePosition(new Vector3(
//            finalPosition.x,
//            rb.position.y,
//            finalPosition.z
//        ));
//    }

//    // =========================
//    // LANE TARGET UPDATE
//    // =========================
//    void UpdateTargetLane()
//    {
//        if (currentRoad == null) return;

//        targetLaneOffset = currentRoad.GetLaneOffset(currentLane);
//    }

//    // =========================
//    // ROAD DETECTION
//    // =========================
//    private void OnTriggerEnter(Collider other)
//    {
//        SmartRoad road = other.GetComponent<SmartRoad>();

//        if (road == null) return;

//        currentRoad = road;

//        currentLane = road.GetClosestLaneIndex(rb.position);

//        targetLaneOffset = currentRoad.GetLaneOffset(currentLane);
//        currentLaneOffset = targetLaneOffset;
//    }

//    private void OnTriggerExit(Collider other)
//    {
//        SmartRoad road = other.GetComponent<SmartRoad>();

//        if (road == null) return;

//        if (road == currentRoad)
//        {
//            currentRoad = null;
//        }
//    }
//}



//using UnityEngine;

//[RequireComponent(typeof(Rigidbody))]
//public class PlayerMovement : MonoBehaviour
//{
//    [Header("Forward Movement")]
//    public float forwardSpeed = 10f;
//    public bool canControl = true;

//    [Header("Lane Movement")]
//    public float laneChangeSpeed = 8f;

//    private Rigidbody rb;
//    private SmartRoad currentRoad;

//    private int currentLane = 0;

//    private float currentLaneOffset = 0f;
//    private float targetLaneOffset = 0f;

//    void Start()
//    {
//        rb = GetComponent<Rigidbody>();
//        rb.constraints = RigidbodyConstraints.FreezeRotation;
//        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
//    }

//    void Update()
//    {
//        if (!canControl || currentRoad == null)
//            return;

//        if (Input.GetKeyDown(KeyCode.A))
//        {
//            if (currentLane > 0)
//            {
//                currentLane--;
//                UpdateTargetLane();
//            }
//        }

//        if (Input.GetKeyDown(KeyCode.D))
//        {
//            if (currentLane < currentRoad.laneCount - 1)
//            {
//                currentLane++;
//                UpdateTargetLane();
//            }
//        }
//    }

//    void FixedUpdate()
//    {
//        if (!canControl)
//        {
//            rb.velocity = Vector3.zero;
//            return;
//        }

//        HandleForward();
//        HandleLaneMovement();
//    }

//    void HandleForward()
//    {
//        float input = Input.GetAxis("Vertical");

//        Vector3 forwardMove = transform.forward * input * forwardSpeed;

//        rb.velocity = new Vector3(
//            forwardMove.x,
//            rb.velocity.y,
//            forwardMove.z
//        );
//    }

//    void HandleLaneMovement()
//    {
//        if (currentRoad == null)
//            return;

//        currentLaneOffset = Mathf.MoveTowards(
//            currentLaneOffset,
//            targetLaneOffset,
//            laneChangeSpeed * Time.fixedDeltaTime
//        );

//        Vector3 roadCenter = GetProjectedPositionOnRoad();

//        Vector3 finalPosition =
//            roadCenter +
//            currentRoad.transform.right * currentLaneOffset;

//        rb.MovePosition(new Vector3(
//            finalPosition.x,
//            rb.position.y,
//            finalPosition.z
//        ));
//    }

//    void UpdateTargetLane()
//    {
//        if (currentRoad == null)
//            return;

//        targetLaneOffset = currentRoad.GetLaneOffset(currentLane);
//    }

//    Vector3 GetProjectedPositionOnRoad()
//    {
//        Vector3 toCar = rb.position - currentRoad.transform.position;

//        float forwardAmount = Vector3.Dot(
//            toCar,
//            currentRoad.transform.forward
//        );

//        return currentRoad.transform.position +
//               currentRoad.transform.forward * forwardAmount;
//    }

//    int GetClosestLaneIndex(SmartRoad road)
//    {
//        Vector3 toCar = rb.position - road.transform.position;

//        float lateralAmount = Vector3.Dot(
//            toCar,
//            road.transform.right
//        );

//        int closestLane = 0;
//        float closestDistance = Mathf.Infinity;

//        for (int i = 0; i < road.laneCount; i++)
//        {
//            float laneOffset = road.GetLaneOffset(i);
//            float distance = Mathf.Abs(lateralAmount - laneOffset);

//            if (distance < closestDistance)
//            {
//                closestDistance = distance;
//                closestLane = i;
//            }
//        }

//        return closestLane;
//    }

//    private void OnTriggerEnter(Collider other)
//    {
//        SmartRoad road = other.GetComponent<SmartRoad>();

//        if (road != null)
//        {
//            currentRoad = road;

//            //  Ambil lane berdasarkan posisi sekarang
//            currentLane = GetClosestLaneIndex(road);

//            targetLaneOffset = currentRoad.GetLaneOffset(currentLane);
//            currentLaneOffset = targetLaneOffset;
//        }
//    }

//    private void OnTriggerExit(Collider other)
//    {
//        SmartRoad road = other.GetComponent<SmartRoad>();

//        if (road != null && road == currentRoad)
//        {
//            currentRoad = null;
//        }
//    }
//}




//using UnityEngine;

//[RequireComponent(typeof(Rigidbody))]
//public class PlayerMovement : MonoBehaviour
//{
//    [Header("Forward Movement")]
//    public float forwardSpeed = 10f;
//    public bool canControl = true;

//    [Header("Lane Movement")]
//    public float laneChangeSpeed = 8f;

//    private Rigidbody rb;
//    private SmartRoad currentRoad;

//    private int currentLane = 0;

//    private float currentLaneOffset = 0f;
//    private float targetLaneOffset = 0f;

//    void Start()
//    {
//        rb = GetComponent<Rigidbody>();

//        rb.constraints = RigidbodyConstraints.FreezeRotation;
//        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
//    }

//    void Update()
//    {
//        if (!canControl || currentRoad == null)
//            return;

//        // Pindah kiri
//        if (Input.GetKeyDown(KeyCode.A))
//        {
//            if (currentLane > 0)
//            {
//                currentLane--;
//                UpdateTargetLane();
//            }
//        }

//        // Pindah kanan
//        if (Input.GetKeyDown(KeyCode.D))
//        {
//            if (currentLane < currentRoad.laneCount - 1)
//            {
//                currentLane++;
//                UpdateTargetLane();
//            }
//        }
//    }

//    void FixedUpdate()
//    {
//        if (!canControl)
//        {
//            rb.velocity = Vector3.zero;
//            return;
//        }

//        HandleForward();
//        HandleLaneMovement();
//    }

//    void HandleForward()
//    {
//        float input = Input.GetAxis("Vertical");

//        Vector3 forwardMove = transform.forward * input * forwardSpeed;

//        rb.velocity = new Vector3(
//            forwardMove.x,
//            rb.velocity.y,
//            forwardMove.z
//        );
//    }

//    void HandleLaneMovement()
//    {
//        if (currentRoad == null)
//            return;

//        // Smooth menuju offset target
//        currentLaneOffset = Mathf.MoveTowards(
//            currentLaneOffset,
//            targetLaneOffset,
//            laneChangeSpeed * Time.fixedDeltaTime
//        );

//        // Hitung posisi tengah jalan (proyeksi ke forward jalan)
//        Vector3 roadCenter = GetProjectedPositionOnRoad();

//        // Geser ke kanan/kiri sesuai lane
//        Vector3 finalPosition =
//            roadCenter +
//            currentRoad.transform.right * currentLaneOffset;

//        rb.MovePosition(new Vector3(
//            finalPosition.x,
//            rb.position.y,
//            finalPosition.z
//        ));
//    }

//    void UpdateTargetLane()
//    {
//        if (currentRoad == null)
//            return;

//        targetLaneOffset = currentRoad.GetLaneOffset(currentLane);
//    }

//    Vector3 GetProjectedPositionOnRoad()
//    {
//        Vector3 toCar = rb.position - currentRoad.transform.position;

//        float forwardAmount = Vector3.Dot(
//            toCar,
//            currentRoad.transform.forward
//        );

//        return currentRoad.transform.position +
//               currentRoad.transform.forward * forwardAmount;
//    }

//    private void OnTriggerEnter(Collider other)
//    {
//        SmartRoad road = other.GetComponent<SmartRoad>();

//        if (road != null)
//        {
//            currentRoad = road;

//            currentLane = Mathf.Clamp(currentLane, 0, currentRoad.laneCount - 1);

//            targetLaneOffset = currentRoad.GetLaneOffset(currentLane);
//            currentLaneOffset = targetLaneOffset;
//        }
//    }

//    private void OnTriggerExit(Collider other)
//    {
//        SmartRoad road = other.GetComponent<SmartRoad>();

//        if (road != null && road == currentRoad)
//        {
//            currentRoad = null;
//        }
//    }
//}









//using UnityEngine;

//[RequireComponent(typeof(Rigidbody))]
//public class PlayerMovement : MonoBehaviour
//{
//    [Header("Forward Movement")]
//    public float forwardSpeed = 10f;
//    public bool canControl = true;

//    [Header("Lane Movement")]
//    public float laneChangeSpeed = 10f;

//    private Rigidbody rb;
//    private SmartRoad currentRoad;

//    private int currentLane = 0;

//    private Vector3 targetPosition;
//    private const float laneSnapThreshold = 0.01f;

//    void Start()
//    {
//        rb = GetComponent<Rigidbody>();

//        rb.constraints = RigidbodyConstraints.FreezeRotation;
//        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

//        targetPosition = rb.position;
//    }

//    void Update()
//    {
//        if (!canControl || currentRoad == null)
//            return;

//        // Pindah kiri
//        if (Input.GetKeyDown(KeyCode.A))
//        {
//            if (currentLane > 0)
//            {
//                currentLane--;
//                UpdateTargetLane();
//            }
//        }

//        // Pindah kanan
//        if (Input.GetKeyDown(KeyCode.D))
//        {
//            if (currentLane < currentRoad.laneCount - 1)
//            {
//                currentLane++;
//                UpdateTargetLane();
//            }
//        }
//    }

//    void FixedUpdate()
//    {
//        if (!canControl)
//        {
//            rb.velocity = Vector3.zero;
//            return;
//        }

//        HandleForward();
//        HandleLaneMovement();
//    }

//    // FORWARD SEKARANG LOCAL-BASED
//    void HandleForward()
//    {
//        float input = Input.GetAxis("Vertical");

//        Vector3 forwardMove = transform.forward * input * forwardSpeed;

//        rb.velocity = new Vector3(
//            forwardMove.x,
//            rb.velocity.y,
//            forwardMove.z
//        );
//    }

//    // LANE MOVEMENT SEKARANG IKUT ARAH JALAN
//    void HandleLaneMovement()
//    {
//        if (currentRoad == null)
//            return;

//        Vector3 newPosition = Vector3.MoveTowards(
//            rb.position,
//            targetPosition,
//            laneChangeSpeed * Time.fixedDeltaTime
//        );

//        if (Vector3.Distance(newPosition, targetPosition) < laneSnapThreshold)
//            newPosition = targetPosition;

//        rb.MovePosition(newPosition);
//    }

//    void UpdateTargetLane()
//    {
//        if (currentRoad == null)
//            return;

//        float laneOffset = currentRoad.GetLaneOffset(currentLane);

//        // Offset mengikuti arah kanan jalan, bukan world X
//        Vector3 laneWorldPosition =
//            currentRoad.transform.position +
//            currentRoad.transform.right * laneOffset;

//        // Tetap pertahankan posisi maju sesuai arah mobil
//        float forwardOffset = Vector3.Dot(
//            rb.position - currentRoad.transform.position,
//            currentRoad.transform.forward
//        );

//        laneWorldPosition += currentRoad.transform.forward * forwardOffset;

//        targetPosition = new Vector3(
//            laneWorldPosition.x,
//            rb.position.y,
//            laneWorldPosition.z
//        );
//    }

//    private void OnTriggerEnter(Collider other)
//    {
//        SmartRoad road = other.GetComponent<SmartRoad>();

//        if (road != null)
//        {
//            Debug.Log("Masuk Road: " + road.name);

//            currentRoad = road;

//            currentLane = Mathf.Clamp(currentLane, 0, currentRoad.laneCount - 1);

//            UpdateTargetLane();
//        }
//    }

//    private void OnTriggerExit(Collider other)
//    {
//        SmartRoad road = other.GetComponent<SmartRoad>();

//        if (road != null && road == currentRoad)
//        {
//            Debug.Log("Keluar Road");

//            currentRoad = null;
//        }
//    }
//}












//using UnityEngine;

//[RequireComponent(typeof(Rigidbody))]
//public class PlayerMovement : MonoBehaviour
//{
//    [Header("Forward Movement")]
//    public float forwardSpeed = 10f;
//    public bool canControl = true;

//    [Header("Lane Movement")]
//    public float laneChangeSpeed = 10f;

//    private Rigidbody rb;
//    private SmartRoad currentRoad;

//    private int currentLane = 0;
//    private float targetX;

//    private const float laneSnapThreshold = 0.01f;

//    void Start()
//    {
//        rb = GetComponent<Rigidbody>();

//        rb.constraints = RigidbodyConstraints.FreezeRotation;
//        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

//        targetX = transform.position.x;
//    }

//    void Update()
//    {
//        if (!canControl || currentRoad == null)
//            return;

//        // Pindah kiri
//        if (Input.GetKeyDown(KeyCode.A))
//        {
//            if (currentLane > 0)
//            {
//                currentLane--;
//                UpdateTargetLane();
//            }
//        }

//        // Pindah kanan
//        if (Input.GetKeyDown(KeyCode.D))
//        {
//            if (currentLane < currentRoad.laneCount - 1)
//            {
//                currentLane++;
//                UpdateTargetLane();
//            }
//        }
//    }

//    void FixedUpdate()
//    {
//        if (!canControl)
//        {
//            rb.velocity = Vector3.zero;
//            return;
//        }

//        HandleForward();
//        HandleLaneMovement();
//    }

//    void HandleForward()
//    {
//        float moveZ = Input.GetAxis("Vertical") * forwardSpeed;

//        Vector3 velocity = rb.velocity;
//        velocity.z = moveZ;
//        velocity.y = 0;

//        rb.velocity = velocity;
//    }

//    void HandleLaneMovement()
//    {
//        if (currentRoad == null)
//            return;

//        Vector3 position = rb.position;

//        float newX = Mathf.MoveTowards(
//            position.x,
//            targetX,
//            laneChangeSpeed * Time.fixedDeltaTime
//        );

//        if (Mathf.Abs(newX - targetX) < laneSnapThreshold)
//            newX = targetX;

//        rb.MovePosition(new Vector3(newX, position.y, position.z));
//    }

//    void UpdateTargetLane()
//    {
//        if (currentRoad == null)
//            return;

//        float laneOffset = currentRoad.GetLaneOffset(currentLane);

//        // Kunci penting di sini:
//        targetX = currentRoad.transform.position.x + laneOffset;
//    }

//    private void OnTriggerEnter(Collider other)
//    {
//        SmartRoad road = other.GetComponent<SmartRoad>();

//        if (road != null)
//        {
//            Debug.Log("Masuk Road: " + road.name);

//            currentRoad = road;

//            currentLane = Mathf.Clamp(currentLane, 0, currentRoad.laneCount - 1);

//            UpdateTargetLane();
//        }
//    }
//}







//using UnityEngine;

//[RequireComponent(typeof(Rigidbody))]
//public class PlayerMovement : MonoBehaviour
//{
//    [Header("Forward Movement")]
//    public float forwardSpeed = 10f;
//    public bool canControl = true;

//    [Header("Lane Movement")]
//    public float laneChangeSpeed = 10f;

//    private Rigidbody rb;
//    private SmartRoad currentRoad;

//    private int currentLane = 0;
//    private float targetX;

//    private const float laneSnapThreshold = 0.01f;

//    void Start()
//    {
//        rb = GetComponent<Rigidbody>();

//        rb.constraints = RigidbodyConstraints.FreezeRotation;
//        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

//        targetX = transform.position.x;
//    }

//    void Update()
//    {
//        if (!canControl || currentRoad == null)
//            return;

//        // Pindah kiri
//        if (Input.GetKeyDown(KeyCode.A))
//        {
//            if (currentLane > 0)
//            {
//                currentLane--;
//                UpdateTargetLane();
//            }
//        }

//        // Pindah kanan
//        if (Input.GetKeyDown(KeyCode.D))
//        {
//            if (currentLane < currentRoad.laneCount - 1)
//            {
//                currentLane++;
//                UpdateTargetLane();
//            }
//        }
//    }

//    void FixedUpdate()
//    {
//        if (!canControl)
//        {
//            rb.velocity = Vector3.zero;
//            return;
//        }

//        HandleForward();
//        HandleLaneMovement();
//    }

//    void HandleForward()
//    {
//        float moveZ = Input.GetAxis("Vertical") * forwardSpeed;

//        Vector3 velocity = rb.velocity;
//        velocity.z = moveZ;
//        velocity.y = 0;

//        rb.velocity = velocity;
//    }

//    void HandleLaneMovement()
//    {
//        if (currentRoad == null)
//            return;

//        Vector3 position = rb.position;

//        float newX = Mathf.MoveTowards(
//            position.x,
//            targetX,
//            laneChangeSpeed * Time.fixedDeltaTime
//        );

//        if (Mathf.Abs(newX - targetX) < laneSnapThreshold)
//            newX = targetX;

//        rb.MovePosition(new Vector3(newX, position.y, position.z));
//    }

//    void UpdateTargetLane()
//    {
//        if (currentRoad == null)
//            return;

//        targetX = currentRoad.GetLanePositionX(currentLane);
//    }

//    private void OnTriggerEnter(Collider other)
//    {
//        SmartRoad road = other.GetComponent<SmartRoad>();

//        if (road != null)
//        {
//            currentRoad = road;

//            // Pastikan lane valid saat masuk road baru
//            currentLane = Mathf.Clamp(currentLane, 0, currentRoad.laneCount - 1);

//            UpdateTargetLane();
//        }
//    }
//}












//using UnityEngine;

//[RequireComponent(typeof(Rigidbody))]
//public class PlayerMovement : MonoBehaviour
//{
//    [Header("Forward Movement")]
//    public float forwardSpeed = 10f;
//    public bool canControl = true;

//    [Header("Lane Movement")]
//    public float laneChangeSpeed = 10f;

//    private Rigidbody rb;

//    private SmartRoad currentRoad;

//    private int currentLane = 0;
//    private float targetX;
//    private bool isChangingLane = false;

//    void Start()
//    {
//        rb = GetComponent<Rigidbody>();
//        rb.constraints = RigidbodyConstraints.FreezeRotation;
//        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
//    }

//    void FixedUpdate()
//    {
//        if (!canControl)
//        {
//            rb.velocity = Vector3.zero;
//            return;
//        }

//        HandleForward();
//        HandleLaneMovement();
//    }

//    void Update()
//    {
//        if (!canControl || currentRoad == null) return;

//        // Pindah kiri
//        if (Input.GetKeyDown(KeyCode.A))
//        {
//            if (currentLane > 0)
//            {
//                currentLane--;
//                SetTargetLane();
//            }
//        }

//        // Pindah kanan
//        if (Input.GetKeyDown(KeyCode.D))
//        {
//            if (currentLane < currentRoad.laneCount - 1)
//            {
//                currentLane++;
//                SetTargetLane();
//            }
//        }
//    }

//    void HandleForward()
//    {
//        float moveZ = Input.GetAxis("Vertical") * forwardSpeed;
//        rb.velocity = new Vector3(rb.velocity.x, 0, moveZ);
//    }

//    void HandleLaneMovement()
//    {
//        if (currentRoad == null) return;

//        Vector3 position = rb.position;

//        float newX = Mathf.MoveTowards(position.x, targetX, laneChangeSpeed * Time.fixedDeltaTime);

//        rb.MovePosition(new Vector3(newX, position.y, position.z));
//    }

//    void SetTargetLane()
//    {
//        if (currentRoad == null) return;

//        targetX = currentRoad.GetLanePositionX(currentLane);
//    }

//    private void OnTriggerEnter(Collider other)
//    {
//        SmartRoad road = other.GetComponent<SmartRoad>();
//        if (road != null)
//        {
//            currentRoad = road;

//            // Pastikan lane valid kalau masuk road baru
//            currentLane = Mathf.Clamp(currentLane, 0, currentRoad.laneCount - 1);

//            SetTargetLane();
//        }
//        Debug.Log("Masuk Road: " + other.name);
//    }
//}






//[RequireComponent(typeof(Rigidbody))]
//public class PlayerMovement : MonoBehaviour
//{
//    public float speed = 10f;
//    public float strafeSpeed = 7f;
//    public bool canControl = true;

//    private Rigidbody rb;

//    void Start()
//    {
//        rb = GetComponent<Rigidbody>();
//        rb.constraints = RigidbodyConstraints.FreezeRotation;
//        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
//    }

//    void FixedUpdate()
//    {
//        if (!canControl)
//        {
//            rb.velocity = Vector3.zero; // Pastikan mobil berhenti total
//            return;
//        }

//        // Normal control (WASD)
//        float moveX = Input.GetAxis("Horizontal") * strafeSpeed;
//        float moveZ = Input.GetAxis("Vertical") * speed;

//        Vector3 velocity = new Vector3(moveX, 0, moveZ);
//        rb.velocity = velocity;
//    }
//}




//[RequireComponent(typeof(Rigidbody))]
//public class PlayerMovement : MonoBehaviour
//{
//    public float speed = 10f; // kecepatan maju
//    public float strafeSpeed = 7f; // kecepatan geser kiri/kanan

//    private Rigidbody rb;

//    void Start()
//    {
//        rb = GetComponent<Rigidbody>();
//        rb.constraints = RigidbodyConstraints.FreezeRotation;
//        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
//    }

//    void FixedUpdate()
//    {
//        float moveX = Input.GetAxis("Horizontal") * strafeSpeed;
//        float moveZ = Input.GetAxis("Vertical") * speed;

//        // Vector gerakan
//        Vector3 movement = new Vector3(moveX, 0, moveZ) * Time.fixedDeltaTime;

//        // Pindahkan dengan MovePosition (ikut physics)
//        rb.MovePosition(rb.position + movement);
//    }
//}


//using UnityEngine;
//public class PlayerMovement : MonoBehaviour
//{
//    public float speed = 10f;

//    void Update()
//    {
//        float moveX = Input.GetAxis("Horizontal") * speed * Time.deltaTime; // A/D
//        float moveZ = Input.GetAxis("Vertical") * speed * Time.deltaTime;   // W/S

//        // Gerakan geser bebas, tanpa rotasi
//        transform.Translate(new Vector3(moveX, 0, moveZ), Space.World);
//    }
//}

/*
 using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10f;          // Kecepatan maju/mundur
    public float laneDistance = 2f;    // Jarak antar lane (X axis)
    public float laneChangeSpeed = 10f; // Seberapa cepat pindah lane

    private int currentLane = 0;       // -1 = kiri, 0 = tengah, +1 = kanan
    private Vector3 targetPosition;

    void Start()
    {
        targetPosition = transform.position;
    }

    void Update()
    {
        // Gerakan maju/mundur (Z axis)
        float moveZ = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        // Input lane kiri (A) / kanan (D)
        if (Input.GetKeyDown(KeyCode.A) && currentLane > -1) // batas kiri
        {
            currentLane--;
        }
        if (Input.GetKeyDown(KeyCode.D) && currentLane < 1) // batas kanan
        {
            currentLane++;
        }

        // Tentukan posisi target berdasarkan lane
        targetPosition = new Vector3(currentLane * laneDistance, transform.position.y, transform.position.z + moveZ);

        // Smooth bergerak ke target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * laneChangeSpeed);
    }
}

 */

/*
public class PlayerController : MonoBehaviour
{
    public float acceleration = 5f;
    public float maxSpeed = 10f;
    public float deceleration = 5f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 inputDir = new Vector3(moveX, 0, moveZ).normalized;

        if (inputDir.magnitude > 0)
        {
            // percepatan
            rb.velocity = Vector3.MoveTowards(rb.velocity, inputDir * maxSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            // perlambatan (kalau tidak input)
            rb.velocity = Vector3.MoveTowards(rb.velocity, Vector3.zero, deceleration * Time.deltaTime);
        }
    }
} 
*/