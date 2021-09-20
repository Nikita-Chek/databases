use lab_13;

document = {
Name: "Ira",
Age: 19,
Group:{
Name:"KM",
Number:5
},
Exams: [{Name:"Math",Mark:10},{Name:"Prog",Mark:9},{Name:"BD",Mark:9}]
};
document = {
Name: "Anya",
Age: 18,
Group:{
Name:"KM",
Number:5
},
Exams: [{Name:"Math",Mark:6},{Name:"Prog",Mark:7},{Name:"BD",Mark:5}]
};
document = {
Name: "Dasha",
Age: 19,
Group:{
Name:"KM",
Number:5
},
Exams: [{Name:"Math",Mark:8},{Name:"Prog",Mark:6},{Name:"BD",Mark:6}]
};
document = {
Name: "Sasha",
Age: 20,
Group:{
Name:"KM",
Number:5
},
Exams: [{Name:"Math",Mark:5},{Name:"Prog",Mark:8},{Name:"BD",Mark:8}]
}
document = {
Name: "Kolya",
Age: 19,
Group:{
Name:"WEB",
Number:9
},
Exams: [{Name:"Math",Mark:4},{Name:"Prog",Mark:4},{Name:"BD",Mark:5}]
}
document = {
Name: "Rita",
Age: 18,
Group:{
Name:"WEB",
Number:9
},
Exams: [{Name:"Math",Mark:7},{Name:"Prog",Mark:7},{Name:"BD",Mark:7}]
}
document = {
Name: "Katya",
Age: 21,
Group:{
Name:"WEB",
Number:10
},
Exams: [{Name:"Math",Mark:9},{Name:"Prog",Mark:9},{Name:"BD",Mark:7}]
}

db.students.insertOne(document);
db.students.find();

db.students.find({Name:"Anya"}, {Exams:true});

db.students.find({Age:{$ne: 19}});

db.students.find({Age:{$gte: 19}});

db.students.find({ Exams:{ Name:"Math", Mark:6}});

db.students.find({ Exams:{$elemMatch:{ "Name":"Math", Mark:{$gte:7}}}});

db.students.find({"Group.Name": "WEB" }).sort({Age:-1});
db.students.find().sort({Age:1}).limit(3);

f = function(){
return (this.Group.Name == "KM" && this.Name.reverse() == "ahsaD")
}

db.students.aggregate(
[
{$unwind: "$Exams"},
{
$group:{
_id: "$Exams.Name",
minMark:{$min:"$Exams.Mark"}
}
}]
);

db.students.aggregate(
[
{
$group:{
_id: "$Group.Number",
count:{$sum:1}
}
}]
);

db.students.createIndex({Name:1});

db.students.find();