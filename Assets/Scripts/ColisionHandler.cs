using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ColisionHandler : MonoBehaviour
{
    [SerializeField] private float delay = 5.0f;
    [SerializeField] private int hitFuelDrain = 20;
    [SerializeField] private int fuelRechargeAmount = 40;
    private FuelManager rocketFuelManager;
    private Movement rocketMovement;

    private void Start()
    {
        rocketFuelManager = this.GetComponent<FuelManager>();
        rocketMovement = this.GetComponent<Movement>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Obstacle":
                rocketFuelManager.ChangeFuelLevel(-hitFuelDrain);
                break;
            case "Finish":
                //TODO insert sound, effects etc. for winning
                LoadNextLevel();              
                break;
            default: // In default case (i.e. hitting terrain), we want to destroy the rocket
                rocketFuelManager.ChangeFuelLevel(-rocketFuelManager.MaxFuel);
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch(other.gameObject.tag)
        {
            case "Fuel":
                rocketFuelManager.ChangeFuelLevel(fuelRechargeAmount);
                break;
            default:
                break;
        }
    }

    private void LoadNextLevel()
    {
        int nextLvlIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextLvlIndex >= SceneManager.sceneCountInBuildSettings) nextLvlIndex = 0;
        StartCoroutine(LoadLvlAfterDelay(delay, nextLvlIndex));
        
    }

    IEnumerator LoadLvlAfterDelay(float amount, int index)
    {
        rocketMovement.canMove = false;
        yield return new WaitForSeconds(amount);
        SceneManager.LoadScene(index);
        rocketMovement.canMove = true;
    }
}
