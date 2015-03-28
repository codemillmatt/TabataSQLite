using System;
using System.IO;
using System.Collections.Generic;

using System.Threading;
using System.Threading.Tasks;

namespace TabataPCL
{
	public class Tabata
	{
		int _currentWorkSecond;
		int _currentRestSecond;
		int _currentSet;

		#region Properties 

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

			_currentSet = 1;
		}

		public void StartTabata(Action<string> workUpdate, Action<string> restUpdate, Action<bool, int> switchState, Action finishedUpdate)
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
							restUpdate(_currentRestSecond.ToString());
						}

					} else {
						// the rest interval
						_currentRestSecond -= 1;

						// update the display
						restUpdate (_currentRestSecond.ToString());

						if (_currentRestSecond == 0) {
							currentState = "work";
							_currentSet += 1;

							if (_currentSet > this.NumberOfSets) {
								keepOnLooping = false;
								finishedUpdate ();
							} else {
								_currentWorkSecond = this.WorkInterval;
								switchState (true, _currentSet);
								workUpdate(_currentWorkSecond.ToString());
							}
						}
					}
				}
				
			});
		}
			
		public async void SaveTabata(StreamWriter file)
		{		
			this.TabataDate = DateTime.Now;

			string tabataInfo = string.Format ("{0},{1},{2},{3}", this.WorkInterval, this.RestInterval,
				                    this.NumberOfSets, this.TabataDate.ToString ());

			await file.WriteLineAsync (tabataInfo);
		}
	}

	public class AllTabatas : List<Tabata>
	{
		public void PopulateTabatas(StreamReader sr)
		{		
			while (true) {
				var tabataLine = sr.ReadLine ();

				if (tabataLine != null) {
					var individParts = tabataLine.Split (new char[] { ',' });

					var t = new Tabata ();  
					t.WorkInterval = int.Parse (individParts [0]);
					t.RestInterval = int.Parse (individParts [1]);
					t.NumberOfSets = int.Parse (individParts [2]);
					t.TabataDate = DateTime.Parse(individParts[3]);
							
					this.Add (t);
				} else {
					break;
				}
			}				
		}
	}
}

