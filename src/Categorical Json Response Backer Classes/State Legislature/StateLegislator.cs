﻿using System;
using Newtonsoft.Json;

namespace cSharpGeocodio
{
	public class StateLegislator
	{
		private StateLegislator(){ }

		[JsonConstructor]
		public StateLegislator(string name, string distrinctNumber)
		{
			this.Name = name;
			this.DistrictNumber = distrinctNumber;
		}

		public string Name { get; set; }
		[JsonProperty("district_number")]
		public string DistrictNumber { get; set; }
	}
}
