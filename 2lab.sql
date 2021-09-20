create database univer;
use univer;

create table gruppa_scrip
(
stud_id int(11) auto_increment,
stud_name varchar(20),
sex set('m', 'w'),
salary int,
rating float,
edu_cost float,
birht datetime,
is_like_study bool,
primary key(stud_id) 
);

alter table gruppa_scrip
modify salary float;

alter table gruppa_scrip
change birht birth date after stud_name;

alter table gruppa_scrip
modify rating decimal(3,2);

alter table gruppa_scrip
add column stud_num int;

insert into gruppa_scrip (stud_name, sex,
salary, rating, edu_cost, birth, is_like_study, stud_num) values
('Masha', 'w', 120, 8, 2000,
'2001-03-12 00:00:00', False, 12345),
('Dasha', 'w', 110, 9.2, Null,
'2000-02-01 00:00:00', True, 23456),
('Kolya', 'm', 100, 7, 2000,
'2000-04-01 00:00:00', False, 34775);

insert into gruppa_scrip (stud_name, sex,
salary, rating, edu_cost, birth,
is_like_study, stud_num) values
('Daana', 'w', 130, 9.5, 3000,
'2002-02-01 00:00:00', True, 37752),
('Niiiick', 'm', 100, 6, 1500,
'2000-04-28 00:00:00', False, 33333);

select * from gruppa_scrip;

set sql_safe_updates = 0;

update gruppa_scrip
set salary = salary * 1.1, edu_cost = edu_cost * 1.15;

update gruppa_scrip
set stud_num = 44444
where stud_name = 'Niiiick';

update gruppa_scrip
set salary = salary * 1.2
where  (5 * length(stud_name)-length(REPLACE(stud_name,'a',''))-
		length(REPLACE(stud_name,'e',''))-
		length(REPLACE(stud_name,'i',''))-
		length(REPLACE(stud_name,'o',''))-
		length(REPLACE(stud_name,'u',''))) > 
        (-4 * length(stud_name)+
        length(REPLACE(stud_name,'a',''))+
		length(REPLACE(stud_name,'e',''))+
		length(REPLACE(stud_name,'i',''))+
		length(REPLACE(stud_name,'o',''))+
		length(REPLACE(stud_name,'u','')));        

update gruppa_scrip
set salary = salary * 1.2
where  (3 * length(stud_num)-length(REPLACE(stud_num,'3',''))-
		length(REPLACE(stud_num,'6',''))-
		length(REPLACE(stud_num,'9',''))) > 
        (length(stud_num)-
        length(REPLACE(stud_num,'7',''))); 


update gruppa_scrip
set salary = salary * 1.1
where  (5 * length(stud_num)-length(REPLACE(stud_num,'2',''))-
		length(REPLACE(stud_num,'4',''))-
		length(REPLACE(stud_num,'6',''))-
        length(REPLACE(stud_num,'8',''))-
        length(REPLACE(stud_num,'0',''))) > 
        (5 * length(stud_num)-
        length(REPLACE(stud_num,'1',''))-
        length(REPLACE(stud_num,'3',''))-
        length(REPLACE(stud_num,'5',''))-
        length(REPLACE(stud_num,'7',''))-
        length(REPLACE(stud_num,'9',''))); 

select * from gruppa_scrip;

drop table gruppa_scrip_m;

create table gruppa_scrip_w
(
stud_id int(11) auto_increment,
stud_name varchar(20),
salary int,
rating float,
edu_cost float,
birth datetime,
is_like_study bool,
stud_num int,
primary key(stud_id) 
);

insert into gruppa_scrip_w
(stud_name, salary, rating, edu_cost, birth,
is_like_study, stud_num)
select stud_name, salary, rating, edu_cost,
birth, is_like_study, stud_num
from gruppa_scrip
where sex = 'w';

rename table gruppa_scrip to gruppa_scrip_m;

delete from gruppa_scrip_m
where sex = 'w';

alter table gruppa_scrip_m
drop column sex;

select date_add(curdate(),
interval
(if( dayofweek(curdate())>5, if(dayofweek(curdate())=6, 6, 5), 5-dayofweek(curdate())))
day);

select * from gruppa_scrip_w;

/*2.1 //////////////////////////////////////////////////////////////////////////*/

use univer;

create table t2_1
(
id int primary key auto_increment,
author_1 varchar(20),
author_2 varchar(20),
title varchar(20),
isbn int,
price float,
cust_name varchar(20),
cust_address varchar(30),
purch date
);

