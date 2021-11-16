using Controle.Gamedes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InimigoIA : MonoBehaviour
{
    [SerializeField]
    private float _velocidade = 20f;
    [SerializeField]
    private LayerMask _layerPermitida;
    [SerializeField]
    private Vector2 _raycastOffset;

    #region Public Variables
    private Rigidbody2D _rb;
    private Controle2D _controle;
    private int _andandoParaDireita;
    private float _movimentoHorizontal;
    public Transform rayCast;
    public LayerMask rayCastMask;
    public float rayCastLength;
    public float attackDistance;
    public float moveSpeed;
    public Transform leftLimit;
    public Transform rightLimit;
    #endregion

    #region Private Variables
    private RaycastHit2D hit;
    private Transform target;
    private float distance;
    private bool attackMode;
    private bool inRange;
    #endregion

    private void OnEnable()
    {
        SelectTarget();
        _rb = GetComponent<Rigidbody2D>();
        _controle = GetComponent<Controle2D>();
        _andandoParaDireita = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (!attackMode)
        {
            Move();
        }
        if(!InsideofLimits() && !inRange)
        {
            SelectTarget();
        }
        
        if(inRange == true)
        {
            hit = Physics2D.Raycast(rayCast.position, transform.right, rayCastLength, rayCastMask);            
            RaycastDebugger();
        }

        if (hit.collider != null){
            EnemyLogic();
        }
        else if(hit.collider == null)
        {
            inRange = false;
        }
        if(inRange == false)
        {

        }
        /*_movimentoHorizontal = _andandoParaDireita * _velocidade;
        ColisaoRaycastParede();
        ColisaoRaycastChao();*/
    }

    void FixedUpdate()
    {
       // _controle.Movimento(_movimentoHorizontal * Time.fixedDeltaTime, false);
    }

    /*void ColisaoRaycastParede()
    {
        var raycastParedeDireita = Physics2D.Raycast(new Vector2(transform.position.x + _raycastOffset.x, transform.position.y + _raycastOffset.y), Vector2.right, 0.5f, _layerPermitida);
        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + _raycastOffset.y), Vector2.right, Color.blue);

        if (raycastParedeDireita.collider != null)
        {
            _andandoParaDireita = -1;
        }

        var raycastParedeEsquerda = Physics2D.Raycast(new Vector2(transform.position.x - _raycastOffset.x, transform.position.y + _raycastOffset.y), Vector2.left, 0.5f, _layerPermitida);
        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + _raycastOffset.y), Vector2.left, Color.blue);

        if (raycastParedeEsquerda.collider != null)
        {
            _andandoParaDireita = 1;
        }
    }

    void ColisaoRaycastChao()
    {
        var raycastChaoDireita = Physics2D.Raycast(new Vector2(transform.position.x + _raycastOffset.x, transform.position.y), Vector2.down, 1f, _layerPermitida);
        Debug.DrawRay(new Vector2(transform.position.x + _raycastOffset.x, transform.position.y), Vector2.down, Color.red);

        if (raycastChaoDireita.collider == null)
        {
            _andandoParaDireita = -1;
        }

        var raycastChaoEsquerda = Physics2D.Raycast(new Vector2(transform.position.x - _raycastOffset.x, transform.position.y), Vector2.down, 1f, _layerPermitida);
        Debug.DrawRay(new Vector2(transform.position.x - _raycastOffset.x, transform.position.y), Vector2.down, Color.red);
        if (raycastChaoEsquerda.collider == null)
        {
            _andandoParaDireita = 1;
        }
    } */

    void OnTriggerEnter2D(Collider2D collision) //Detecta a colisão do raycast com o player;
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            target = collision.transform;
            inRange = true;
            Flip();
        }
    }

    void RaycastDebugger()
    {
        if(distance > attackDistance)
        {
            Debug.DrawRay(rayCast.position, transform.right * rayCastLength, Color.red);
        }
        else if(attackDistance > distance)
        {
            Debug.DrawRay(rayCast.position, transform.right * rayCastLength, Color.green);
        }
    }

    void EnemyLogic() //Lógica do inimigo.
    {
        distance = Vector2.Distance(transform.position, target.position);
        if(distance > attackDistance)
        {
            Move();            
        }
    }

    void Move() //Função básica de movimentação do inimigo.
    {
        Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    private bool InsideofLimits() //Determina se o inimigo está dentro da "zona de patrulha".
    {
        return transform.position.x > leftLimit.position.x && transform.position.x < rightLimit.position.x;
    }

    private void SelectTarget() //Seleciona o Limite mais distante, caso o inimigo deixe a "zona de patrulha". 
    {
        float distanceToLeft = Vector2.Distance(transform.position, leftLimit.position);
        float distanceToRight = Vector2.Distance(transform.position, rightLimit.position);

        //target = distanceToLeft > distanceToRight ? target : leftLimit;
        //target = distanceToLeft < distanceToRight ? target : rightLimit;

        if (distanceToLeft > distanceToRight)
        {
            target = leftLimit;
        }
        else
        {
            target = rightLimit;
        }

        Flip();
    }
    private void Flip()
    {
        Vector3 rotation = transform.eulerAngles;
        if (transform.position.x > target.position.x)
            rotation.y = 180f;
        else
            rotation.y = 0f;

        transform.eulerAngles = rotation;
    }
}
