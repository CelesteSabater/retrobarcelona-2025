using UnityEngine;
using UnityEngine.UI;

public class BarraSimetrica : MonoBehaviour
{
    public RectTransform barraPositiva; // Azul (derecha)
    public RectTransform barraNegativa; // Roja (izquierda)

    [Header("Puntuación Karma - Simetría")]
    public SistemaDePuntos sistemaDePuntos;

    [Range(0f, 1f)]
    public float valor = 0.5f;

    private void Update()
    {
        if (sistemaDePuntos == null) return;

        // Normaliza el karma entre -100 y 100 a un valor entre 0 y 1
        valor = Mathf.InverseLerp(-100f, 100f, sistemaDePuntos.GetKarma());

        float diferencia = valor - 0.5f;

        if (diferencia > 0)
        {
            // Karma positivo → escalar barra azul hacia la derecha
            barraPositiva.localScale = new Vector3(diferencia * 2f, 1f, 1f);
            barraNegativa.localScale = new Vector3(0f, 1f, 1f);
        }
        else if (diferencia < 0)
        {
            // Karma negativo → escalar barra roja hacia la izquierda
            barraNegativa.localScale = new Vector3(diferencia * 2f, 1f, 1f);
            barraPositiva.localScale = new Vector3(0f, 1f, 1f);
        }
        else
        {
            // Neutral
            barraPositiva.localScale = new Vector3(0f, 1f, 1f);
            barraNegativa.localScale = new Vector3(0f, 1f, 1f);
        }
    }

    public void BuenaAccion() => valor = Mathf.Clamp01(valor + 0.05f);
    public void MalaAccion() => valor = Mathf.Clamp01(valor - 0.05f);
}
