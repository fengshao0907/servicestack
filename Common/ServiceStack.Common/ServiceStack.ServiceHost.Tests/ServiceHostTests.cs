﻿using System;
using Funq;
using NUnit.Framework;
using ServiceStack.ServiceHost.Tests.Support;
using ServiceStack.ServiceHost.Tests.TypeFactory;

namespace ServiceStack.ServiceHost.Tests
{
	[TestFixture]
	public class ServiceHostTests
	{
	
		[Test]
		public void Can_execute_BasicService()
		{
			var serviceController = new ServiceController();

			serviceController.Register(() => new BasicService());
			var result = serviceController.Execute(new BasicRequest()) as BasicRequestResponse;

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		public void Can_execute_BasicService_from_dynamic_Type()
		{
			var requestType = typeof(BasicRequest);

			var serviceController = new ServiceController();
			serviceController.Register(requestType, typeof(BasicService));

			object request = Activator.CreateInstance(requestType);

			var result = serviceController.Execute(request) as BasicRequestResponse;

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		public void Can_AutoWire_types_dynamically_with_reflection()
		{
			var serviceType = typeof(AutoWireService);

			var container = new Container();
			container.Register<IFoo>(c => new Foo());
			container.Register<IBar>(c => new Bar());

			var typeContainer = new ReflectionTypeFunqContainer(container);
			typeContainer.Register(serviceType);

			var service = container.Resolve<AutoWireService>();

			Assert.That(service.Foo, Is.Not.Null);
			Assert.That(service.Bar, Is.Not.Null);
		}

		[Test]
		public void Can_AutoWire_types_dynamically_with_expressions()
		{
			var serviceType = typeof(AutoWireService);

			var container = new Container();
			container.Register<IFoo>(c => new Foo());
			container.Register<IBar>(c => new Bar());

			var typeContainer = new ExpressionTypeFunqContainer(container);
			typeContainer.RegisterTypes(serviceType);

			var service = container.Resolve<AutoWireService>();

			Assert.That(service.Foo, Is.Not.Null);
			Assert.That(service.Bar, Is.Not.Null);
		}

		[Test]
		public void Can_execute_RestTestService()
		{
			var serviceController = new ServiceController();

			serviceController.Register(() => new RestTestService());
			var result = serviceController.Execute(new RestTest()) as RestTestResponse;

			Assert.That(result, Is.Not.Null);
			Assert.That(result.MethodName, Is.EqualTo("Execute"));
		}

		[Test]
		public void Can_RestTestService_GET()
		{
			var serviceController = new ServiceController();

			serviceController.Register(() => new RestTestService());
			var result = serviceController.Execute(new RestTest(),
				new HttpRequestContext(null, EndpointAttributes.HttpGet)) as RestTestResponse;

			Assert.That(result, Is.Not.Null);
			Assert.That(result.MethodName, Is.EqualTo("Get"));
		}

		[Test]
		public void Can_RestTestService_PUT()
		{
			var serviceController = new ServiceController();

			serviceController.Register(() => new RestTestService());
			var result = serviceController.Execute(new RestTest(),
				new HttpRequestContext(null, EndpointAttributes.HttpPut)) as RestTestResponse;

			Assert.That(result, Is.Not.Null);
			Assert.That(result.MethodName, Is.EqualTo("Put"));
		}

		[Test]
		public void Can_RestTestService_POST()
		{
			var serviceController = new ServiceController();

			serviceController.Register(() => new RestTestService());
			var result = serviceController.Execute(new RestTest(),
				new HttpRequestContext(null, EndpointAttributes.HttpPost)) as RestTestResponse;

			Assert.That(result, Is.Not.Null);
			Assert.That(result.MethodName, Is.EqualTo("Post"));
		}

		[Test]
		public void Can_RestTestService_DELETE()
		{
			var serviceController = new ServiceController();

			serviceController.Register(() => new RestTestService());
			var result = serviceController.Execute(new RestTest(),
				new HttpRequestContext(null, EndpointAttributes.HttpDelete)) as RestTestResponse;

			Assert.That(result, Is.Not.Null);
			Assert.That(result.MethodName, Is.EqualTo("Delete"));
		}
	}
}
