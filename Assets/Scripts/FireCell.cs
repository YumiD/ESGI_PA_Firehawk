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
	private SphereCollider _collider;
	public SphereCollider Collider => _collider;
	
	public float Temperature { get; private set; }
	public FireState FireState { get; private set; } = FireState.None;
	public float Fuel { get; private set; } = 100;

	private void Start()
	{
		FireManager.Instance.RegisterFireCell(this);
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
		// change particles or smth
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