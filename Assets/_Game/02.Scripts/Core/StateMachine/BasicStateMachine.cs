using System;
using System.Collections.Generic;

namespace _Game._02.Scripts.Core
{
	/// <summary>
	/// Simple state machine that can be used to separate logic of different states
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class BasicStateMachine<T>  where T : struct, IComparable, IConvertible, IFormattable
	{
		// Whether or not this state machine should trigger events
		public bool TriggerEvents { get; protected set; }
		public T CurrentState { get; protected set; }
		public T PreviousState { get; protected set; }

		public delegate void OnStateChangeDelegate();
		
		// Event you can listen to the changes on this state machine
		public OnStateChangeDelegate OnStateChange;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="triggerEvents">Set to true if you want to trigger events.</param>
		public BasicStateMachine(bool triggerEvents)
		{
			TriggerEvents = triggerEvents;
		} 

		/// <summary>
		/// Changes the current movement state to the one specified in the parameters
		/// </summary>
		/// <param name="newState">New state</param>
		public virtual void ChangeState(T newState)
		{
			// Do nothing if the new state is same as the current one
			if (EqualityComparer<T>.Default.Equals(newState, CurrentState))
			{
				return;
			}

			// Store the previous state
			PreviousState = CurrentState;
			CurrentState = newState;

			// Trigger the event
			if(TriggerEvents)
				OnStateChange?.Invoke();
		}

		/// <summary>
		/// Returns the state machine to the previous state and triggers events if needed
		/// </summary>
		public virtual void RestorePreviousState()
		{
			CurrentState = PreviousState;

			if(TriggerEvents)
				OnStateChange?.Invoke();
			
		}	
	}
}