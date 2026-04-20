using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Daftar Level (Level_1, Level_2, Level_3)")]
    public GameObject[] levels;

    [Header("Tutorial Manager")]
    public TutorialStepManager tutorial;

    [HideInInspector]
    public GameObject currentFinishTrigger;

    private int currentLevel = 0;

    // =========================
    // AWAKE
    // =========================
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // =========================
    // START
    // =========================
    private void Start()
    {
        ActivateLevel(0);
    }

    // =========================
    // NEXT LEVEL
    // =========================
    public void NextLevel()
    {
        Debug.Log("Current Level sebelum naik: " + currentLevel);

        currentLevel++;

        if (currentLevel >= levels.Length)
        {
            Debug.Log("Semua tutorial selesai!");
            return;
        }

        ActivateLevel(currentLevel);
    }

    // =========================
    // ACTIVATE LEVEL
    // =========================
    void ActivateLevel(int index)
    {
        Debug.Log("Aktifkan Level index: " + index);

        // 1. Matikan semua level
        for (int i = 0; i < levels.Length; i++)
            levels[i].SetActive(false);

        // 2. Aktifkan level yang dipilih
        GameObject activeLevel = levels[index];
        activeLevel.SetActive(true);

        // 3. Ambil FinishTrigger level aktif
        TriggerFinish triggerScript = activeLevel.GetComponentInChildren<TriggerFinish>(true);

        if (triggerScript != null)
        {
            currentFinishTrigger = triggerScript.gameObject;

            // Reset trigger agar siap dipakai
            triggerScript.ResetTrigger();

            // Assign ke tutorial
            if (tutorial != null)
            {
                tutorial.finishTrigger = currentFinishTrigger;
                tutorial.currentStep = 0; // reset step tutorial
            }
        }
        else
        {
            Debug.LogWarning("Trigger_Finish belum ada di " + activeLevel.name);
            currentFinishTrigger = null;
            if (tutorial != null)
                tutorial.finishTrigger = null;
        }

        // 4. Load tutorial sesuai level
        if (tutorial != null)
            tutorial.LoadTutorialForLevel(index + 1);
    }
}


//using UnityEngine;

//public class LevelManager : MonoBehaviour
//{
//    public static LevelManager Instance;

//    [Header("Daftar Level (Level_1, Level_2, Level_3)")]
//    public GameObject[] levels;

//    [Header("Tutorial Manager")]
//    public TutorialStepManager tutorial;

//    [HideInInspector]
//    public GameObject currentFinishTrigger;

//    private int currentLevel = 0;

//    // =========================
//    // AWAKE
//    // =========================
//    private void Awake()
//    {
//        if (Instance == null)
//            Instance = this;
//        else
//            Destroy(gameObject);
//    }

//    // =========================
//    // START
//    // =========================
//    private void Start()
//    {
//        ActivateLevel(0); // mulai dari level pertama
//    }

//    // =========================
//    // NEXT LEVEL
//    // =========================
//    public void NextLevel()
//    {
//        Debug.Log("Current Level sebelum naik: " + currentLevel);

//        currentLevel++;

//        if (currentLevel >= levels.Length)
//        {
//            Debug.Log("Semua tutorial selesai!");
//            return;
//        }

//        ActivateLevel(currentLevel);
//    }

//    // =========================
//    // ACTIVATE LEVEL
//    // =========================
//    void ActivateLevel(int index)
//    {
//        Debug.Log("Aktifkan Level index: " + index);

//        // 1. Matikan semua level
//        for (int i = 0; i < levels.Length; i++)
//            levels[i].SetActive(false);

//        // 2. Aktifkan level yang dipilih
//        GameObject activeLevel = levels[index];
//        activeLevel.SetActive(true);

//        // 3. Ambil FinishTrigger level aktif
//        TriggerFinish triggerScript = activeLevel.GetComponentInChildren<TriggerFinish>(true);
//        if (triggerScript != null)
//        {
//            currentFinishTrigger = triggerScript.gameObject;
//            currentFinishTrigger.SetActive(false); // selalu start mati

//            // assign ke tutorial supaya tahu collider baru
//            if (tutorial != null)
//            {
//                tutorial.finishTrigger = currentFinishTrigger;
//                tutorial.currentStep = 0; // reset step jika perlu
//            }

