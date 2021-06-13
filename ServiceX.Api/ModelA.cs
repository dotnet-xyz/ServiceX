// Dotnet-XYZ © 2021

using System;

namespace DotnetXYZ.ServiceX.Api
{
	[Serializable]
	public class ModelA
	{
		public Guid Id;
		public DateTime Time;
		public String Data;

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
			return this.MemberwiseClone() as ModelA;
		}
	}
}