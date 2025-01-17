using System.IO;
using UnityEngine;

public class EventLogger : MonoBehaviour
{

    public static EventLogger Instance { get; private set; }

    // File path for the log file
    private string filePath;

    // Variables for logging
    private float timeSinceLastLog = 0f;
    public float logInterval = 1f; // Log every 1 second

    // Unique session ID for the log
    private string sessionId;

    private void Awake()
    {        
        // Ensure there's only one instance of the logger
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Make sure Logger persists across scenes
        DontDestroyOnLoad(gameObject);

        // Initialize the logger
        InitializeLogger();

        Debug.Log("EventLogger script is AWAKE and running!");
    }

    private void InitializeLogger()
    {
        sessionId = System.DateTime.Now.ToString("ddMMyyyy_HHmmss");
        filePath = Path.Combine(Application.persistentDataPath, "VRLog_" + sessionId + ".csv");

        string header = "Frame,Timestamp,EventName,SessionID,ElapsedTime\n";
        File.AppendAllText(filePath, header);

        Debug.Log("Logger initialized. Log file path: " + filePath);
    }


    void Start()
    {
    }

    void Update()
    {
    }

    public void LogEvent(string eventName)
    {
        // Log a custom event (e.g., object grabbed)
        WriteLog(eventName);

    }


    public void LogBoxSelect(GameObject box)
    {

        specialBox boolComponent = box.GetComponent<specialBox>();

        if (boolComponent.specialBoxTrue)
        {
            WriteLog($"Correct: {box.name}");
        }
        else
        {
            WriteLog($"Wrong: {box.name}");
        }

        Debug.Log("EventLogger box was pressed!");
    }

    public void LogRandomBox(string boxName)
    {

        WriteLog($"RandomCreated{boxName}");
        Debug.Log("EventLogger randomBox was assigned");
    }



    private void WriteLog(string eventName, Vector3? position = null, Quaternion? rotation = null)
    {
        // Get the frame number and current timestamp
        int frame = Time.frameCount;
        string timestamp = System.DateTime.Now.ToString("HH:mm:ss:fff");
        float elapsedTime = Time.time;

        // Format data into a CSV line
        string line = $"{frame},{timestamp},{eventName},{sessionId},{elapsedTime}\n";

        // Write the line to the CSV file
        File.AppendAllText(filePath, line);
    }

    private void OnDestroy()
    {
        Debug.Log($"Log saved to {filePath}");
    }
}
