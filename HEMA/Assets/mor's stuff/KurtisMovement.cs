using UnityEngine;
using System;
using System.Collections;

public class KurtisMovement : MonoBehaviour {

	[SerializeField]
	private float m_MoveSpeed = 10;
	[SerializeField]
	private float m_JumpSpeed = 50;
	[SerializeField]
	private float m_AirSlowdownRate = 0.3f;
	private bool m_Grounded = false;

	private Rigidbody2D m_Rigidbody;
	private Animator m_Animator;

	private float m_MovementDirection{
		get{
			return Mathf.Sign(transform.localScale.x);
		}
	}

	private const string ka_RunBoolParam = "Run";
	private const string ka_JumpBoolParam = "Jump";
	private const string ka_FallBoolParam = "Fall";
	private const string k_FloorLayer = "Floor";
	private const string k_PlayerLayer = "Player";


	public Vector3 PlayerVelocity {
		get
		{
			return m_Rigidbody.velocity;
		}
	}


	// Use this for initialization
	void Start () 
	{
		m_Rigidbody = GetComponent<Rigidbody2D>();
		m_Animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		float xMovement = GetXMovement();
		m_Rigidbody.velocity = new Vector3(xMovement, 0);

		HandleGravityStates();

		float yMovement = GetYMovement();
		m_Rigidbody.AddForce(new Vector2(0, yMovement));
	}

	private void HandleGravityStates(){
		//0 Velocity happens in 2 states, max height of jump and landing, we'll check which one is happening
		if(m_Rigidbody.velocity.y == 0){
			//Touching the ground
			if(m_Rigidbody.IsTouchingLayers(1 << LayerMask.NameToLayer(k_FloorLayer))){
				if(!m_Grounded){
					m_Animator.SetBool(ka_JumpBoolParam, false);
					m_Animator.SetBool(ka_FallBoolParam, false);
					m_Grounded = true;
				}
			}
			//Reaching max height
			else{
				m_Animator.SetBool(ka_JumpBoolParam, false);
				m_Animator.SetBool(ka_FallBoolParam, true);
				m_Grounded = false;
			}
		}
		//Otherwise, we're moving in the air, we'll check if it's a landing or jumping move
		else{
			//Falling
			if(m_Rigidbody.velocity.y < 0){
				m_Animator.SetBool(ka_FallBoolParam, true);
			}
			
			m_Grounded = false;
		}
	}

	private float GetYMovement()
	{
		float yMovement = 0;

		if(m_Grounded && Input.GetKeyDown(KeyCode.UpArrow)){
			m_Animator.SetBool(ka_JumpBoolParam, true);
			yMovement = m_JumpSpeed;
		}

		return yMovement;
	}

	private float GetXMovement()
	{
		float xMovement = m_Rigidbody.velocity.x;

		if(m_Rigidbody.velocity.y == 0){
			if(Input.GetKey(KeyCode.RightArrow)){
				transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
				m_Animator.SetBool(ka_RunBoolParam, true);
				xMovement = m_MoveSpeed;
			}
			else if(Input.GetKey(KeyCode.LeftArrow)){
				transform.localScale = new Vector2(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
				m_Animator.SetBool(ka_RunBoolParam, true);
				xMovement = -m_MoveSpeed;
			}
			else{
				m_Animator.SetBool(ka_RunBoolParam, false);
				xMovement = 0;
			}
		}
		else{
			if((!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))){
				if(m_Rigidbody.velocity.x != 0){
					xMovement += m_AirSlowdownRate * -m_MovementDirection;
				}
			}
			else if((Input.GetKey(KeyCode.LeftArrow) && m_MovementDirection == 1) ||
				(Input.GetKey(KeyCode.RightArrow) && m_MovementDirection == -1)){
				xMovement += m_AirSlowdownRate * -m_MovementDirection * 2;
			}
			else if((Input.GetKey(KeyCode.LeftArrow) && m_MovementDirection == -1) ||
			        (Input.GetKey(KeyCode.RightArrow) && m_MovementDirection == 1)){
				if(Mathf.Abs(xMovement) < m_MoveSpeed){
					xMovement += m_AirSlowdownRate * m_MovementDirection * 2;
				}
			}
		}

		return xMovement;
	}
}
