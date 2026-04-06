using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager instance;  // This makes our sound manager REACHABLE from any other script where you need to play a sound
    [SerializeField] private AudioSource soundFXObject;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;  // By doing this you DON'T need to initialize a new component of this manager in any script, you can reach to it directly
            // Without this you'd do: private SoundFXManager soundScript; then do soundScript = GetInstance or something like this
            // NOW YOU CAN: SoundFXManager.instance.myFunction() anywhere in the script

            // BUT !!!!!
            // This works only if you know this sound manager is going to be the ONLY ONE IN THE SCENE EVER
            // so be careful
        }
    }

    public void PlayRandomSoundFXClip(AudioClip[] audioClip, Transform spawnTransform, float volume)
    {
        // Choose a random audioClip
        int rand = Random.Range(0, audioClip.Length);

        // Spawn in the gameObject
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        // Assign the audioClip and volume
        audioSource.clip = audioClip[rand];
        audioSource.volume = volume;

        // Play the sound
        audioSource.Play();

        // Get length of the clip and destroy the prefab after this much time
        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }   
}
