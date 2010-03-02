using System;
using System.Collections;
using System.Collections.Generic;

namespace ServiceStack.Text
{
	public static class ReflectionExtensions
	{
		private static readonly Dictionary<Type, object> DefaultValueTypes 
			= new Dictionary<Type, object>();

		public static object GetDefaultValue(Type type)
		{
			if (!type.IsValueType) return null;

			object defaultValue;
			lock (DefaultValueTypes)
			{
				if (!DefaultValueTypes.TryGetValue(type, out defaultValue))
				{
					defaultValue = Activator.CreateInstance(type);
					DefaultValueTypes[type] = defaultValue;
				}
			}

			return defaultValue;
		}

		public static bool IsGenericType(this Type type)
		{
			while (type != null)
			{
				if (type.IsGenericType)
					return true;

				type = type.BaseType;
			}
			return false;
		}

		public static Type GetGenericType(this Type type)
		{
			while (type != null)
			{
				if (type.IsGenericType)
					return type;

				type = type.BaseType;
			}
			return null;
		}

		public static bool IsOrHasInterfaceOf(this Type type, Type interfaceType)
		{
			var genericType = type.GetGenericType();
			var listInterfaces = type.FindInterfaces((t, critera) => t.IsGenericType && t.GetGenericTypeDefinition() == interfaceType, null);
			return listInterfaces.Length > 0 || (genericType != null && genericType.GetGenericTypeDefinition() == interfaceType);
		}

		public static Type GetTypeWithInterfaceOf(this Type type, Type interfaceType)
		{
			if (type == interfaceType) return interfaceType;

			var interfaces = type.FindInterfaces((t, critera) => t == interfaceType, null);

			if (interfaces.Length > 0)
				return interfaces[0];

			return null;
		}

		public static Type GetTypeWithGenericInterfaceOf(this Type type, Type genericInterfaceType)
		{
			var listInterfaces = type.FindInterfaces(
				(t, critera) => t.IsGenericType && t.GetGenericTypeDefinition() == genericInterfaceType, null);

			if (listInterfaces.Length > 0)
				return listInterfaces[0];

			var genericType = type.GetGenericType();
			return genericType.GetGenericTypeDefinition() == genericInterfaceType
					? genericType
					: null;
		}

		public static bool HasAnyTypeDefinitionsOf(this Type genericType, params Type[] theseGenericTypes)
		{
			if (!genericType.IsGenericType) return false;
			var genericTypeDefinition = genericType.GetGenericTypeDefinition();

			foreach (var thisGenericType in theseGenericTypes)
			{
				if (genericTypeDefinition == thisGenericType)
					return true;
			}

			return false;
		}

		public static bool AllHaveInterfacesOfType(
			this Type assignableFromType, params Type[] types)
		{
			foreach (var type in types)
			{
				if (assignableFromType.GetTypeWithInterfaceOf(type) == null) return false;
			}
			return true;
		}

		public static Type[] GetGenericArgumentsIfBothHaveSameGenericDefinitionTypeAndArguments(
			this Type assignableFromType, Type typeA, Type typeB)
		{
			var typeAInterface = typeA.GetTypeWithGenericInterfaceOf(assignableFromType);
			if (typeAInterface == null) return null;

			var typeBInterface = typeB.GetTypeWithGenericInterfaceOf(assignableFromType);
			if (typeBInterface == null) return null;

			var typeAGenericArgs = typeAInterface.GetGenericArguments();
			var typeBGenericArgs = typeBInterface.GetGenericArguments();
			if (typeAGenericArgs.Length != typeBGenericArgs.Length) return null;

			for (var i = 0; i < typeBGenericArgs.Length; i++)
			{
				if (typeAGenericArgs[i] != typeBGenericArgs[i]) return null;
			}

			return typeAGenericArgs;
		}


		internal delegate object CtorDelegate();

		static readonly Dictionary<Type, Func<object>> ConstructorMethods = new Dictionary<Type, Func<object>>();
		public static Func<object> GetConstructorMethod(Type type)
		{
			lock (ConstructorMethods)
			{
				Func<object> ctorFn;
				if (!ConstructorMethods.TryGetValue(type, out ctorFn))
				{
					ctorFn = GetConstructorMethodToCache(type);
					ConstructorMethods[type] = ctorFn;
				}
				return ctorFn;
			}
		}

		public static Func<object> GetConstructorMethodToCache(Type type)
		{
			var dm = new System.Reflection.Emit.DynamicMethod("MyCtor", type, Type.EmptyTypes, typeof(ReflectionExtensions).Module, true);
			var ilgen = dm.GetILGenerator();
			ilgen.Emit(System.Reflection.Emit.OpCodes.Nop);
			ilgen.Emit(System.Reflection.Emit.OpCodes.Newobj, type.GetConstructor(Type.EmptyTypes));
			ilgen.Emit(System.Reflection.Emit.OpCodes.Ret);

			Func<object> ctorFn = ((CtorDelegate)dm.CreateDelegate(typeof(CtorDelegate))).Invoke;
			return ctorFn;
		}

		public static object CreateInstance(Type type)
		{
			try
			{
				var ctorFn = GetConstructorMethod(type);
				return ctorFn();
			}
			catch (Exception ex)
			{
				throw;
			}
		}
	}
}