/*
 * Copyright 2022
 * 
 * All rights reserved.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

using jsonList = System.Collections.Generic.List<System.Collections.Generic.Dictionary<string, string>>;
using jsonObject = System.Collections.Generic.Dictionary<string, string>;

namespace Estuary {

	public class EstuaryApi {
		string apiKey;
		string downloadUrl = "https://dweb.link/ipfs/";
		string uploadUrl = "https://shuttle-1.estuary.tech/content/add";
		string listUrl = "https://api.estuary.tech/content/list";
		public string errorMessage;
		
		JavaScriptSerializer serializer;
		public EstuaryApi(string apiKey) {
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
			this.apiKey = apiKey;
			serializer = new JavaScriptSerializer();
		}
		
		public jsonList list() {
			errorMessage = null;
			try {
				using (WebClient client = new WebClient()) {
					client.Headers.Add("Authorization", "Bearer" + " " + apiKey);
					string result = client.DownloadString(listUrl);
					jsonList list = serializer.Deserialize<jsonList>(result);
					return list;
				}
			} catch (Exception e) {
				errorMessage = e.Message;
				return null;
			}
		}
		
		public byte [] downloadFile(string cid) {
			errorMessage = null;
			try {
				using (WebClient client = new WebClient()) {
					byte[] result = client.DownloadData(downloadUrl + cid);
					return result;
				}
			} catch (Exception e) {
				errorMessage = e.Message;
				return null;
			}
		}
		
		public async Task<string> uploadFile(string path) {
			errorMessage = null;
			try {
				string name = Path.GetFileName(path);
				using (HttpClient client = new HttpClient()) {
					using (FileStream stream = new FileStream(path, FileMode.Open)) {
						client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
						MultipartFormDataContent form = new MultipartFormDataContent();
						form.Add(new StreamContent(stream), "data", name);
						HttpResponseMessage response = await client.PostAsync(uploadUrl, form);
						response.EnsureSuccessStatusCode();
						string result = response.Content.ReadAsStringAsync().Result;
						Dictionary<string, object> obj = serializer.Deserialize<Dictionary<string, object>>(result);
						return (string)obj["cid"];
					}
				}
			} catch (Exception e) {
				errorMessage = e.Message;
				return null;
			}
		}
	}
}