//            // Optional: reset trigger di TriggerFinish
//            triggerScript.ResetTrigger();
//        }
//        else
//        {
//            Debug.LogWarning("Trigger_Finish belum ada di " + activeLevel.name);
//        }

//        // 4. Load tutorial sesuai level
//        if (tutorial != null)
//            tutorial.LoadTutorialForLevel(index + 1);
//    }
//}





//using UnityEngine;

//public class LevelManager : MonoBehaviour
//{
//    public static LevelManager Instance;

//    [Header("Daftar Level (Level_1, Level_2, Level_3)")]
//    public GameObject[] levels;

//    [Header("Tutorial Manager")]
//    public TutorialStepManager tutorial;

//    [HideInInspector]
//    public GameObject currentFinishTrigger;

//    private int currentLevel = 0;

//    private void Awake()
//    {
//        if (Instance == null)
//            Instance = this;
//        else
//            Destroy(gameObject);
//    }

//    private void Start()
//    {
//        ActivateLevel(0); // mulai dari level 0 (Level_1)
//    }

//    public void NextLevel()
//    {
//        Debug.Log("Current Level sebelum naik: " + currentLevel);

//        currentLevel++;

//        if (currentLevel >= levels.Length)
//        {
//            Debug.Log("Semua tutorial selesai!");
//            return;
//        }

//        ActivateLevel(currentLevel);
//    }

//    void ActivateLevel(int index)
//    {
//        Debug.Log("Aktifkan Level index: " + index);

//        // 1. Matikan semua level
//        for (int i = 0; i < levels.Length; i++)
//            levels[i].SetActive(false);

//        // 2. Aktifkan level yang dipilih
//        GameObject activeLevel = levels[index];
//        activeLevel.SetActive(true);

//        // 2. Ambil FinishTrigger level aktif
//        TriggerFinish triggerScript = activeLevel.GetComponentInChildren<TriggerFinish>(true);
//        if (triggerScript != null)
//        {
//            currentFinishTrigger = triggerScript.gameObject;
//            currentFinishTrigger.SetActive(false); // selalu mulai mati
//            //  assign ke tutorial supaya tahu collider baru
//            if (tutorial != null)
//                tutorial.finishTrigger = currentFinishTrigger;
//        }
//        else
//        {
//            Debug.LogWarning("Trigger_Finish belum ada di " + activeLevel.name);
//        }

//        // 4. Load tutorial sesuai level
//        if (tutorial != null)
//            tutorial.LoadTutorialForLevel(index + 1);
//    }
//}




//using UnityEngine;

//public class LevelManager : MonoBehaviour
//{
//    public static LevelManager Instance;

//    [Header("Daftar Level (Level_1, Level_2, Level_3)")]
//    public GameObject[] levels;

//    [Header("Tutorial Manager")]
//    public TutorialStepManager tutorial;

//    [HideInInspector]
//    public GameObject currentFinishTrigger;

//    private int currentLevel = 0;

//    private void Awake()
//    {
//        if (Instance == null)
//            Instance = this;
//        else
//            Destroy(gameObject);
//    }

//    private void Start()
//    {
//        ActivateLevel(0);
//    }

//    public void NextLevel()
//    {
//        Debug.Log("Current Level sebelum naik: " + currentLevel);

//        currentLevel++;

//        Debug.Log("Current Level sesudah naik: " + currentLevel);

//        if (currentLevel >= levels.Length)
//        {
//            Debug.Log("Semua tutorial selesai!");
//            return;
//        }

//        ActivateLevel(currentLevel);
//    }

//    void ActivateLevel(int index)
//    {
//        Debug.Log("Aktifkan Level index: " + index);

//        // 1. Matikan semua level
//        for (int i = 0; i < levels.Length; i++)
//            levels[i].SetActive(false);

//        // 2. Aktifkan level yang dipilih
//        GameObject activeLevel = levels[index];
//        activeLevel.SetActive(true);

//        // 3. Ambil FinishTrigger berdasarkan script TriggerFinish
//        TriggerFinish triggerScript = activeLevel.GetComponentInChildren<TriggerFinish>(true);

