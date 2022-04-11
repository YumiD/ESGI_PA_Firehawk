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
	
	public float Temperature { get; private set; }
	public FireState FireState { get; private set; } = FireState.None;
	public float Fuel { get; private set; } = 100;

	private void Start()
	{
		FireManager.Instance.RegisterFireCell(this);
		
		ParticleSystem.ShapeModule fireShape = _fire.shape;
		fireShape.radius = _radius;
		
		ParticleSystem.ShapeModule smokeShape = _smoke.shape;
		smokeShape.radius = _radius;
	}

	public void DebugSetTemperature(float temperature)
	{
		Temperature = temperature;
	}
	
	public void OnPropagate(FireCell other, float distanceCenterToCenter, float distanceEdgeToEdge)
	{
		if (FireState == FireState.OnFire && distanceEdgeToEdge < 2)
		{
			other.Temperature += 100 * Time.fixedDeltaTime;
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
				break;
			case FireState.OnFire:
				_smoke.Stop();
				_fire.Play();
				break;
		}
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
		if (Temperature >= CATCH_FIRE_TEMPERATURE && Fuel > 0)
		{
			newFireState = FireState.OnFire;
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
			FireState = newFireState;
			OnFireStateChanged();
		}
	}
}