﻿using System;

namespace Investmogilev.Infrastructure.StateMachine
{
	public partial class StateMachine<TState, TTrigger>
	{
		internal class TransitioningTriggerBehaviour : TriggerBehaviour
		{
			private readonly TState _destination;

			public TransitioningTriggerBehaviour(TTrigger trigger, TState destination, Func<bool> guard)
				: base(trigger, guard)
			{
				_destination = destination;
			}

			public override bool ResultsInTransitionFrom(TState source, object[] args, out TState destination)
			{
				destination = _destination;
				return true;
			}
		}
	}
}