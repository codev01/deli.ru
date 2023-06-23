using System.Linq.Expressions;

using MongoDB.Bson;

namespace data.deli.ru.MongoDB
{
	internal class MongoHelper
	{
		public int Count { get; private set; } = 0;
		public string Expression { get; private set; }

		private const char DELIMITER = ',';
		private const char NOT_DELIMITER = ' ';
		protected const char _PARAM_PREF = '$';
		protected const string AND = "and",
							OR = "or",
							GTE = "gte",
							LTE = "lte",
							REGEX = "regex",
							OPTIONS = "options";

		protected char Delimiter { get; private set; } = NOT_DELIMITER;
		protected bool IsFirst { get; private set; } = true;

		protected char GetDelimiter(bool iss)
		{
			if (iss)
				return DELIMITER;
			else
				return NOT_DELIMITER;
		}

		protected void PlusCount()
		{
			Count++;
			Delimiter = GetDelimiter(true);
			IsFirst = false;
		}

		protected void AddExpression(string expression)
			=> Expression += expression;

		protected void AddDelimiter()
			=> Expression += DELIMITER;

		protected void CleareExpression()
			=> Expression = string.Empty;

		protected void OpenExpression()
			=> AddExpression("{");

		protected void CloseExpression()
			=> AddExpression("}");

		protected void OpenArray()
			=> AddExpression("[");

		protected void CloseArray()
			=> AddExpression("]");
	}

	internal class MongoExpressionManager : MongoHelper
	{
		public MongoExpressionManager(MongoExpression expression)
			=> Add(expression);
		public MongoExpressionManager(params MongoExpression[] expressions)
			=> Add(expressions);

		public void And(params MongoExpression[] expressions)
			=> _addOperator(AND, expressions);

		public void Or(params MongoExpression[] expressions)
			=> _addOperator(OR, expressions);

		private void _addOperator(string operatorName, params MongoExpression[] expressions)
		{
			AddExpression($"{Delimiter}{_PARAM_PREF}{operatorName}:");

			OpenArray();
			bool isFirst = true;
			foreach (MongoExpression expression in expressions)
			{
				if (!isFirst)
					AddDelimiter();
				OpenExpression();
				AddExpression(expression.Expression);
				CloseExpression();
				isFirst = false;
			}
			CloseArray();

			PlusCount();
		}

		public void Add(MongoExpression expression)
		{
			AddExpression(Delimiter + expression.Expression);
			PlusCount();
		}

		public void Add(params MongoExpression[] expressions)
		{
			foreach (MongoExpression expression in expressions)
				Add(expression);
		}

		public string CreateExpressionString()
		{
			string ex = Expression;
			CleareExpression();
			OpenExpression();
			AddExpression(ex);
			CloseExpression();

			return Expression;
		}

		public static string GetFieldName<T, TValue>(Expression<Func<T, TValue>> memberAccess) =>
			((MemberExpression)memberAccess.Body).Member.Name;
		//var fieldName = GetFieldName((MyClass c) => c.Field);
		//var propertyName = GetFieldName((MyClass c) => c.Property);
	}

	internal class MongoExpression : MongoHelper
	{
		public MongoExpression(string fieldName, params MongoParameter[] parameters)
		{
			_addFieldName(fieldName);
			OpenExpression();
			foreach (MongoParameter parameter in parameters)
			{
				AddExpression($"{Delimiter}{parameter.Name}:{parameter.Value}");
				PlusCount();
			}
			CloseExpression();
		}

		public MongoExpression(string fieldName, MongoParameter parameter)
		{
			_addFieldName(fieldName);
			AddExpression(parameter.Value.ToString());
		}

		private void _addFieldName(string fieldName)
			=> AddExpression($"\"{fieldName}\":");
	}

	internal class MongoParameter : MongoHelper
	{
		public string Name { get; }
		public object Value { get; }
		public MongoParameter(string name, object value)
		{
			Name = name;
			Value = value;
		}

		public static MongoParameter Gte(int value)
			=> Parameter(GTE, value);
		public static MongoParameter Gte(double value)
			=> Parameter(GTE, value);
		public static MongoParameter Gte(decimal value)
			=> Parameter(GTE, value);

		public static MongoParameter Lte(int value)
			=> Parameter(LTE, value);
		public static MongoParameter Lte(double value)
			=> Parameter(LTE, value);
		public static MongoParameter Lte(decimal value)
			=> Parameter(LTE, value);

		public static MongoParameter Regex(string regex)
			=> Parameter(REGEX, regex);
		public static MongoParameter Options(string options)
			=> Parameter(OPTIONS, options);

		public static MongoParameter Parameter(string name, string value)
			=> Parameter(name, (object)_strFormat(value));
		public static MongoParameter Parameter(string name, object value)
			=> new MongoParameter(_PARAM_PREF + name, ValidFormater(value));


		public MongoParameter(object value)
			=> Value = value;

		public static MongoParameter ObjectId(string id)
			=> new MongoParameter($"ObjectId(\"{id}\")");

		private static string _strFormat(string value)
			=> $"\"{value}\"";

		public static string ValidFormater(object value)
		{
			string valueStr = value.ToString();
			string typeName = value.GetType().Name;
			switch (typeName)
			{
				case nameof(String):
					break;
				case nameof(Double):
					return string.Format("{0}", ((double)value).ToString(System.Globalization.CultureInfo.InvariantCulture));
				case nameof(Decimal) or nameof(Decimal128):
					return string.Format("{0}", ((decimal)value).ToString(System.Globalization.CultureInfo.InvariantCulture));
				case nameof(Int16) or nameof(Int32) or nameof(Int64):
					break;
				default:
					throw new TypeAccessException($"Недопустимый тип: {typeName}. Значение: {value}");
			}
			return valueStr;
		}
	}
}
