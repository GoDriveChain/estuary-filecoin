"""
 * Copyright 2022
 * 
 * All rights reserved.
"""

import os
import requests

class Estuary_FileCoin_Api:
	listUrl = 'https://api.estuary.tech/content/list'
	downloadUrl = 'https://dweb.link/ipfs/'
	uploadUrl = 'https://shuttle-1.estuary.tech/content/add'
	errorMessage = None

	def __init__(self, apiKey):
		self.apiKey = apiKey

	def list(self):
		errorMessage = None
		try:
			response = requests.get(self.listUrl, headers = {'Authorization': 'Bearer ' + self.apiKey})
			response.raise_for_status()
			return response.json()
		except Exception as e:
			self.errorMessage = str(e)
			return None

	def downloadFile(self, cid):
		errorMessage = None
		try:
			response = requests.get(self.downloadUrl + cid)
			response.raise_for_status()
			return response.content
		except Exception as e:
			self.errorMessage = str(e)
			return None

	def uploadFile(self, path):
		errorMessage = None
		try:
			response = requests.post(self.uploadUrl, files = {'data': (os.path.basename(path), open(path, 'rb'))}, headers = {'Authorization': 'Bearer ' + self.apiKey})
			response.raise_for_status()
			return response.json()['cid']
		except Exception as e:
			self.errorMessage = str(e)
			return None