insert into t2_1 
(author_1, author_2, title, isbn, price, cust_name, cust_address, purch)
values
('David', 'Adam', 'PHP', 1234, 44.99, 'Emma', 'LA, 13', '2020-09-8'),
('Danny', null, 'HTML', 1235, 59.99, 'Darren', 'NY, 24', '2020-10-5'),
('Hugh', 'Lane', 'MYSQL', 1244, 44.95, 'Earl', 'WS, 56', '2020-10-6'),
('David', 'Adam', 'PHP', 2423, 44.99, 'Darren', 'NY, 24', '2020-11-1'),
('Rasmus', 'Kevin', 'C++', 7544, 39.99, 'Nick', 'DC, 45', '2020-11-4');

drop table book;

create table book
(
book_id int primary key auto_increment,
book_name varchar(20),
price float
);

select * from work;

create table author
(
author_id int primary key auto_increment,
author_name varchar(20)
);

create table work
(
work_id int primary key auto_increment,
author_id int,
book_id int,
constraint cn0 foreign key (author_id) references author (author_id),
constraint cn1 foreign key (book_id) references book (book_id)
);

create table customer
(
customer_id int primary key auto_increment,
customer_name varchar(20),
address varchar(30)
);

create table purch
(
isbn int primary key,
book_id int,
p_date date,
constraint cn2 foreign key (book_id) references book (book_id),
constraint cn3 foreign key (cust_id) references customer (customer_id)
);

 /*///////////////////////////////////////////////////////*/
insert into book
(book_name, price)
select distinct title, price
from t2_1;


select * from t2_1 ;
drop table author_t;
create table author_t
(
author_id int primary key auto_increment,
author_name varchar(20),
book_id int,
constraint cn foreign key (book_id) references book (book_id)
);

insert into author_t
(author_name, book_id)
select distinct author_1, book_id
from  t2_1
left join book on (book_name = title);
/*where author_1 != null;*/

insert into author_t
(author_name, book_id)
select distinct author_2, book_id
from  t2_1
left join book on (book_name = title);
/*where author_2 != null;*/
select * from author_t;

insert into author
(author_name)
select author_t.author_name from author_t;
-- where author_t.author_name != null;
select * from t2_1;

insert into work
(book_id, author_id)
select distinct book_id, author_id
from author_t;
where author_name != null;

select * from work;

insert into customer
(customer_name, address)
select distinct cust_name, cust_address
from t2_1;

insert into purch
(isbn, book_id, p_date, cust_id)
select isbn, book_id, purch, customer_id
from t2_1
left join book on book_name = title
left join customer on cust_name = customer_name;

select * from purch;
select * from t2_1;

/*2.2 ///////////////////////////////////////////////////////////////////////  */
drop table t2_2;

create table t2_2
(
id int primary key auto_increment,
f_name varchar(20),
l_name varchar(20),
c_name varchar(20),
birth varchar(50)
);

insert into t2_2
(f_name, l_name, c_name, birth)
values 
('Jane', 'Doe', 'Mary,Sam', '1-1-1992,5-15-1994'),
('John', 'Doe', 'Mary,Sam', '1-1-1992,5-15-1994'),
('Jane', 'Smith', 'John,Pat,Lee,Mary', '10-5-1994,10-12-1990,6-6-1996,8-21-1994'),
('John', 'Smith', 'Michael', '7-4-1996'),
('Jane', 'Jones', 'Edward,Marta', '10-1-1992,10-15-1989');

alter table t2_2
add column c_name1 varchar(20) after birth;
alter table t2_2
add column c_name2 varchar(20);
alter table t2_2
add column c_name3 varchar(20);
alter table t2_2
add column c_name4 varchar(20);

set sql_safe_updates = 0;
update t2_2
set c_name1 = substring_index(c_name, ',', 1);
update t2_2
set c_name = right(c_name,
length(c_name)-length(c_name1)-1);

update t2_2
set c_name2 = substring_index(c_name, ',', 1);
update t2_2
set c_name = right(c_name,
length(c_name)-length(c_name2)-1);

update t2_2
set c_name3 = substring_index(c_name, ',', 1);
update t2_2
set c_name = right(c_name,
length(c_name)-length(c_name3)-1);

update t2_2
set c_name4 = substring_index(c_name, ',', 1);
update t2_2
set c_name = right(c_name,
length(c_name)-length(c_name4)-1);

alter table t2_2
add column birth1 varchar(20);
alter table t2_2
add column birth2 varchar(20);
alter table t2_2
add column birth3 varchar(20);
alter table t2_2
add column birth4 varchar(20);

set sql_safe_updates = 0;
update t2_2
set birth1 = substring_index(birth, ',', 1);
update t2_2
set birth = right(birth,
length(birth)-length(birth1)-1);

update t2_2
set birth2 = substring_index(birth, ',', 1);
update t2_2
set birth = right(birth,
length(birth)-length(birth2)-1);

update t2_2
set birth3 = substring_index(birth, ',', 1);
update t2_2
set birth = right(birth,
length(birth)-length(birth3)-1);

