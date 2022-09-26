"""
 * Copyright 2022
 * 
 * All rights reserved.
"""

import Estuary_FileCoin
import sys

if __name__ == '__main__':
	apiKey = 'EST7695a194-8208-4457-a207-8bd962b452afARY'
	api = Estuary_FileCoin.Estuary_FileCoin_Api(apiKey)
	if len(sys.argv) == 2 and sys.argv[1] == 'list':
		list = api.list()
		if list is None:
			print('Error: {}'.format(api.errorMessage))
		else:
			for item in list:
				print('{}: {}'.format(item['cid'], item['name']))
	elif len(sys.argv) == 4 and sys.argv[1] == 'downloadFile':
		file = api.downloadFile(sys.argv[2])
		if file is None:
			print('Error: {}'.format(api.errorMessage))
		else:
			with open(sys.argv[3], 'wb') as f:
				f.write(file)
	elif len(sys.argv) == 3 and sys.argv[1] == 'uploadFile':
		cid = api.uploadFile(sys.argv[2])
		if cid is None:
			print('Error: {}'.format(api.errorMessage))
		else:
			print(cid)
	else:
		print('Usage:')
		print('  {} list'.format(sys.argv[0]))
		print('  {} downloadFile CID file.ext'.format(sys.argv[0]))
		print('  {} uploadFile file.ext'.format(sys.argv[0]))
