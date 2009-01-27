﻿using System;
using System.Collections.Generic;
using System.Reflection;
using ServiceStack.Service;

namespace ServiceStack.ServiceInterface
{
	/// <summary>
	/// Base class for resolving handlers decorated with the [Port] attribute.
	/// </summary>
	/// <remarks>
	/// Uses port attributes to find the available ports and what messages they can handle.
	/// </remarks>
	public class PortResolver : IServiceResolver
	{
		private const int DEFAULT_VERSION = default(int);
		private readonly IDictionary<int, IDictionary<string, Type>> handlerCacheByVersion;

		public PortResolver(params Assembly[] serviceInterfaceAssemblies)
		{
			this.OperationTypes = new List<Type>();
			this.handlerCacheByVersion = new Dictionary<int, IDictionary<string, Type>>();

			foreach (var assembly in serviceInterfaceAssemblies)
			{
				foreach (Type typeInAssembly in assembly.GetTypes())
				{
					IDictionary<string, Type> handlerTypeCache;

					var attrs = typeInAssembly.GetCustomAttributes(typeof(PortAttribute), false);

					if (attrs.Length == 0)
						continue;

					var portAttr = (PortAttribute)attrs[0];

					this.OperationTypes.Add(portAttr.OperationType);
					var operationName = portAttr.OperationType.Name;
					var versionNumber = portAttr.Version.GetValueOrDefault(DEFAULT_VERSION);

					if (!this.handlerCacheByVersion.TryGetValue(versionNumber, out handlerTypeCache))
					{
						handlerTypeCache = new Dictionary<string, Type>();
						this.handlerCacheByVersion.Add(versionNumber, handlerTypeCache);
					}

					if (handlerTypeCache.ContainsKey(operationName))
					{
						throw new AmbiguousMatchException(string.Format(
							"A port handler has already been registered for operation '{0}' version '{1}'",
							operationName, versionNumber));
					}
					handlerTypeCache[operationName] = typeInAssembly;
				}
			}
		}

		/// <summary>
		/// Returns a list of operation types available in this service
		/// </summary>
		/// <value>The operation types.</value>
		public IList<Type> OperationTypes
		{
			get; protected set;
		}

		/// <summary>
		/// Finds a service by the service name (i.e. port name).
		/// Always returns the port from the maximum version. 
		/// </summary>
		/// <returns>A new instance of the port</returns>
		public virtual object FindService(string operationName)
		{
			return FindService(operationName, DEFAULT_VERSION);
		}

		/// <summary>
		/// Finds a service by the service name (i.e. handler name) and version number.
		/// </summary>
		/// <returns>A new instance of the handler</returns>
		public virtual object FindService(string operationName, int version)
		{
			IDictionary<string, Type> portCache;

			if (this.handlerCacheByVersion.TryGetValue(version, out portCache))
			{
				Type type;

				return !portCache.TryGetValue(operationName, out type) ? null : Activator.CreateInstance(type);
			}

			return null;
		}
	}
}