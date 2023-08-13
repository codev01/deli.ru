using Newtonsoft.Json;

namespace deli.api.Common
{
	public static class Utils
	{
		public static string JsonSerializer(object value, Formatting formatting = Formatting.None)
			=> JsonConvert.SerializeObject(value, formatting);

		public static string CreatePathStr(params string[] paths)
		{
			string _path = string.Empty;
			char backslash = '\\';
			foreach (string path in paths)
				if (!string.IsNullOrEmpty(path))
					_path += path + (path.Last() != backslash ? backslash : string.Empty);
				else
					throw new Exception("path = null or empty");
			return _path;
		}
	}
}
