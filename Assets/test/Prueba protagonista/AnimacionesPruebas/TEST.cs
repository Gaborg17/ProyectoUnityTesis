using UnityEngine;

public class TestAnimator : MonoBehaviour
{
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Esta línea es la única que importa para evitar el error.
        // Si el sistema nuevo está bloqueando el viejo, esto ayuda a que no explote.
        if (UnityEngine.Input.anyKey || UnityEngine.Input.anyKeyDown)
        {
            // Saltar
            if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
            {
                anim.SetTrigger("Jump");
            }

            // Hit
            if (UnityEngine.Input.GetKeyDown(KeyCode.H))
            {
                anim.SetTrigger("Hit");
            }

            // Caminar
            float move = UnityEngine.Input.GetKey(KeyCode.RightArrow) ? 1.0f : 0.0f;
            anim.SetFloat("Speed", move);
        }
    }
}