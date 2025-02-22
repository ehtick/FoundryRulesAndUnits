using FoundryRulesAndUnits.Extensions;

namespace FoundryRulesAndUnits.Models
{
	// public class DO_Promise
	// {
	// 	public string key { get; set; }
	// 	public string url { get; set; }
	// }

	// [System.Serializable]
	// public class DT_Promise
	// {
	// 	public string key;
	// 	public string url;
	// }

	// public class DO_Part
	// {
	// 	public string referenceDesignation { get; set; }
	// 	public string partNumber { get; set; }
	// 	public string serialNumber { get; set; }
	// 	public string revision { get; set; }
	// }
	[System.Serializable]
	public class DT_Part
	{
		public string? title;
		public string? partType;
		public string? promiseReference;
		public string? structureReference;
		public string? referenceDesignation;
		public string? partNumber;
		public string? serialNumber;
		public string? version;

		public DT_Part() 
		{
		}
		
		public bool IsPartType(string type)
		{
			if ( string.IsNullOrEmpty(partType) ) return false;
			return partType.Matches(type);
		}

		public int StructureDepth()
		{
			if ( string.IsNullOrEmpty(structureReference) ) return 0;
			var leg = structureReference ?? "";
			return leg.Split('.').Length;
		}

		public static string ParentReference(string path) 
		{
			if ( string.IsNullOrEmpty(path) ) return "";
			string[] list = path.Split('.');
			string result = string.Join(".", list, 0, list.Length - 1);
			return result;
		}
		public static string ParentName(string path) 
		{
			if ( string.IsNullOrEmpty(path) ) return "";
			string[] list = path.Split('.') ?? new string[0];
			if ( list.Length < 2 ) return "";
			return list[^2];
		}

		public static string Subpath(string path)
		{
			if (string.IsNullOrEmpty(path)) return "";
			string[] array = path.Split('.') ?? new string[0];
			if (array.Length < 2)  return "";
			return string.Join(".", array.Skip(1));
		}

		public static string Parentpath(string path)
		{
			if (string.IsNullOrEmpty(path)) return "";
			string[] array = path.Split('.') ?? new string[0];
			if (array.Length < 2)  return "";
			return string.Join(".", array.Take(array.Length - 1));
		}
		public static string RootName(string path) 
		{
			if ( string.IsNullOrEmpty(path) ) return "";
			string[] list = path.Split('.') ?? new string[0];;
			return list[0];
		}
		public static string SelfName(string path) 
		{
			if ( string.IsNullOrEmpty(path) ) return "";
			string[] list = path.Split('.') ?? new string[0];;
			if ( list.Length < 1 ) return "";
			return list[^1];
		}

		public bool MatchStructure(DT_Part other)
		{
			return structureReference == other.structureReference;
		}

		public bool IsSubStructure(DT_Part parent)
		{
			if (string.IsNullOrEmpty(structureReference)) return false;
			if (string.IsNullOrEmpty(parent.structureReference)) return false;
			return structureReference.StartsWith(parent.structureReference);
		}

		public bool MatchPartNumber(DT_Part other)
		{
			return partNumber == other.partNumber;
		}

		public bool MatchSerialNumber(DT_Part other)
		{
			if (serialNumber != other.serialNumber) return false;
			return MatchPartNumber(other);
		}

		public bool MatchRefDes(DT_Part other)
		{
			return referenceDesignation == other.referenceDesignation;
		}

		public bool MatchVersion(DT_Part other)
		{
			if (version != other.version) return false;
			return MatchPartNumber(other);
		}

		public bool NearMatch(DT_Part other)
		{
			return MatchPartNumber(other) && MatchRefDes(other);
		}

		public bool CompleteMatch(DT_Part other)
		{
			return MatchVersion(other) && MatchRefDes(other);
		}

		public bool IsEmpty()
		{
			if (!string.IsNullOrEmpty(partType)) return false;
			if (!string.IsNullOrEmpty(structureReference)) return false;
			if (!string.IsNullOrEmpty(referenceDesignation)) return false;
			if (!string.IsNullOrEmpty(partNumber)) return false;
			if (!string.IsNullOrEmpty(serialNumber)) return false;
			if (!string.IsNullOrEmpty(version)) return false;
			return true;
		}
		public DT_Part ShallowCopy()
		{
			var result = (DT_Part)this.MemberwiseClone();
			return result;
		}
		public string serialName()
		{
			if (string.IsNullOrEmpty(serialNumber))
				return partName();

			return $"{partName()} (SN:{serialNumber})";
		}
		public string partName()
		{
			if (string.IsNullOrEmpty(version))
				return partNumber ?? "";

			return $"{partNumber}-{version}";
		}

		public string refName()
		{
			if (string.IsNullOrEmpty(referenceDesignation))
				return partName();

			return $"{partName()} ({referenceDesignation})";
		}

		public string PartTitle()
		{
			if (string.IsNullOrEmpty(title))
				return refName();

			return title;
		}

		public string ComputeTitle()
		{
			var result =  $"{title} {partName()}";
						
			if (!string.IsNullOrEmpty(referenceDesignation))
				result = $"{result} ({referenceDesignation})";

			if (!string.IsNullOrEmpty(serialNumber))
				result = $"{result} (SN:{serialNumber})";

			return result;
		}
	}






}