
-- 1/////////////////////////////////////////////

create database lab4;
use lab4;

DROP TABLE studs;
CREATE TABLE studs (
 st_id INT(11) PRIMARY KEY AUTO_INCREMENT,
 st_name VARCHAR(30),
 st_speciality ENUM('km', 'web', 'ped', 'mex'),
 st_form ENUM('p', 'f'),
 st_rating FLOAT,
 st_semester int default 1
);

DROP TABLE subjects;
CREATE TABLE subjects (
 sub_id INT(11) PRIMARY KEY AUTO_INCREMENT,
 sub_name VARCHAR(30),
 sub_teacher VARCHAR(30),
 sub_hours INT(11)
);

drop table offset;
create table offset
(
    ref_sub_id int,
    ref_stud_id int,
    pass bool default false,
    repass int default 0,
    unique key offset_key (ref_sub_id, ref_stud_id),
    constraint cn3 foreign key (ref_stud_id) references studs (st_id) on delete cascade on update cascade,
    constraint  cn4 foreign key (ref_sub_id) references subjects (sub_id)
);

drop table offset_list;
create table offset_list
(
    semester_number int,
    sub_id int,
    unique key key_o_l (semester_number, sub_id),
    constraint offset_lst foreign key (sub_id) references subjects (sub_id) on delete cascade on update cascade
);

drop table offset_stud;
create table offset_stud
(
    st_id int,
    is_pass bool default false,
    of_semester int,
    unique key offset_std_key (st_id, of_semester),
    constraint offset_st foreign key (st_id) references studs (st_id) on delete cascade on update cascade
);

drop trigger pass_offset_trigger;
create trigger pass_offset_trigger after insert on offset
    for each row
    begin
        declare passed int;
        declare to_pass int;
        declare semester int;

        set semester = (select st_semester from studs where st_id = new.ref_stud_id);
        set to_pass = (select count(sub_id) from offset_list where semester_number = semester);
        set passed = (select count(sub_id) from offset_list
                        inner join offset
                        on offset.ref_sub_id = offset_list.sub_id
                        where semester_number = semester and ref_stud_id = new.ref_stud_id  and new.pass = true);

        if  passed = to_pass  then
            insert into offset_stud (st_id, is_pass, of_semester) values
            (new.ref_stud_id, true, semester);
        end if;
    end;



drop trigger offset_pass;
create trigger offset_pass after update on offset
    for each row
    begin
        declare passed int;
        declare to_pass int;
        declare semester int;

        if (NEW.repass > 5) then
            delete from studs
                where studs.st_id = new.ref_stud_id;
        end if;

        set semester = (select st_semester from studs where st_id = new.ref_stud_id);
        set to_pass = (select count(sub_id) from offset_list where semester_number = semester);
        set passed = (select count(sub_id) from offset_list
                        inner join offset
                        on offset.ref_sub_id = offset_list.sub_id
                        where semester_number = semester and ref_stud_id = new.ref_stud_id and new.pass = true);

        if  passed = to_pass  then
            insert into offset_stud (st_id, is_pass, of_semester) values
            (new.ref_stud_id, true, semester);
        end if;
    end;

