// Dotnet-XYZ © 2021

using System;

namespace DotnetXYZ.ServiceX.Api
{
	[Serializable]
	public class ModelA
	{
		public Guid Id { get; set; }
		public DateTime Time { get; set; }
		public string Data { get; set; }

		public override bool Equals(object obj)
		{
			var model = obj as ModelA;
			if (model == null)
			{
				return false;
			}
			return (Id == model.Id)
				&& (Time == model.Time)
				&& (Data == model.Data);
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}

		public ModelA Clone()
		{
			return MemberwiseClone() as ModelA;
		}
	}
}