namespace deli.runtime.Bases
{
	public delegate void ContainerHandler(Container_Base container);

	public class Container_Base : IDisposable
	{
		public event ContainerHandler? OnTerminated;

		public string Key { get; }
		public bool IsTerminated { get; private set; } = false;
		/// <summary>
		/// Последнее обращение к серверу
		/// </summary>
		public DateTime LastCall { get; private set; } = DateTime.Now;
		public UInt128 Counter { get; private set; }
		public string Token { get; }

		private const uint _MAX_TICK = 60 * 5;
		private uint _tick = _MAX_TICK;
		private Timer TimerTick { get; }

		public Container_Base(string key, string token)
		{
			Key = key;
			Token = token;

			var autoEvent = new AutoResetEvent(false);
			TimerTick = new Timer(Timer_Tick, autoEvent, 1, 1);
			Update();
		}

		public void Update()
		{
			Counter++;
			LastCall = DateTime.Now;
			if (!IsTerminated && _tick < _MAX_TICK)
				_tick += 5;
		}

		public bool ValidateToken(string token)
			=> token == Token;

		private void Timer_Tick(object? obj)
		{
			_tick--;

			if (_tick == 0)
			{
				IsTerminated = true;
				OnTerminated?.Invoke(this);
			}
		}

		public void Dispose()
			=> TimerTick.Dispose();
	}
}
