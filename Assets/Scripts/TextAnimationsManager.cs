using UnityEngine;
using TMPro;

public class TextAnimationsManager : MonoBehaviour
{
    private Timer latencyTimer = new Timer();
    private Timer cpuTimer = new Timer();
    private Timer ramTimer = new Timer();
    private Timer speedTimer = new Timer();
    [SerializeField] private TMP_Text latencyText;
    [SerializeField] private TMP_Text cpuLoadText;
    [SerializeField] private TMP_Text ramUsageText;
    [SerializeField] private TMP_Text speedText;
    [SerializeField] private TMP_Text commandLineText;

    private string[] successCommands = {
        "> Handshake protocol synchronized: OFFSET_0x12", 
        "> Cracking RSA-4096 entropy... BIT_STREAMS_ALIGNED", 
        "> Packet sniffer active on VLAN 12", 
        "> Spoofing MAC address [DE:AD:BE:EF:CA:FE]", 
        "> Rootkit deployment: 100% ... HIDDEN", 
        "> Bruteforce dictionary loaded: 1.2M entries", 
        "> Tunneling through TOR entry node: 192.16.0.1", 
        "> Overriding administrative lockout... BYPASSED"
    };
    private string[] failCommands = {
        "> ALERT: Intrusion Detection System (IDS) triggered", 
        "> FATAL: Stack smashing detected at 0x008A2B",
        "> Connection reset by peer [REMOTE_REFUSAL]",
        "> Segmentation fault: core dumped",
        "> Warning: Kernel panic imminent in Sector 3",
        "> Trace progress: 85% ... LOG_ANALYSER_DETECTION",
        "> ERROR: Invalid checksum for packet #402",
        "> Access Denied: Biometric signature mismatch",
        "> Emergency lockdown initiated by SYSTEM_ROOT"
    };

    public void Start()
    {
        // Set up the latency text timer
        latencyTimer.Start(1f);
    }

    public void Update()
    {
        // Changing the latency text, speed and any small animation where random numbers change over time
        RandomNumbersAnimation(latencyText, latencyTimer, 1f, 18, 27, "LATENCY:", "ms");
        RandomNumbersAnimation(cpuLoadText, cpuTimer, 1.5f, 83, 89, "cpu load:", "%");
        // RandomNumbersAnimation(ramUsageText, ramTimer, 2f, 12.0, 12.6, "ram usage:", "GB");
        RandomNumbersAnimation(speedText, speedTimer, 1f, 38, 66, "speed:", "mbps");
    }

    public void updateCommandLine(string outcome)
    {
        if (outcome == "success")
        {
            int randMessage = UnityEngine.Random.Range(0, successCommands.Length);
            commandLineText.text += successCommands[randMessage] + "\n";
        }
        else
        {
            int randMessage = UnityEngine.Random.Range(0, failCommands.Length);
            commandLineText.text += failCommands[randMessage] + "\n";
        }
    }

    private void RandomNumbersAnimation(TMP_Text textObject, Timer timer, float time, int minVal, int maxVal, string text, string measure)
    {
        timer.Tick(Time.deltaTime);  // Calling this EVERY UPDATE to reduce the timer
        if (timer.IsFinished())  // Changing the text only when the timer FINISHED
        {
            // Flick the mbps text
            int latencyValue = UnityEngine.Random.Range(minVal, maxVal);
            textObject.text = $"{text} {latencyValue}{measure}";
            
            // Calling the timer once again for infinite flickering of the number
            timer.Start(time);
        }
    }
}
