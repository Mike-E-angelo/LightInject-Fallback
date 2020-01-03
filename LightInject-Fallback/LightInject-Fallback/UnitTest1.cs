using DragonSpark;
using DragonSpark.Compose;
using DragonSpark.Runtime.Activation;
using FluentAssertions;
using LightInject;
using Xunit;

namespace LightInject_Fallback
{
	public class UnitTest1
	{
		[Fact]
		void VerifyFallback()
		{
			var container = new ServiceContainer();
			container.RegisterFallback(CanActivate.Default.Get, Start.A.Selection<ServiceRequest>()
			                                                         .By.Calling(x => x.ServiceType)
			                                                         .Then()
			                                                         .Activate());

			container.GetInstance<Singleton>().Should().BeSameAs(Singleton.Default);

			container.GetInstance<Activated>()
			         .Should()
			         .NotBeNull()
			         .And.Subject.Should()
			         .NotBeSameAs(container.GetInstance<Activated>());
		}

		[Fact]
		void VerifyFallbackOnRegistered()
		{

			var container = new ServiceContainer();
			container.RegisterFallback(CanActivate.Default.Get, Start.A.Selection<ServiceRequest>()
			                                                         .By.Calling(x => x.ServiceType)
			                                                         .Then()
			                                                         .Activate());
			container.Register(typeof(IFrom), typeof(Singleton));

			container.GetInstance<IFrom>().Should().BeSameAs(Singleton.Default);
		}

		public interface IFrom {}

		sealed class Singleton : IFrom
		{
			public static Singleton Default { get; } = new Singleton();

			Singleton() {}
		}

		sealed class Activated {}
	}
}