/*
 * Copyright 2022
 * 
 * All rights reserved.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;

using Estuary;

using jsonList = System.Collections.Generic.List<System.Collections.Generic.Dictionary<string, string>>;
using jsonObject = System.Collections.Generic.Dictionary<string, string>;

namespace Estuary_FileCoin_Demo{
	public partial class MainForm : Form {		
		public MainForm() {
			InitializeComponent();
		}
		
		void Button1Click(object sender, EventArgs e) {
			progressBar(true);
			EstuaryApi api = new EstuaryApi(textBox1.Text);
			jsonList list = api.list();
			progressBar(false);

			if (list != null) {
				listBox1.Items.Clear();
				List <string> tags = new List<string>();
				foreach (jsonObject obj in list) {
					listBox1.Items.Add(obj["name"]);
					tags.Add(obj["cid"]);
				}
				listBox1.Tag = tags;
			} else {
				MessageBox.Show(api.errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		
		void Button2Click(object sender, EventArgs e) {
			if (textBox2.Text == String.Empty) {
				MessageBox.Show("Enter CID or select file from list.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			
			saveFileDialog1.FileName = "";
			if (listBox1.Tag != null) {
				List<string> tags = (List<string>)listBox1.Tag;
				for (int i = 0; i < tags.Count; i++) {
					if (tags[i] == textBox2.Text) {
						saveFileDialog1.FileName = listBox1.Items[i].ToString();
					}
				}
			}
			
			DialogResult result = saveFileDialog1.ShowDialog();
			if (result == DialogResult.OK) {
				new Thread(delegate() {
					progressBar(true);
					EstuaryApi api = new EstuaryApi(textBox1.Text);
					byte[] file = api.downloadFile(textBox2.Text);
					progressBar(false);
					if (file != null) {
						File.WriteAllBytes(saveFileDialog1.FileName, file);
						MessageBox.Show("File downloaded");
					} else {
						MessageBox.Show(api.errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}).Start();
			}
		}
		
		void Button3Click(object sender, EventArgs e) {
			openFileDialog1.FileName = String.Empty;
			DialogResult result = openFileDialog1.ShowDialog();
			if (result == DialogResult.OK) {
				new Thread(delegate () {
					progressBar(true);
					EstuaryApi api = new EstuaryApi(textBox1.Text);
					string res = api.uploadFile(openFileDialog1.FileName).Result;
					progressBar(false);

					if (res != null) {
						MessageBox.Show(res, "File uploaded");
					} else {
						MessageBox.Show(api.errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
					button1.PerformClick();
				}).Start();
			}
		}
		
		void ListBox1SelectedIndexChanged(object sender, EventArgs e) {
			if (listBox1.SelectedIndex != -1) {
				List<string> tags = (List<string>)listBox1.Tag;
				textBox2.Text = tags[listBox1.SelectedIndex];
			}
		}
		
		void progressBar(bool enabled) {
			new Thread(delegate () {
				statusStrip1.Invoke((MethodInvoker)delegate() {
					if (enabled)
						toolStripProgressBar1.Style = ProgressBarStyle.Marquee;
					else
						toolStripProgressBar1.Style = ProgressBarStyle.Blocks;
				});
			}).Start();
		}
	}
}