//        if (triggerScript != null)
//        {
//            currentFinishTrigger = triggerScript.gameObject;
//            currentFinishTrigger.SetActive(false); // selalu mulai dalam keadaan mati
//        }
//        else
//        {
//            Debug.LogWarning("Trigger_Finish belum ada di " + activeLevel.name);
//        }

//        // 4. Load tutorial sesuai level
//        tutorial.LoadTutorialForLevel(index + 1);
//    }
//}


//using UnityEngine;

//public class LevelManager : MonoBehaviour
//{
//    public static LevelManager Instance;

//    [Header("Daftar Level (Level_1, Level_2, Level_3)")]
//    public GameObject[] levels;

//    [Header("Tutorial Manager")]
//    public TutorialStepManager tutorial;

//    [HideInInspector]
//    public GameObject currentFinishTrigger;

//    private int currentLevel = 0;

//    private void Awake()
//    {
//        if (Instance == null)
//            Instance = this;
//        else
//            Destroy(gameObject);
//    }

//    private void Start()
//    {
//        ActivateLevel(0);
//    }

//    public void NextLevel()
//    {
//        Debug.Log("Current Level sebelum naik: " + currentLevel);

//        currentLevel++;

//        Debug.Log("Current Level sesudah naik: " + currentLevel);

//        if (currentLevel >= levels.Length)
//        {
//            Debug.Log("Semua tutorial selesai!");
//            return;
//        }

//        ActivateLevel(currentLevel);
//    }

//    void ActivateLevel(int index)
//    {
//        Debug.Log("Aktifkan Level index: " + index);

//        // 1. Matikan semua level
//        for (int i = 0; i < levels.Length; i++)
//            levels[i].SetActive(false);

//        // 2. Aktifkan level yang dipilih
//        GameObject activeLevel = levels[index];
//        activeLevel.SetActive(true);

//        // 3. Ambil FinishTrigger dari level aktif
//        Transform trigger = activeLevel.transform.Find("Trigger_Finish/FinishTrigger");

//        if (trigger != null)
//        {
//            currentFinishTrigger = trigger.gameObject;
//            currentFinishTrigger.SetActive(false); // selalu mulai dalam keadaan mati
//        }
//        else
//        {
//            Debug.LogWarning("Trigger_Finish belum ada di " + activeLevel.name);
//        }

//        // 4. Load tutorial sesuai level
//        tutorial.LoadTutorialForLevel(index + 1);
//    }
//}


//using UnityEngine;

//public class LevelManager : MonoBehaviour
//{
//    public static LevelManager Instance;

//    public GameObject[] levels;           // Level_1, Level_2, Level_3
//    public TutorialStepManager tutorial;

//    private int currentLevel = 0;

//    private void Awake()
//    {
//        if (Instance == null)
//            Instance = this;
//        else
//            Destroy(gameObject);
//    }

//    private void Start()
//    {
//        ActivateLevel(0);
//    }

//    public void NextLevel()
//    {
//        Debug.Log("Current Level sebelum naik: " + currentLevel);
//        currentLevel++;
//        Debug.Log("Current Level sesudah naik: " + currentLevel);

//        if (currentLevel >= levels.Length)
//        {
//            Debug.Log("Semua tutorial selesai!");
//            return;
//        }

//        ActivateLevel(currentLevel);
//    }

//    void ActivateLevel(int index)
//    {
//        Debug.Log("Aktifkan Level index: " + index);

//        // 1 Matikan semua level
//        for (int i = 0; i < levels.Length; i++)
//            levels[i].SetActive(false);

//        // 2 Aktifkan level yang dituju
//        GameObject activeLevel = levels[index];
//        activeLevel.SetActive(true);

//        // 3 Cari Finish Trigger di level itu
//        void ActivateLevel(int index)
//        {
//            Debug.Log("Aktifkan Level index: " + index);

//            for (int i = 0; i < levels.Length; i++)
//                levels[i].SetActive(false);

//            levels[index].SetActive(true);

//            tutorial.LoadTutorialForLevel(index + 1);
//        }

//        // 4 Reset tutorial sesuai level
//        tutorial.LoadTutorialForLevel(index + 1);
//    }
//}
