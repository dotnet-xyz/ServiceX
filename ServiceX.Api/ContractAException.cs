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
}