﻿namespace deli.data.Parameters
{
	public class Parameter<T> where T : new()
	{
		public static T Default { get; } = new T();
	}
}