// Dotnet-XYZ © 2021

using DotnetXYZ.ServiceX.Api;
using Microsoft.Extensions.DependencyInjection;

namespace DotnetXYZ.ServiceX.Mock
{
	public static class ServiceCollectionExt
	{
		public static void AddServiceXDataLayerMock(this IServiceCollection sc)
		{
			sc.AddSingleton<IContractADataLayer, ContractADataLayerMock>();
		}
	}
}