using DragonSpark;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Runtime.Activation;
using FluentAssertions;
using LightInject;
using System;
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
			container.Register(new Selector<IFrom>(typeof(Singleton)).Get);
			container.RegisterFallback(CanActivate.Default.Get, Start.A.Selection<ServiceRequest>()
			                                                         .By.Calling(x => x.ServiceType)
			                                                         .Then()
			                                                         .Activate());

			container.GetInstance<IFrom>().Should().BeSameAs(Singleton.Default);
		}

		sealed class Selector<T> : ISelect<IServiceFactory, T>
		{
			readonly Type _type;

			public Selector(Type type) => _type = type;

			public T Get(IServiceFactory parameter) => parameter.GetInstance(_type).To<T>();
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