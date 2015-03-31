using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using SQLite.Net.Attributes;
using SQLite.Net.Interop;
using SQLite.Net;

namespace TabataPCL
{
	[Table ("tabata")]
	public class Tabata
	{
		int _currentWorkSecond;
		int _currentRestSecond;
		int _currentSet;

		#region Properties

		[PrimaryKey, AutoIncrement]
		public int TabataID {
			get;
			set;
		}

		[Column ("rest")]
		public int RestInterval {
			get;
			set;
		}

		public int WorkInterval {
			get;
			set;
		}

		public int NumberOfSets {
			get;
			set;
		}

		[Unique]
		public DateTime TabataDate {
			get;
			set;
		}

		#endregion

		public Tabata ()
		{	

		}

		public Tabata (int workInterval, int restInterval, int numberOfSets)
		{
			this.WorkInterval = workInterval;
			this.RestInterval = restInterval;
			this.NumberOfSets = numberOfSets;

			this.TabataDate = DateTime.Now;

			_currentSet = 1;
		}

		public void StartTabata (Action<string> workUpdate, Action<string> restUpdate, Action<bool, int> switchState, Action finishedUpdate)
		{		
			var keepOnLooping = true;
			var currentState = "work";

			_currentWorkSecond = this.WorkInterval;

			Task.Run (async delegate {
							
				while (keepOnLooping) {
					await Task.Delay (1000);
						
				
					if (currentState.Equals ("work")) {
						_currentWorkSecond -= 1;

						// Update the display
						workUpdate (_currentWorkSecond.ToString ());

						if (_currentWorkSecond == 0) {
							currentState = "rest";
							_currentRestSecond = this.RestInterval;
							switchState (false, _currentSet);
							restUpdate (_currentRestSecond.ToString ());
						}

					} else {
						// the rest interval
						_currentRestSecond -= 1;

						// update the display
						restUpdate (_currentRestSecond.ToString ());

						if (_currentRestSecond == 0) {
							currentState = "work";
							_currentSet += 1;

							if (_currentSet > this.NumberOfSets) {
								keepOnLooping = false;
								finishedUpdate ();
							} else {
								_currentWorkSecond = this.WorkInterval;
								switchState (true, _currentSet);
								workUpdate (_currentWorkSecond.ToString ());
							}
						}
					}
				}
				
			});
		}
	}
}