drop procedure pass_offset;
create procedure pass_offset(in st_id int, in sub_id int, in is_pass bool)
begin
    declare passed int;
    declare to_pass int;
    declare semester int;

    set semester = (select st_semester from studs where studs.st_id = st_id);
    if !(select count(semester_number) from offset_list
        where offset_list.sub_id = sub_id and offset_list.semester_number = semester) then
        signal sqlstate '45000'
            set message_text = 'No such offset in semestor';
    else
        if st_id>0 and sub_id>0 then
            if !exists(select ref_sub_id from offset
                where st_id = ref_stud_id and sub_id = ref_sub_id) then
                if is_pass then
                    insert into offset
                    (ref_sub_id, ref_stud_id, pass, repass) values
                    (sub_id, st_id, is_pass, 0);
                else
                    insert into offset
                    (ref_sub_id, ref_stud_id, pass, repass) values
                    (sub_id, st_id, is_pass, 1);
                end if;
            else
                if is_pass then
                    update offset
                    set pass = is_pass
                    where (st_id = ref_stud_id) and (sub_id = ref_sub_id);
                else
                    update offset
                    set repass = repass + 1
                    where st_id = ref_stud_id and sub_id = ref_sub_id;
                end if;
            end if;
        end if;
    end if;

    set to_pass = (select count(sub_id) from offset_list where semester_number = semester);
    set passed = (select count(sub_id) from offset_list
                        inner join offset
                        on offset.ref_sub_id = offset_list.sub_id
                        where semester_number = semester and offset.ref_stud_id = st_id  and offset.pass = true);
    if  passed = to_pass  then
            delete from offset
                where offset.ref_stud_id = st_id;
    end if;
end;

drop procedure get_offset;
create procedure get_offset()
begin
    select studs.st_name, subjects.sub_name, offset.pass, offset.repass from offset
    inner join studs on offset.ref_stud_id = studs.st_id
    inner join subjects  on offset.ref_sub_id = subjects.sub_id;
end;


call pass_offset(3, 4, true);
call get_offset();
select * from offset_list;
select * from studs;
select * from offset_stud;

call pass_exam(3, 3, 9);

select * from exam_stud;
select * from exams;
select * from exam_list;


DROP TABLE exams;
CREATE TABLE exams (
 ex_id INT(11) PRIMARY KEY AUTO_INCREMENT,
 ref_sub_id INT(11),
 ref_st_id INT(11),
 ex_date DATE,
 ex_mark TINYINT,
 CONSTRAINT exam_1 FOREIGN KEY (ref_st_id) REFERENCES studs (st_id) on delete cascade on update cascade,
 CONSTRAINT exam_2 FOREIGN KEY (ref_sub_id) REFERENCES subjects (sub_id)
);

drop table exam_stud;
create table exam_stud
(
    st_id int,
    is_pass bool default false,
    ex_semester int,
    st_average_mark float,
    unique key exam_std_key (st_id, ex_semester),
    constraint exam_st foreign key (st_id) references studs (st_id) on delete cascade on update cascade
);

drop table exam_list;
create table exam_list
(
    semester_number int,
    sub_id int,
    unique key key_e_l (semester_number, sub_id),
    constraint exam_lst foreign key (sub_id) references subjects (sub_id) on delete cascade on update cascade
);

drop trigger is_pass_offsets;
create trigger is_pass_offsets before insert on exams
    for each row
    begin
        declare semester int;
        set semester = (select st_semester from studs where st_id = new.ref_st_id);
        if !(select count(is_pass) from offset_stud where st_id = new.ref_st_id and of_semester = semester)
            then
            signal sqlstate '45000'
            set message_text = 'Dint pass the offsets!!!';
        end if;
    end;

drop trigger pass_exam_trigger;
create trigger pass_exam_trigger after insert on exams
    for each row
    begin
        declare mark float;
        declare passed int;
        declare to_pass int;
        declare semester int;

        set semester = (select st_semester from studs where st_id = new.ref_st_id);
        set to_pass = (select count(sub_id) from exam_list where semester_number = semester);
        set passed = (select count(sub_id) from exam_list
                        inner join exams
                        on exams.ref_sub_id = exam_list.sub_id
                        where semester_number = semester and ref_st_id = new.ref_st_id);
        set mark = (select avg(exams.ex_mark) from exam_list
                    inner join exams
                    on exams.ref_sub_id = exam_list.sub_id
                    where exam_list.semester_number = semester and exams.ref_st_id = new.ref_st_id);

        if  passed = to_pass  then
            update studs set st_semester = st_semester + 1
            where st_id = new.ref_st_id;
            insert into exam_stud (st_id, is_pass, ex_semester,  st_average_mark) values
            (new.ref_st_id, true, semester, mark);
        end if;
    end;

