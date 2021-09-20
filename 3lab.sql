use world;

select count(*) from countrylanguage
group by IsOfficial;


/* 2 /////////////////////////////////////////////////////////// */
show tables;
select * from countrylanguage;


-- 2.1//////////////////////////
select name from country
where name like 'A%';
/*left(name, 1)='A';*/


-- 2.2//////////////////////////
select city.name, country.name from city
inner join countrylanguage
on city.countrycode = countrylanguage.countrycode
inner join country
on country.code = city.countrycode
where countrylanguage.language = 'Russian';

s

/*select country.name, avg(city.population) as averege from country
inner join city
on city.countrycode = country.code
group by country.code;
select country.name, city.population from country
inner join city
on city.id = country.capital;*/

-- 2.3//////////////////////////0
select t1.nm, t1.capital, t2.average from 
(select country.name as nm, city.population as capital from country
inner join city
on city.id = country.capital) as t1
inner join
(select country.name as nm, avg(city.population) as average from country
inner join city
on city.countrycode = country.code
group by country.code) as t2
on t1.nm = t2.nm
where t1.capital >= t2.average;

-- 2.4//////////////////////////
select t1.region, t1.cnt from
(select country.region as region, count(*) as cnt from country
inner join city
on city.countrycode = country.code
where country.continent = 'Asia'
group by country.region) as t1;

-- having country.continent = 'Asia'

-- select t1.region, t2.cnt from 
-- (select country.code as code, country.region as region from country
-- where country.continent = 'Asia') as t1
-- inner join
-- (select city.countrycode as code, count(city.name) as cnt from city
-- group by city.countrycode) as t2
-- on t1.code = t2.code;

use world;

select country.code, city.name from city
inner join country
on city.countrycode = country.code
where city.countrycode = 'BLR'
order by city.population desc limit 5;

/*select countrycode, name from 
(select *, rank() over (partition by countrycode order by population desc) as r
from city) as t
where t.r<=5;

select country.code as nm, avg(city.population) as average from country
inner join 
(select * from 
(select *, rank() over (partition by countrycode order by population desc) as r
from city) as t
where t.r<=5) as city
on city.countrycode = country.code
group by country.code;

select countrycode, sum(percentage)*population as q from countrylanguage
inner join country
on country.code = countrylanguage.countrycode
where isofficial = 'F'
group by countrycode;

select country.name as nm, avg(city.population) as average from country
inner join city
on city.countrycode = country.code
group by country.code;*/

-- 2.5//////////////////////////
select t1.cd, t1.average from
(select country.code as cd, avg(city.population) as average from country
inner join 
(select * from 
(select *, rank() over (partition by countrycode order by population desc) as r
from city) as t
where t.r<=5) as city
on city.countrycode = country.code
group by country.code) as t1
inner  join
(select countrycode as cd, sum(percentage)*population as q from countrylanguage
inner join country
on country.code = countrylanguage.countrycode
where isofficial = 'F'
group by countrycode) as t2
on t1.cd = t2.cd
where t1.average < t2.q;

/* additional //////////////////////////////////////////*/

select * from country;
select * from city;
select * from countrylanguage;

select t1.name, t1.cnt, t2.pop from
(select country.name as name, sum(countrylanguage.percentage * 0.01 * country.population) as cnt
from country
inner join countrylanguage
on countrylanguage.countrycode = country.code
where countrylanguage.isofficial = 'T'
group by country.name) as t1
inner join 
(select country.name as name, sum(city.population) as pop from city
inner join country
on city.countrycode = country.code
where city.id != country.capital
group by city.countrycode) as t2
on t1.name = t2.name
having t1.cnt < t2.pop;


select t1.code, t1.n1, t2.n2 from
(select countrylanguage.countrycode as code, avg(countrylanguage.percentage) as n1 
from countrylanguage
group by countrylanguage.countrycode) as t1
inner join 
(select city.countrycode as code, count(city.id) as n2 from city
group by city.countrycode) as t2
on t2.code = t1.code
where t1.n1 > t2.n2;

select countrylanguage.countrycode as code, avg(countrylanguage.percentage) as n1
from countrylanguage
group by countrylanguage.countrycode;

/* 3 //////////////////////////////////////////*/

show tables from univer;
use univer;
select * from author;
select * from book;
select * from customer;
select * from purch;

select purch.p_date, book.book_name, author.author_name, customer.customer_name from purch
inner join book
on purch.book_id = book.book_id
inner join author
on book.book_id = author.book_id
inner join customer
on purch.cust_id = customer.customer_id;

select * from author;
select  * from book;
select  * from work;

SET sql_safe_updates = 0;

update author, work
set author.author_name = 'BORIS'
where work.author_id = author.author_id
and work.book_id = 1;


/* 4 //////////////////////////////////////////*/

use world;

create or replace view capitals
(country, capital) as
select country.name, city.name from country
inner join city
on city.id = country.capital;

select * from capitals;

create or replace view test
(t1, t2, t3) as
select t1.code, t1.n1, t2.n2 from
(select countrylanguage.countrycode as code, avg(countrylanguage.percentage) as n1
from countrylanguage
group by countrylanguage.countrycode) as t1
inner join
(select city.countrycode as code, count(city.id) as n2 from city
group by city.countrycode) as t2
on t2.code = t1.code
where t1.n1 > t2.n2;

select * from test;


select * from capitals;

create or replace view country_1
(country, population) as
select country.name, country.population from country
where country.population > 10000000;

select * from country_1;

use lab4;

create table test
(
    id int
);

insert into test values (1), (4), (5);

drop view test1;
create view test1
(n) as (select id from test
where id>4)
with check option;

select * from test1;

insert into test1 value (1);





use world;

select * from city;
select * from country;

select concat(country.name,' ',city.name), city.population from country
inner join city
on country.code = city.countrycode
order by city.population desc limit 5,7;


create table color
(
    color_id int primary key auto_increment,
    color_name varchar(20)
);

create table product
(
    product_id int primary key auto_increment,
    product_price float
);

create table link
(
    link_id int primary key auto_increment,
    link_product int,
    link_color int,
    constraint cn1 foreign key (link_product) references product (product_id),
    constraint cn1 foreign key (link_color) references color (color_id)
);