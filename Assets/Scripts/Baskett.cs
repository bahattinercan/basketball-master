using UnityEngine;
using UnityEngine.SceneManagement;
public class Baskett : MonoBehaviour
{

    [SerializeField]private BasketballController basketballController;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            basketballController.CanMove = false;
            Invoke("LoadScene", 1);
            Debug.Log("will be loaded");
        }         
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}