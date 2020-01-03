using DragonSpark.Model.Selection.Conditions;
using System;

namespace DragonSpark.Runtime.Activation
{
	public sealed class CanActivate : Condition<Type>
	{
		public static CanActivate Default { get; } = new CanActivate();

		CanActivate() : base(HasSingletonProperty.Default.Then().Or(HasActivationConstructor.Default)) {}
	}
}