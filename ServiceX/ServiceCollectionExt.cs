// Dotnet-XYZ © 2021

using DotnetXYZ.ServiceX.Api;
using Microsoft.Extensions.DependencyInjection;

namespace DotnetXYZ.ServiceX
{
	public static class ServiceCollectionExt
	{
		public static void AddServiceX(this IServiceCollection sc)
		{
			sc.AddSingleton<IContractA, ContractA>();
		}
	}
}