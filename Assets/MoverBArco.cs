using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MoverBArco : MonoBehaviour
{
    [SerializeField]private Transform barco;
    [SerializeField] private float velocidad;//Velocidad a la que se va a mover el barco

    public Transform destino { get; set; }//Lugar al que se va a mover le barco

    public string islaActual;
    

    [SerializeField] private Camera cam;
    [SerializeField] private bool isMoving = false;

    void Start()
    {
        barco.position = GameManager.Instance.boatPosition;
        barco.rotation = GameManager.Instance.boatRotation;
    }

    
    void Update()
    {
        if (InputManager.Instance.LeftMouseClicked() && isMoving == false)
        {
            Clicked();
        }
    }

    private void Clicked()
    {
        
        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        if(Physics.Raycast(ray, out RaycastHit hit))
        {
            IClickable clickable = hit.collider.GetComponent<IClickable>();
            if (clickable != null)
            {
                
                clickable.OnClick();
                islaActual = destino.parent.gameObject.name;
                GameManager.Instance.islaActual = islaActual;
                StartCoroutine(MoverAIsla());
            }
        }
    }

    public IEnumerator MoverAIsla()
    {
        while(Vector3.Distance(barco.position, destino.position) > 0.01f)
        {
            isMoving = true;
            barco.LookAt(destino);
            barco.position = Vector3.MoveTowards(barco.position, destino.position, velocidad * Time.deltaTime);
            yield return null;
        }
        barco.position = destino.position;
        GameManager.Instance.boatPosition = destino.position;
        barco.rotation = destino.rotation;
        GameManager.Instance.boatRotation = destino.rotation;
        
        yield return new WaitForSeconds(.3f);
        isMoving = false;
        SceneManager.LoadScene("Islas");
    }


}
