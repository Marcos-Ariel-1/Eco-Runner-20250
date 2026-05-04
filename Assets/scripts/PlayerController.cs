using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float velocidadeFrente = 15f; // Um pouco mais rápido para dar a sensação de Endless Runner
    public float velocidadeLateral = 10f;
    public float distanciaDaTrilha = 2.5f;
    public float forcaPulo = 7f; 

    private int trilhaAtual = 1; // 0 = Esquerda, 1 = Meio, 2 = Direita
    private Rigidbody rb;
    private bool noChao = true; 

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        // A BLINDAGEM: Impede totalmente que ele atravesse o chão em altas velocidades
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    void Update()
    {
        // 1. Controles para trocar de trilha (RF01)
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            trilhaAtual++;
            if (trilhaAtual > 2) trilhaAtual = 2; 
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            trilhaAtual--;
            if (trilhaAtual < 0) trilhaAtual = 0; 
        }

        // 2. Pulo (RF01)
        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && noChao)
        {
            rb.AddForce(Vector3.up * forcaPulo, ForceMode.Impulse);
            noChao = false;
        }
    }

    void FixedUpdate()
    {
        // Descobre onde é o centro da trilha escolhida
        float posicaoAlvoX = 0f;
        if (trilhaAtual == 0) posicaoAlvoX = -distanciaDaTrilha;
        else if (trilhaAtual == 1) posicaoAlvoX = 0;
        else if (trilhaAtual == 2) posicaoAlvoX = distanciaDaTrilha;

        // Calcula a força necessária para puxar ele para a trilha
        float diferencaX = posicaoAlvoX - transform.position.x;

        // 3. MOVIMENTO NATIVO: Usa o motor de física corretamente
        rb.linearVelocity = new Vector3(
            diferencaX * velocidadeLateral, // Puxa para os lados
            rb.linearVelocity.y,                  // Deixa a gravidade e o pulo agirem em paz
            velocidadeFrente                // Corre sempre para frente
        );
    }

    // 4. Detecta quando ele volta a pisar na pista
    void OnCollisionEnter(Collision collision)
    {
        noChao = true; 
    }

    // 5. Detecta o choque com a Mancha Urbana (RF04)
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstaculo")) 
        {
            Debug.Log("💥 GAME OVER! A Mancha Urbana te pegou!");
            
            // Zera a velocidade e congela o tempo
            velocidadeFrente = 0;
            velocidadeLateral = 0;
            Time.timeScale = 0f; 
        }
    }
}