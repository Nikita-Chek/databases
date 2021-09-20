from pymongo import MongoClient
import pprint
import json

class MongoCRUD:
	def __init__(self):
		self.client = MongoClient("mongodb://localhost:27017")
		self.db = self.client.lab_13
		self.students_collection = self.db.students

	def add_student(self, doc:dict):
		self.students_collection.insert_one(doc)

	def remove_student(self, doc:dict):
		self.students_collection.delete_one(doc)

	def update_student(self, query:dict, doc:dict):
		self.students_collection.update_one(query, doc)

	def show_student(self, *docs:dict):
		for doc in self.students_collection.find(*docs):
			pprint.pprint(doc, sort_dicts=False)

	def get_student(self):
		lst = []
		for x in self.students_collection.find():
			lst.append(x) 
		return lst

	def drop(self):
		self.students_collection.drop()

	def add_studs_from_file(self, path:str):
		with open(path) as f:
			file_data = json.load(f)
			self.students_collection.insert_many(file_data)

	def export_studs_to_file(self, path:str):
		docs = self.students_collection.find()
		with open(path, 'w') as json_out_file:
			json_out_file.write('[')
			for doc in docs:
				json_out_file.write(json.dumps({i:doc[i] for i in doc if i!='_id'}))
				json_out_file.write(',')
			json_out_file.write(']')
		
	def close(self):
		self.client.close()