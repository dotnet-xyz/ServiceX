// Dotnet-XYZ © 2021

using DotnetXYZ.ServiceX.Api;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DotnetXYZ.ServiceX.Server
{
	[ApiController]
	[Route("ContractA")]
	public class ContractAController : ControllerBase
	{
		private readonly IContractA _ContractA;

		public ContractAController(IContractA contractA)
		{
			_ContractA = contractA;
		}

		[HttpPost]
		public async Task<ActionResult> CreateAsync([FromBody] ModelA model, CancellationToken ct)
		{
			try
			{
				await _ContractA.CreateAsync(model, ct);
			}
			catch (ContractAIdAlreadyExistsException e)
			{
				return Conflict(e.Message);
			}
			return Ok();
		}

		[HttpGet]
		public async Task<ActionResult<ModelA>> GetAsync([FromQuery] Guid id, CancellationToken ct)
		{
			ModelA model = await _ContractA.GetAsync(id, ct);
			if (model == null)
			{
				return NotFound();
			}
			return Ok(model);
		}

		[HttpPut]
		public async Task<ActionResult> UpdateAsync([FromBody] ModelA model, CancellationToken ct)
		{
			int count = await _ContractA.UpdateAsync(model, ct);
			if (count == 0)
			{
				return NotFound();
			}
			return Ok();
		}

		[HttpDelete]
		public async Task<ActionResult> DeleteAsync([FromQuery] Guid id, CancellationToken ct)
		{
			int count = await _ContractA.DeleteAsync(id, ct);
			if (count == 0)
			{
				return NotFound();
			}
			return Ok();
		}
	}
}