// Dotnet-XYZ © 2021

using System;

namespace DotnetXYZ.ServiceX.Api
{
	public class ContractAIdAlreadyExistsException : ArgumentException
	{
		public ContractAIdAlreadyExistsException(Guid id, Exception inner) :
			base($"Identifier already exists: {id}.", inner)
		{
			Id = id;
		}

		public Guid Id { get; protected set; }
	}

	public class ContractAIdNotFoundException : ArgumentException
	{
		public ContractAIdNotFoundException(Guid id, Exception inner) :
			base($"Identifier not found: {id}.", inner)
		{
			Id = id;
		}

		public Guid Id { get; protected set; }
	}
}