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

    private Rigidbody2D _rb;
    private Controle2D _controle;
    private int _andandoParaDireita;
    private float _movimentoHorizontal;

    private void OnEnable()
    {
        _rb = GetComponent<Rigidbody2D>();
        _controle = GetComponent<Controle2D>();
        _andandoParaDireita = 1;
    }

    // Update is called once per frame
    void Update()
    {
        _movimentoHorizontal = _andandoParaDireita * _velocidade;
        ColisaoRaycastParede();
        ColisaoRaycastChao();
    }

    void FixedUpdate()
    {
        _controle.Movimento(_movimentoHorizontal * Time.fixedDeltaTime, false);
    }

    void ColisaoRaycastParede()
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
    }
}
