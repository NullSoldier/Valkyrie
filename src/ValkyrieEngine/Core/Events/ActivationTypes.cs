namespace Valkyrie.Engine.Events
{
	public enum ActivationTypes
	{
		Activate = 0,
		Movement = 2,
		Collision = 4,
		OnMapEnter = 8,
		Static = 16
	}
}

// Static is used for events that shouldn't be triggered but should still exist on the map
// Such as events where there are only one, or auto ran events