update t2_2
set birth4 = substring_index(birth, ',', 1);
update t2_2
set birth = right(birth,
length(birth)-length(birth4)-1);

drop table family;

create table family
(
fam_id int primary key auto_increment,
fam_name varchar(20)
);

insert into family
(fam_name)
select distinct l_name from t2_2;

drop table parents;

create table parents
(
p_id int primary key auto_increment,
p_name varchar(20),
f_id int,
constraint cn4 foreign key (f_id) references family (fam_id)
);

insert into parents
(p_name, f_id)
select f_name, fam_id
from t2_2
left join family on fam_name = l_name;

select * from family;

drop table childrens;

create table childrens
(
child_id int primary key auto_increment,
child_name varchar(20),
child_birth date,
f_id int,
constraint cn5 foreign key (f_id) references family (fam_id)
);



insert into childrens
(child_name, child_birth, f_id)
select distinct c_name1, concat(right(birth1, 4), '-', substring_index(birth1, '-', 2)), fam_id
from t2_2
left join family on fam_name = l_name
where c_name1 != '';

insert into childrens
(child_name, child_birth, f_id)
select distinct c_name2, concat(right(birth2, 4), '-', substring_index(birth2, '-', 2)), fam_id
from t2_2
left join family on fam_name = l_name
where c_name2 != '';

insert into childrens
(child_name, child_birth, f_id)
select distinct c_name3, concat(right(birth3, 4), '-', substring_index(birth3, '-', 2)), fam_id
from t2_2
left join family on fam_name = l_name
where c_name3 != '';

insert into childrens
(child_name, child_birth, f_id)
select distinct c_name4, concat(right(birth4, 4), '-', substring_index(birth4, '-', 2)), fam_id
from t2_2
left join family on fam_name = l_name
where c_name4 != '';

select * from t2_2;
select * from childrens;

/*3.1 ////////////////////////////////////////////////////////////////////////////////*/

use univer;

create table movie
(
title varchar(20),
star varchar(20),
producer varchar(20)
);

insert into movie
values ('Great','Lovely','Money'), ('Great','Man','Money'), ('Great','Lovely','Helen'),
('Great','Man','Helen'), ('Boring','Lovely','Helen'), ('Boring','Child','Helen');

drop table prods;
create table prods
(
title varchar(20),
producer varchar(20)
);

drop table stars;
create table stars
(
title varchar(20),
star varchar(20)
);

insert into prods
(title, producer)
select distinct title, producer from movie;

insert into stars
(title, star)
select distinct title, star from movie;

select * from prods;
select * from stars;

/*3.2 /////////////////////////////////////////////////////*/
drop table study;
create table study
(
last_name varchar(20),
curce varchar(20),
book varchar(20)
);

insert into study
values ('A', 'Inf', 'inf'), ('A', 'Netw', 'inf'), ('A', 'Inf', 'netw'),
('A', 'Netw', 'netw'), ('B', 'Prog', 'prog'), ('B', 'Prog', 'theo');

create table curse
(
last_name varchar(20),
curse varchar(20)
);

insert into curse
select distinct last_name, curce from study;

create table books
(
book varchar(20),
curse varchar(20)
);

insert into books
select distinct book, curce from study;

select * from curse;
select * from books;
select * from study;

/*3.3 ///////////////////////////////////////////////////////////////*/

drop table stops;
create table stops
(
num int,
start time,
end time,
tarif varchar(20)
);

insert into stops
values (1,'9:30', '10:30', 'care'), (1,'11:30', '12:00', 'care'),
(1,'14:00', '15:30', 'stand'), (2,'10:00', '11:30', 'prem1'),
(2,'11:30', '13:30', 'prem1'), (2,'15:00', '16:30', 'prem2');

create table booking
(
tarif varchar(20),
start time,
end time
);

insert into booking
select distinct tarif, start, end from stops;

create table tarifs
(
num int,
tarif varchar(20)
);

insert into tarifs
select distinct num, tarif from stops;

select * from tarifs;
select * from booking;

/*3.4 ////////////////////////////////////////////////*/

drop table shop;
create table shop
(
prod_name varchar(20),
supplier_name varchar(20),
pack_size int
);

insert into shop
values ('Chai', 'Exo', 16), ('Chai', 'Exo', 12), ('Chai', 'Exo', 8),
('Chef', 'New', 16), ('Chef', 'New', 12), ('Chef', 'New', 8),
('Pavlova', 'Pav', 16), ('Pavlova', 'Pav', 12), ('Pavlova', 'Pav', 8);
select * from shop;

create table products
(
prod_name varchar(20),
pack_size int
);

insert into products
select distinct prod_name, pack_size from shop;

create table suppliers
(
supplier varchar(20),
prod_name varchar(20)
);

insert into suppliers
select distinct supplier_name, prod_name from shop;

select * from products;
select * from suppliers;