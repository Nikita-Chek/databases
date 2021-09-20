use lab_13;

query = db.students.find({Name: {$in:["Anya", "Kolya", "Dasha"]}});

db.students.find({Name: {$in:["Anya", "Kolya", "Dasha"]}}).forEach( function(myDoc) { print( "user: " + myDoc._id ); })
db.students.find().forEach( function(m) { print("user: " + m.Name ); })

stud1 = db.students.findOne()
stud2 = db.students.findOne({Name:"Kolya"})

document = {
Name:"Swimming",
Date: new Date(),
Student: new DBRef("students", stud1._id)
}

db.competition.insertOne({
Name:"Swimming",
Date: new Date(),
Student: new DBRef("students", stud1._id)
})

db.competition.remove({})
db.competition.find()


db.competition.aggregate(
[
{
$project: {
Name:"$Name",
Student:{$arrayElemAt:[{$objectToArray:"$Student"},1]}
}},
{
$project:{
Name: "$Name",
Stud: "$Student.v"
}},
{
$lookup: {
from: "students",
localField: "Stud",
foreignField: "_id",
as: "IntData"
}
}
]
)

db.students.find().sort({Age:1}).limit(3);

db.students.aggregate(
[
{
$group:{
_id: "$Group.Number",
count:{$sum:1}
}
}]
);

//mongoimport --jsonArray --db=lab_13 --collection=students --file="C:\Users\admin\stud.json"