drop procedure pass_exam;
create procedure pass_exam(in student int, in subject int, in mark float)
begin
    declare passed int;
    declare to_pass int;
    declare semester int;
    set semester = (select st_semester from studs where st_id = student);

    if (select semester_number from exam_list
        where sub_id = subject and semester_number = semester) then
        insert into exams (ref_sub_id, ref_st_id, ex_date, ex_mark) values
        (subject,student, now(), mark);
    else
        signal sqlstate '45000'
            set message_text = 'No such exam in semestor';
    end if;
    set to_pass = (select count(sub_id) from exam_list where semester_number = semester);
    set passed = (select count(sub_id) from exam_list
                        inner join exams
                        on exams.ref_sub_id = exam_list.sub_id
                        where semester_number = semester and ref_st_id = student);
    if  passed = to_pass  then
            delete from exams
                where ref_st_id = student;
    end if;
end;


drop trigger t1;
create trigger pass_exam_refill_pay after insert on exam_stud
for each row
begin
	update pay
    set summa = summa - 500 * (new.st_average_mark)
	where ;
end;





drop table lesson;
create table lesson
(
    lesson_id int primary key auto_increment,
    lesson_subject int,
    lesson_date datetime default current_timestamp(),
    constraint lesson_cn foreign key (lesson_subject) references subjects(sub_id)
);

drop table attendance;
create table attendance
(
    attendance_stud int,
    attendance_lesson int,
    attendance_value bool default false,
    unique key key_a (attendance_stud, attendance_lesson),
    constraint att_1 foreign key (attendance_stud) references studs(st_id) on delete cascade on update cascade,
    constraint att_2 foreign key (attendance_lesson) references lesson(lesson_id)
);

drop table marks;
create table marks
(
    lesson_id int,
    stud_id int,
    mark tinyint,
    unique key key_m (lesson_id, stud_id),
    constraint m_1 foreign key (lesson_id) references lesson(lesson_id),
    constraint m_2 foreign key (stud_id) references studs(st_id) on delete cascade on update cascade
);


insert into lesson (lesson_subject) values
(1), (2), (4), (3), (1), (2);

insert into attendance (attendance_stud, attendance_lesson, attendance_value) values
(1,1,true), (2,1,true), (3, 1, true), (4, 1, false), (1,2,false), (2, 2, true), (4, 2, false),
(5, 1, true), (5, 2, false), (5,3, false);

insert into marks (lesson_id, stud_id, mark) values
(1,2, 7), (1,3, 8), (2, 2, 9), (2, 1, 6), (3, 4, 10);
insert into marks (lesson_id, stud_id, mark) values
(4, 5, 8), (4, 6, 9), (4, 7, 7), (5, 5, 9);


/*2 //////////////////////////////////////*/


use lab4;
SET sql_safe_updates = 0;


drop procedure attendance_auto;
create procedure attendance_auto(in id int)
begin
    update studs
        set st_miss = (select count(attendance_value)-sum(attendance_value) from attendance a
                        where a.attendance_stud = id)
    where st_id = id;
end;
call attendance_auto(5);


drop procedure attendance_auto_all;
create procedure attendance_auto_all()
begin
    declare end int;
    declare i int;
    set end = (select count(st_id) from studs), i = 1;
    label1: loop
        update studs
        set st_miss = (select count(attendance_value)-sum(attendance_value) from attendance a
                        where a.attendance_stud = i)
        where st_id = i;
        set i = i+1;
    if i <= end then
       iterate label1;
    end if;
    leave label1;
    end loop label1;
end;
call attendance_auto_all();


drop procedure rating_auto;
create procedure rating_auto(in id int)
begin
    update studs
        set st_rating = (select avg(mark) from marks
                        where stud_id = id)
    where st_id = id;
end;
call rating_auto(5);


drop procedure rating_auto_all;
create procedure rating_auto_all()
begin
    declare end int;
    declare i int;
    set end = (select count(st_id) from studs), i = 1;
    label1: loop
        update studs
        set st_rating = (select avg(mark) from marks
                        where stud_id = i)
        where st_id = i;
        set i = i+1;
    if i <= end then
       iterate label1;
    end if;
    leave label1;
    end loop label1;
