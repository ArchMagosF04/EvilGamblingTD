using UnityEngine;

[CreateAssetMenu(fileName = "Void Event", menuName = "Event Channels/Void Event")]
public class VoidEvent : AbstractEvent<EmptyPayload>
{

}

public struct EmptyPayload { }
