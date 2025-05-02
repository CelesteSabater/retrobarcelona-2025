using UnityEngine;
using UnityEngine.UI;

public class BarraSimetrica : MonoBehaviour
{
    public RectTransform barraPositiva;
    public RectTransform barraNegativa;

    [Range(0f, 1f)]
    public float valor = 0.5f; // 0 = total negativo, 1 = total positivo, 0.5 = centro

    private void Update()
    {
        // Calcular magnitudes relativas al centro (0.5)
        float diferencia = valor - 0.5f;

        if (diferencia > 0)
        {
            barraPositiva.localScale = new Vector3(diferencia * 2f, 1, 1);
            barraNegativa.localScale = new Vector3(0f, 1, 1);
        }
        else if (diferencia < 0)
        {
            barraNegativa.localScale = new Vector3(-diferencia * 2f, 1, 1);
            barraPositiva.localScale = new Vector3(0f, 1, 1);
        }
        else
        {
            // Centro (neutral)
            barraPositiva.localScale = new Vector3(0f, 1, 1);
            barraNegativa.localScale = new Vector3(0f, 1, 1);
        }
    }

    // Métodos para simular acciones
    public void BuenaAccion() => valor = Mathf.Clamp01(valor + 0.05f);
    public void MalaAccion() => valor = Mathf.Clamp01(valor - 0.05f);
}
