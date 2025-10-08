using UnityEngine.Events;

namespace IsolarvHelperTools.Runtime
{
	public class TemplateEvent : UnityEvent
	{
		
	}
	
    public class TemplateEvent<T> : UnityEvent<T>
    {
		
    }

    public class TemplateEvent<T, TS> : UnityEvent<T, TS>
    {
		
    }
}