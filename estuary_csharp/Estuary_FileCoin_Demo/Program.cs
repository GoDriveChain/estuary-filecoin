/*
 * Copyright 2022
 * 
 * All rights reserved.
 */
using System;
using System.Windows.Forms;

namespace Estuary_FileCoin_Demo {
	internal sealed class Program {
		[STAThread]
		private static void Main(string[] args) {
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}
	}
}