end;






drop table pay;
create table pay
(
    stud_id int primary key,
    summa double,
    summa_paid double
);
insert into pay values
(5, 10000, 0);

drop table pay_semester;
create table pay_semester
(
    stud_id int,
    semester int,
    summa double,
    unique key p_s_key (stud_id, semester)
);

drop procedure pay_semester;
create procedure pay_semester(in student int, in sum double)
begin
    declare semester int;
    declare summa_to_pay double;
    set semester = (select st_semester from studs where st_id = student);
    set summa_to_pay = (select (summa-summa_paid)/(5-semester) from pay where stud_id = student);

    if summa_to_pay = sum then
        insert into pay_semester values
        (student, semester, sum);
        update pay
            set summa_paid = summa_paid + sum
        where stud_id = student;
    else
        signal sqlstate '45000'
        set message_text = 'Wrong sum';
    end if;
end;

call lab4.pay_semester(5, 2500);
select * from pay_semester;





/*3 //////////////////////////////////////*/

delimiter //
create procedure f()
begin
	select * from studs;
end//
delimiter ;


delimiter //
create procedure f1(in percent float)
begin
	update studs set st_pay = st_pay * percent
    where st_form = 'p';
end//
delimiter ;

call f();
call f1(1.2);



delimiter //
create procedure f2(in prepod varchar(20))
begin
	select avg(ex_mark) from exams
	inner join subjects
	on ref_sub_id = sub_id
	where sub_teacher = prepod;
end//
delimiter ;

call f2('Scooby');
select * from exams;



drop procedure f3;
delimiter //
create procedure f3()
begin
	update studs set st_pay = st_pay * 1.1
    where st_form = 'p' and st_activity > 3;
end//
delimiter ;

call f3();



drop table f4_res;
create table f4_res
(
st_name varchar(20),
st_value float
);

drop procedure f4;
delimiter //
create procedure f4()
begin
	declare is_end bool default false;
    declare x varchar(20);
    declare y float;
    
    declare stud_top5 cursor for
    select studs.st_name, studs.st_rating from studs order by st_rating desc limit 5;
    
    declare continue handler for not found set is_end = true;
    open stud_top5;
    while is_end = false do
		fetch stud_top5 into x, y;
        if is_end = false then
			insert into f4_res values(x, y);
		end if;
	end while;
	close stud_top5;
end//
delimiter ;

call f4();
select st_name, st_value from f4_res;




drop procedure f5;
delimiter //
create procedure f5()
begin
	delete from studs
	where st_rating<4 and st_miss > 10;
end//
delimiter ;

call f5();




set global log_bin_trust_function_creators = 1;
drop function f6;
delimiter //
create function f6()
returns varchar(20)
begin
	declare popmark varchar(20);
	select concat(st_rating,'--', count(st_rating)) into popmark from studs
    group by st_rating
    order by count(st_rating) desc limit 1;
	return popmark;
end//
delimiter ;

select f6();




drop procedure f7;
delimiter //
create procedure f7()
begin
	declare hours int;
    select sum(sub_hours) into hours from subjects;
    
    select st_name, (hours - st_miss)/hours from studs;
end//
delimiter ;

call f7();

select *
from studs;


drop procedure f8;
delimiter //
create procedure f8()
begin
	select sub_teacher, t.m from subjects
	inner join
	(select ref_sub_id as id, avg(ex_mark) as m
	from exams group by ref_sub_id) as t
	on subjects.sub_id = t.id
	order by t.m desc;
end//
delimiter ;

call f8();




drop table f9;
create table f9
(
id int primary key auto_increment,
d date,
m int default 1000
);


insert into f9 (d) values
('2020-3-4'), ('2019-4-6'), ('2018-12-20'), ('2016-5-10'),('2010-7-16');
select * from f9;


