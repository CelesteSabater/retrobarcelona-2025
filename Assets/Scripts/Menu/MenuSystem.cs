using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
public class MenuSystem : MonoBehaviour
{
    // public GameObject playButton;
    public GameObject nivelesPanel;
    public GameObject optionsPanel;
    public GameObject confirmarPanel;

    public Animator animator1;
    public Animator animator2;




    void Start()
    {
        // SelecionMando(playButton);
    }

    public void Jugar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }

    public void Opciones()
    {
        optionsPanel.SetActive(!optionsPanel.activeSelf);

    }

    public void Niveles(GameObject panel)
    {
        animator1.SetBool("Abrir", true);
        animator2.SetBool("Abrir", true);
        panel.SetActive(false);
        nivelesPanel.SetActive(!nivelesPanel.activeSelf);
    }

    public void Confirmar(GameObject panel)
    {
        panel.SetActive(false);
        confirmarPanel.SetActive(!confirmarPanel.activeSelf);
    }

    public void Volver(GameObject panel)
    {
        panel.SetActive(false);
    }

    public void VolverMenu(GameObject panel)
    {
        animator1.SetBool("Abrir", false);
        animator2.SetBool("Abrir", false);
        panel.SetActive(false);
    }

    public void ActivarPanel(GameObject panel)
    {
        panel.SetActive(true);
    }


    // public void SelecionMando(GameObject selectedButton)
    // {
    //     // Debug.Log("Seleccionando el botón: " + selectedButton.name);
    //     EventSystem.current.SetSelectedGameObject(selectedButton);
    // }

    public void Salir()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}
