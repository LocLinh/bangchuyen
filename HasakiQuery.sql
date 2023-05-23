create database HasakiDB
go
use HasakiDB
go

create table Accounts(
	maTK varchar(5),
	tenTK varchar(20),
	matKhau varchar(20)
)
go

create table OutBounds(
	ngay varchar(50),
	thoiGian varchar(50),
	maKienHang varchar(100),
	tenTK varchar(20)
)
go

create table InBounds(
	ngay varchar(50),
	thoiGian varchar(50),
	maKienHang varchar(50),
	khoiLuong varchar(50),
	chieuDai varchar(50),
	chieuRong varchar(50),
	chieuCao varchar(50),
	maZone varchar(50),
	trangThai varchar(50),
	tenTK varchar(50)
)
go

create table HasakiSystem(
	ngay varchar(50),
	maKienHang varchar(50),
	maZone varchar(50),
	trangThai varchar(50)
)
go
------------------------------------------------------------------------------------------------------------------
insert into Accounts values ('TK00', 'admin', 'admin')
go
insert into HasakiSystem values ('00/00/0000', 'NG', '99', 0)
go


INSERT INTO HasakiSystem(ngay, maKienHang,maZone,trangthai) VALUES('28/04/2023','120342303300601','zone1',1);
INSERT INTO HasakiSystem(ngay, maKienHang,maZone,trangthai) VALUES('28/04/2023','120342303300597','zone7',1);
INSERT INTO HasakiSystem(ngay, maKienHang,maZone,trangthai) VALUES('28/04/2023','120342303300603','zone3',1);
INSERT INTO HasakiSystem(ngay, maKienHang,maZone,trangthai) VALUES('28/04/2023','120342303300602','zone2',1);
INSERT INTO HasakiSystem(ngay, maKienHang,maZone,trangthai) VALUES('28/04/2023','120342303300604','zone4',1);
INSERT INTO HasakiSystem(ngay, maKienHang,maZone,trangthai) VALUES('28/04/2023','120342303300596','zone6',1);
INSERT INTO HasakiSystem(ngay, maKienHang,maZone,trangthai) VALUES('28/04/2023','120342303300598','zone8',1);
INSERT INTO HasakiSystem(ngay, maKienHang,maZone,trangthai) VALUES('28/04/2023','120342303300605','zone5',1);


INSERT INTO HasakiSystem(ngay, maKienHang,maZone,trangthai) VALUES('04/05/2023','120342303300601','zone1',1);
INSERT INTO HasakiSystem(ngay, maKienHang,maZone,trangthai) VALUES('04/05/2023','120342303300597','zone7',1);
INSERT INTO HasakiSystem(ngay, maKienHang,maZone,trangthai) VALUES('04/05/2023','120342303300603','zone3',1);
INSERT INTO HasakiSystem(ngay, maKienHang,maZone,trangthai) VALUES('04/05/2023','120342303300602','zone2',1);
INSERT INTO HasakiSystem(ngay, maKienHang,maZone,trangthai) VALUES('04/05/2023','120342303300604','zone4',1);
INSERT INTO HasakiSystem(ngay, maKienHang,maZone,trangthai) VALUES('04/05/2023','120342303300596','zone6',1);
INSERT INTO HasakiSystem(ngay, maKienHang,maZone,trangthai) VALUES('04/05/2023','120342303300598','zone8',1);
INSERT INTO HasakiSystem(ngay, maKienHang,maZone,trangthai) VALUES('04/05/2023','120342303300605','zone5',1);

-----------------------------------------------------------------------------------------------------------------
delete from HasakiSystem where ngay = '09/05/2023'
go
select ngay as 'Ngày', maKienHang as 'Mã kiện hàng', maZone as 'Zone', trangThai as 'Trạng thái'
from HasakiSystem
where maKienHang = '120342303300605'
and ngay = '04/05/2023'
go
insert into InBounds values('28/04/2023', '2:10:00', '1124235353', '11,5', '800', '200', '200', 'zone1', 1, 'admin')
go
select * from InBounds
go
select * from OutBounds
go


select * from HasakiSystem where maKienHang = '109142205230018' and ngay = '9/23/2022'
go