using UnityEngine;
using UnityEngine.UI;

public class BarraVida : MonoBehaviour
{
    public Image rellenoBarraVida;
    private CaballeroMovement playerController;
    private float vidaMaxima;

    void Start()
    {
        playerController = GameObject.Find("Caballero").GetComponent<CaballeroMovement>();
        vidaMaxima = playerController.vida;
    }

    private void Update()
    {
        rellenoBarraVida.fillAmount = playerController.vida / vidaMaxima;
    }
}
