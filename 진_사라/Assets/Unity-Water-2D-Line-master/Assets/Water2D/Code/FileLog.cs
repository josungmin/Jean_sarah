using UnityEngine;
using System.Collections;
using System.IO;
using System;

public sealed class FileLog {

	private static volatile FileLog _instance;
	private string _logPath = Application.persistentDataPath + "/tdebug.log";
	private static object _lock = new object();
	public static FileLog Instance{
		get{
			if (_instance == null){
				lock(_lock){
					if (_instance == null){
						_instance = new FileLog();
					}
				}
			
			}
			return _instance;
		}
	}

	private FileLog(){
		if (File.Exists (_logPath)) {
			File.Delete(_logPath);
		}
	}

	public void Log(string format, params object[] args){
		DateTime dt = DateTime.Now;
		string time = dt.ToString ("yyyy/MM/dd HH:mm:ss") + " " + dt.Millisecond.ToString ();
		string message = string.Format (format, args);
		message = "[" + time + "]" + message;
		FileStream file = new FileStream (_logPath, FileMode.Append);
		if (file != null) {
			StreamWriter writer = new StreamWriter(file);
			writer.WriteLine (message);
			writer.Close ();
			file.Close ();
		}
	}
}
