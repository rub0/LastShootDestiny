using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour {

    
    public float _maxVelocity = 1.0f;
    private float _acceleration = 0.3f;
    public InputManager input;
    public Vector3 _momentum;
    public float _gravity = 0.1f;
    private bool _isGrounded; // Significa esta castigado <- JAJAJAJAJA
    private bool _isTouchingRoof = false;
    public float _jumpForce = 2.0f;
    private bool _canJump = true;
    private bool _jumped = false;
    private Vector3 _externalForce = Vector3.zero;
    public float _externalForceTime = 0.2f;
    private float _stunTime = 0.0f;
    private bool _stunned = false;
    public float _timejump = 2.0f;
    public float _heightjump = 2.0f;

    private float _slowTime = 0.0f;
    private float _slowStrength = 1.0f;

    private Transform _transform;

    public CharacterController controller;
    private PlayerState state;

    private float _velFriction;

    public Transform _cam;

	// Use this for initialization
	void Start ()
    {
        if ((Network.peerType == NetworkPeerType.Client && !networkView.isMine) || Network.peerType == NetworkPeerType.Server)
        {
            Destroy(this);
            return;
        }
        // @todo
        // Assign manager by callback. Note that the manager
        // has to implement IMovementCommands interface for this
        // to work as expected.

        GameObject.FindGameObjectsWithTag("MainCamera")[0].GetComponent<CameraBound>().setPlayer(this.gameObject);

        if(!input)
            input = GetComponent<InputManager>();
        _transform = transform;
        _momentum = Vector3.zero;
        _isGrounded = true;

        controller = GetComponent<CharacterController>();
        state = GetComponent<PlayerState>();
        _timejump *= 1000;
        float gravity = (2.0f * _heightjump) / ( _timejump * _timejump );

        float force = _heightjump / _timejump;

        //Debug.Log(_gravity*Time.fixedDeltaTime);


        _velFriction = _maxVelocity / (_maxVelocity + (0.5f * _acceleration * Time.fixedDeltaTime * 100));
        //Debug.Log("termino");
	}
    void OnDestroy()
    {
        //Debug.Log("me destruyen");
    }


    void Update()
    {
        if (!_canJump && input.getButtonJumpUp())
            _canJump = true;
    }
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        // Inherited from an interface in case we want to do
        // more stuff. If it ends up being just one method, we can
        // simply implement it as a callback.
        Vector3 inputmovement = Vector3.zero;
        inputmovement = input.updateMovement();
        if (inputmovement != Vector3.zero)
        {
            _momentum.x += inputmovement.x * _acceleration;
            //Debug.Log(_momentum);
        }
        if (inputmovement.y > 0)
            jump();
        _momentum.y -= _gravity;
        _momentum.x *= _velFriction * ((_isGrounded) ? Mathf.Abs(inputmovement.x) : 1);
        

        Vector3 oldPos = _transform.position;
        updateKinematicPhysics( controller.Move(_momentum*Time.fixedDeltaTime) );
	}

    void jump()
    {
        if (_isGrounded && _canJump)
        {
            //comprobamos si tiene una tile encima
            Ray ray = new Ray(_transform.position, Vector3.up);
            RaycastHit hit;
            int layerMask = 1 << LayerMask.NameToLayer("Wall") | 1 << LayerMask.NameToLayer("ImpenetrableWall");

            if (Physics.Raycast(ray, out hit, 1.0f, layerMask))
                return;

            _momentum.y += _jumpForce;
            _isGrounded = false;
            _canJump = false;
        }
    }

    public Vector3 getVelocity()
    {
        return _momentum;
    }

    public void AddExternalForce(Vector3 force, float maxForce = 9999.0f)
    {
        if (!_isGrounded)
            _momentum.y = 0.0f;

        _momentum += force;

        if (_momentum.y > maxForce)
            _momentum.y = maxForce;
        
        _isGrounded = false;
    }

    public void stun(float time = 1.5f)
    {
        _stunned = true;
        _stunTime = time;
    }

    public void slow(float strength = 0.2f, float time = 2.0f) 
    {
        _slowStrength = strength;
        _slowTime = time;
    }

    private void dead() {
        _momentum = Vector3.zero;
        enabled = false;
        collider.enabled = false;
    }

    private void updateKinematicPhysics(CollisionFlags flags)
    {

        if ((flags & CollisionFlags.Below) != 0)
        {
            _isGrounded = true;
            //Debug.Log("collisiondown");
            _jumped = false;
            _momentum.y = 0f;
            //_transform.position = new Vector3(_transform.position.x, Mathf.Round(_transform.position.y), _transform.position.z);
        }
        else
            _isGrounded = false;

        if ((flags & CollisionFlags.Above) != 0)
        {
            _momentum.y = 0f;
            _isTouchingRoof = true;
        }
        else
            _isTouchingRoof = false;
        if (((flags & CollisionFlags.Sides)) != 0)
        {
            _momentum.x = 0f;
        }
    }

    public Vector3 getMomentum()
    {
        return _momentum;
    }

    public bool isGrounded()
    {
        return _isGrounded;
    }
}