drop procedure f9;
delimiter //
create procedure f9(in input date)
begin
	update f9 set m = m + (2020 - convert(d, signed integer) div 10000)
    where d>input ;
end//
delimiter ;

call f9();




drop procedure f10;
delimiter //
create procedure f10()
begin
    update f9 set m = m * 3
    where dayofweek(d) = 7;
end//
delimiter ;

call f10();


drop procedure f11;
delimiter //
create procedure f11(in id int) -- subject id
begin
	declare temp int;
    select avg(ex_mark) into temp from exams
    where ref_sub_id = id;
    
    select st_name, (st_rating +temp)/2 from studs;
    
end//
delimiter ;

call f11(3);

/*4 //////////////////////////////////////*/
 
drop trigger t1;
delimiter //
create trigger t1 after insert on exam_stud
for each row
begin
	update studs
    set st_value =
    (select avg(ex_mark) from exams
    where st_id = new.ref_st_id)
	where st_id = new.ref_st_id;
    
    update studs
    set st_pay = studs.st_rating * (0.1 * studs.st_rating + 1)
    where st_id = new.ref_st_id and studs.st_rating>8;
end//
delimiter ;

call f();
insert into exams (ref_sub_id, ref_st_id, ex_date, ex_mark)
values (3,2,now(),9);

select * from exams;
select * from studs;

drop trigger t3;
delimiter //
create trigger t3 before insert on exams
for each row
begin
	if new.ex_date > now() then
		signal sqlstate '45000';
	end if;
end//
delimiter ;

insert into exams (ref_sub_id, ref_st_id, ex_date, ex_mark)
values (2, 4, '2020-11-1', 9);



drop table t4;
create table t4
(
id int primary key auto_increment,
name varchar(20),
curs tinyint default 1,
ex_pass bool default false
);


insert into t4 (name) values
('nick'), ('mike');
select * from t4;

drop trigger t4;
delimiter //
create trigger t4 before update on t4
for each row
begin
	if new.ex_pass then
		set new.curs = old.curs + 1;
        set new.ex_pass = false;
	end if;
end//
delimiter ;

update t4 
set ex_pass = true
where id = 1;


/*5 //////////////////////////////////////*/
use lab4;




INSERT INTO studs (st_name,st_speciality, st_form) VALUES
('Masha', 'km', 'p'),
('Dasha', 'web', 'f'),
('Kolya', 'km', 'f'),
('Mike', 'mex', 'p'),
('Nick', 'mex', 'p'),
('Salo', 'web', 'f'),
('Sum', 'km', 'f');

INSERT INTO subjects (sub_name, sub_teacher, sub_hours) VALUES
('Matan', 'Scooby', 32),
('Algebra', 'Sheggi', 23),
('Geometry', 'Dafna', 30),
('BD', 'Velma', 20),
('Diffur', 'Fred', 19);

insert into exam_list (semester_number, sub_id) values
(1,1), (1,2),(1,3), (2, 2), (2,4), (2,5), (3, 1), (3, 3), (4,1), (4, 2), (4, 5);

INSERT INTO exams(ref_sub_id, ref_st_id, ex_date, ex_mark) VALUES
(1, 1, '2000-02-01', 7),
(1, 2, '2000-02-01', 7),
(1, 3, '2000-02-01', 6),
(1, 4, '2000-02-01', 8),
(2, 5, '2000-02-01', 7),
(2, 1, '2000-02-01', 9),
(3, 6, '2000-02-01', 5),
(3, 7, '2000-02-01', 7),
(3, 3, '2000-02-01', 6),
(4, 4, '2000-02-01', 8),
(4, 5, '2000-02-01', 8),
(4, 3, '2000-02-01', 9),
(5, 1, '2000-02-01', 8),
(5, 2, '2000-02-01', 9),
(5, 3, '2000-02-01', 10);

insert into offset_list (semester_number, sub_id) values
(1, 4), (1,5), (2,3), (2,4), (2,5), (3, 1), (3,2), (3,4), (4, 1);
