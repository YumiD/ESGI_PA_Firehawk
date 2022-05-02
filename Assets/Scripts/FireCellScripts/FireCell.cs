using System.Collections.Generic;
using FireCellScripts;
using UnityEngine;

public enum FireState
{
	None,
	Smoking,
	OnFire
}

public class FireCell : MonoBehaviour
{
	private const float START_SMOKING_TEMPERATURE = 300;
	private const float CATCH_FIRE_TEMPERATURE = 400;

	[SerializeField]
	private float _radius = 0.5f;
	public float Radius => _radius;

	[SerializeField]
	private ParticleSystem _fire;

	[SerializeField]
	private ParticleSystem _smoke;

	[SerializeField]
	private Material _burnedMaterial;

	/// <summary>
	/// This is a list of objects that will recieve a Rigidbody to get detached from their base once this FireCell is fully burned.
	/// </summary>
	[SerializeField]
	private List<GameObject> _objectsToDetach = new List<GameObject>();

	/// <summary>
	/// This is a list of objects that will recieve the "Burned" material once this FireCell is fully burned.
	/// </summary>
	[SerializeField]
	private List<MeshRenderer> _objectsToMakeBurned = new List<MeshRenderer>();
	
	public float Temperature { get; private set; }
	public FireState FireState { get; private set; } = FireState.None;
	public float Fuel { get; private set; } = 100;

	private bool _isBeingHeld;

	private void Start()
	{
		FireManager.Instance.RegisterFireCell(this);
		
		ParticleSystem.ShapeModule fireShape = _fire.shape;
		fireShape.radius = _radius;
		
		ParticleSystem.ShapeModule smokeShape = _smoke.shape;
		smokeShape.radius = _radius;
	}

	private void OnDestroy()
	{
		FireManager.Instance.UnregisterFireCell(this);
	}

	public void DebugSetTemperature(float temperature)
	{
		Temperature = temperature;
	}
	
	public void OnPropagate(FireCell other, float distanceCenterToCenter, float distanceEdgeToEdge)
	{
		if (FireState == FireState.OnFire && distanceEdgeToEdge < 3)
		{
			other.Temperature += 80 * Time.fixedDeltaTime;
		}
	}

	private void OnFireStateChanged()
	{
		switch (FireState)
		{
			case FireState.None:
				_smoke.Stop();
				_fire.Stop();
				break;
			case FireState.Smoking:
				_smoke.Play();
				_fire.Stop();
				if (TryGetComponent(out FireCellBranch branch1))
				{
					branch1.FinishSmoking();
				}
				break;
			case FireState.OnFire:
				_smoke.Stop();
				_fire.Play();
				if (TryGetComponent(out FireCellBranch branch2))
				{
					branch2.BurnBranch();
				}
				break;
		}
	}

	public void HoldByFirehawk()
	{
		_isBeingHeld = !_isBeingHeld;
	}

	private void FixedUpdate()
	{
		if (FireState == FireState.OnFire)
		{
			Fuel -= 10 * Time.fixedDeltaTime;
		}
		else
		{
			Temperature *= 0.999f;
		}
		
		FireState newFireState;
		if ((Temperature >= CATCH_FIRE_TEMPERATURE && Fuel > 0) || _isBeingHeld)
		{
			newFireState = FireState.OnFire;
			gameObject.layer = 12;
		}
		else if (Temperature >= START_SMOKING_TEMPERATURE)
		{
			newFireState = FireState.Smoking;
		}
		else
		{
			newFireState = FireState.None;
		}
		
		if (newFireState != FireState)
		{
			if (newFireState != FireState.OnFire && FireState == FireState.OnFire)
			{
				for (int i = 0; i < _objectsToDetach.Count; i++)
				{
					_objectsToDetach[i].AddComponent<Rigidbody>();
				}

				for (int i = 0; i < _objectsToMakeBurned.Count; i++)
				{
					_objectsToMakeBurned[i].sharedMaterial = _burnedMaterial;
				}
			}
			
			FireState = newFireState;
			OnFireStateChanged();
		}
	}
}