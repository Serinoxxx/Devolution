using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour {

    public static WaveSpawner instance;

    public Animation helperTextAnim;
    public Text UIHelperText;

    public Text UICountdownText;

    public float timeBetweenWaves = 10f;
    public Wave[] waves;

    private List<GameObject> waveUnits;

    private int waveCount;

    private ArenaSpawnManager _arenaSpawner;

    private void Awake()
    {
        waveUnits = new List<GameObject>();
        instance = this;
    }

    // Use this for initialization
    void Start () {
        _arenaSpawner = GameController.instance.arenaSpawnManager;
        Invoke("SpawnWave", timeBetweenWaves);
        StopCoroutine(Countdown());
        StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        float startTime = Time.time;
        UICountdownText.gameObject.SetActive(true);
        while (Time.time < startTime + timeBetweenWaves)
        {
            int timeRemaining = (int)(timeBetweenWaves - (Time.time - startTime));
            UICountdownText.text = string.Format("Next wave in {0} seconds", timeRemaining);
            yield return null;
        }
        UICountdownText.gameObject.SetActive(false);
    }

    public void OnDeath(GameObject deadUnit)
    {
        waveUnits.Remove(deadUnit);
        if (waveUnits.Count == 0)
        {

            if (waveCount + 1 == waves.Length)
            {
                //string message = "You defeated all the waves! \n Reload from the menu to play again";
                string message = "The elevator is charged \n Proceed to next level";
                UIHelperText.text = message;
                helperTextAnim.Stop();
                helperTextAnim.Play();
            }
            else
            {
                Invoke("SpawnWave", timeBetweenWaves);

                string message = "Round " + ++waveCount + " Complete!";
                UIHelperText.text = message;
                helperTextAnim.Stop();
                helperTextAnim.Play();
                StopCoroutine(Countdown());
                StartCoroutine(Countdown());
            }
        }
    }
	
	void SpawnWave()
    {
        Wave nextWave = waves[waveCount];
        waveUnits = _arenaSpawner.SpawnUnits(nextWave.unitPrefab, nextWave.count, Vector3.zero);
    }


    [System.Serializable]
    public class Wave
    {
        public GameObject unitPrefab;
        public int count;
    }
}
