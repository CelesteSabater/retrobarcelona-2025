using UnityEngine;
using UnityEngine.UI;

public class BarraSimetrica : MonoBehaviour
{
    // Referencias a las barras positiva y negativa (los RectTransform se usan para modificar el tamaño)
    public RectTransform barraPositiva;
    public RectTransform barraNegativa;

    [Header("Puntuación Karma - Simetría")]
    // Sistema de puntos, se usa para obtener el karma actual
    public SistemaDePuntos sistemaDePuntos;

    // Valor de karma, donde 0.5 es el centro
    [Range(0f, 1f)]
    public float valor = 0.5f;

    private void Update()
    {
        // Si no hay sistema de puntos, no hacer nada
        if (sistemaDePuntos == null) return;

        // Obtiene el karma actual y lo mapea en un rango de 0 a 1 (desde -100 hasta 100)
        valor = Mathf.InverseLerp(-100f, 100f, sistemaDePuntos.GetKarma());

        // Calcula la diferencia desde el centro (0.5)
        float diferencia = valor - 0.5f;

        // Si el valor es mayor que 0.5, ajustamos la barra positiva
        if (valor > 0.5f)
        {
            // La barra positiva se escala en función de cuanto más grande es el valor
            barraPositiva.localScale = new Vector3((valor - 0.5f) * 2f, 1f, 1f);
            // La barra negativa se oculta
            barraNegativa.localScale = new Vector3(0f, 1f, 1f);
        }
        // Si el valor es menor que 0.5, ajustamos la barra negativa
        else if (valor < 0.5f)
        {
            // La barra negativa se escala en función de cuanto más pequeño es el valor
            barraNegativa.localScale = new Vector3((0.5f - valor) * 2f, 1f, 1f);
            // La barra positiva se oculta
            barraPositiva.localScale = new Vector3(0f, 1f, 1f);
        }
        // Si el valor es exactamente 0.5, ambas barras se ocultan
        else
        {
            barraPositiva.localScale = new Vector3(0f, 1f, 1f);
            barraNegativa.localScale = new Vector3(0f, 1f, 1f);
        }
    }

    // Métodos para cambiar el valor de karma (Honor y Odio)
    public void BarraHonor() => valor = Mathf.Clamp01(valor + 0.05f);  // Aumenta el valor del karma
    public void BarraOdio() => valor = Mathf.Clamp01(valor - 0.05f);   // Disminuye el valor del karma
}
