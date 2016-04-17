// (c) Copyright HutongGames, LLC 2010-2015. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Animator")]
	[Tooltip("Sets look at position and weights. A GameObject can be set to control the look at position, or it can be manually expressed.")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W1071")]
	public class SetAnimatorLookAt: FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(PlayMakerAnimatorIKProxy))]
		[CheckForComponent(typeof(Animator))]
		[Tooltip("The target. An Animator component is required. Use a PlayMakerAnimatorIKProxy component to update the look at during the OnAnimatorIK() update.")]
		public FsmOwnerDefault gameObject;
		
		[Tooltip("The gameObject to look at")]
		public FsmGameObject target;
		
		[Tooltip("The lookat position. If Target GameObject set, targetPosition is used as an offset from Target")]
		public FsmVector3 targetPosition;
		
		[HasFloatSlider(0f,1f)]
		[Tooltip("The global weight of the LookAt, multiplier for other parameters. Range from 0 to 1")]
		public FsmFloat weight;
		
		[HasFloatSlider(0f,1f)]
		[Tooltip("determines how much the body is involved in the LookAt. Range from 0 to 1")]
		public FsmFloat bodyWeight;
		
		[HasFloatSlider(0f,1f)]
		[Tooltip("determines how much the head is involved in the LookAt. Range from 0 to 1")]
		public FsmFloat headWeight;
		
		[HasFloatSlider(0f,1f)]
		[Tooltip("determines how much the eyes are involved in the LookAt. Range from 0 to 1")]
		public FsmFloat eyesWeight;
		
		[HasFloatSlider(0f,1f)]
		[Tooltip("0.0 means the character is completely unrestrained in motion, 1.0 means he's completely clamped (look at becomes impossible), and 0.5 means he'll be able to move on half of the possible range (180 degrees).")]
		public FsmFloat clampWeight;
		
		[Tooltip("Repeat every frame. Useful for changing over time.")]
		public bool everyFrame;
		
		private PlayMakerAnimatorIKProxy _animatorProxy;
		
		private Animator _animator;
		
		private Transform _transform;
		
		public override void Reset()
		{
			gameObject = null;
			target = null;
			targetPosition = new FsmVector3() {UseVariable=true};
			weight = 1f;
			bodyWeight = 0.3f;
			headWeight = 0.6f;
			eyesWeight = 1f;
			clampWeight = 0.5f;
			
			everyFrame = false;
		}
		
		public override void OnEnter()
		{
			// get the animator component
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			
			if (go==null)
			{
				Finish();
				return;
			}
			
			_animator = go.GetComponent<Animator>();
			
			if (_animator==null)
			{
				Finish();
				return;
			}
			
			GameObject _target = target.Value;
			if (_target!=null)
			{
				_transform = _target.transform;
			}
			
			_animatorProxy = go.GetComponent<PlayMakerAnimatorIKProxy>();
			if (_animatorProxy!=null)
			{
				_animatorProxy.OnAnimatorIKEvent += OnAnimatorIKEvent;
			}
			
			
			DoSetLookAt();
			
			if (!everyFrame) 
			{
				Finish();
			}
		}
	
		public void OnAnimatorIKEvent(int layerIndex)
		{
			if (_animatorProxy!=null)
			{
				DoSetLookAt();
			}
		}	
		
		public override void OnUpdate() 
		{
			if (_animatorProxy==null)
			{
				DoSetLookAt();
			}
		}
		
	
		void DoSetLookAt()
		{		
			if (_animator==null)
			{
				return;
			}
			
			if (_transform!=null)
			{
				if (targetPosition.IsNone)
				{
					_animator.SetLookAtPosition(_transform.position);
				}else{
					_animator.SetLookAtPosition(_transform.position+targetPosition.Value);
				}
			}else{
				
				if (!targetPosition.IsNone)
				{
					_animator.SetLookAtPosition(targetPosition.Value);
				}
			}
			
			
			if (!clampWeight.IsNone)
			{
				_animator.SetLookAtWeight(weight.Value,bodyWeight.Value,headWeight.Value,eyesWeight.Value,clampWeight.Value);
			}else if (!eyesWeight.IsNone) 
			{
				_animator.SetLookAtWeight(weight.Value,bodyWeight.Value,headWeight.Value,eyesWeight.Value);
			}else if (!headWeight.IsNone) 
			{
				_animator.SetLookAtWeight(weight.Value,bodyWeight.Value,headWeight.Value);
			}else if (!bodyWeight.IsNone) 
			{
				_animator.SetLookAtWeight(weight.Value,bodyWeight.Value);
			}else if (!weight.IsNone) 
			{
				_animator.SetLookAtWeight(weight.Value);
			}

		
		}
		
		public override void OnExit()
		{
			if (_animatorProxy!=null)
			{
				_animatorProxy.OnAnimatorIKEvent -= OnAnimatorIKEvent;
			}
		}
	}
}