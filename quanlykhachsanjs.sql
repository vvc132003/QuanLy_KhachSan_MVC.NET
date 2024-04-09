create database QuanLy_KhachSans;
use QuanLy_KhachSans

create table KhachSan
(
    id INT IDENTITY(1,1) PRIMARY KEY,
	tenkhachsan nvarchar(255) null,
	sosao int null,
	diachi nvarchar(255) null,
	thanhpho nvarchar(255) null,
	quocgia nvarchar(255) null,
	email nvarchar(255) null,
	sodienthoai nvarchar(255) null,
	loaihinh NVARCHAR(50) NULL,
    giayphepkinhdoanh NVARCHAR(50) NULL,
    ngaythanhlap DATE NULL,
);
INSERT INTO KhachSan (tenkhachsan, sosao, diachi, thanhpho, quocgia, email, sodienthoai, loaihinh, giayphepkinhdoanh, ngaythanhlap)
VALUES (N'Khách Sạn A', 5, N'Địa chỉ A', N'Thành phố A', N'Quốc gia A', 'chinhvovan13@gmail.com', '0123456789', N'Loại hình A', 'GP123456', '2023-01-15'),
       (N'Khách Sạn B', 4, N'Địa chỉ B', N'Thành phố B', N'Quốc gia B', 'chinhvovan13@gmail.com', '0987654321', N'Loại hình B', 'GP654321', '2022-05-20'),
       (N'Khách Sạn C', 3, N'Địa chỉ C', N'Thành phố C', N'Quốc gia C', 'chinhvovan13@gmail.com', '0369852147', N'Loại hình C', 'GP987654', '2021-10-08');

create table ThoiGian
(
    id INT IDENTITY(1,1) PRIMARY KEY,
	thoigiannhanphong time null,
	phuthunhanphong decimal(10,2) null,
	thoigianra time null,
	phuthutraphong decimal(10,2) null,
	mota nvarchar(255) null,
	idkhachsan int null,
    FOREIGN KEY (idkhachsan) REFERENCES KhachSan(id)
);
INSERT INTO ThoiGian (thoigiannhanphong, phuthunhanphong, thoigianra, phuthutraphong, mota, idkhachsan)
VALUES
('10:00', 20, '18:00', 20, 'Time slot 1', 3)

create table Tang
(
    id INT IDENTITY(1,1) PRIMARY KEY,
    tentang nvarchar(255) null,
	idkhachsan INT NULL,
    FOREIGN KEY (idkhachsan) REFERENCES KhachSan(id)
);
INSERT INTO Tang (tentang, idkhachsan)
VALUES (N'Tầng 1', 3),
       (N'Tầng 2', 3),
       (N'Tầng 3', 3);
create table Phong
(
    id INT IDENTITY(1,1) PRIMARY KEY,
	sophong int null,
	songuoi int null,
    loaiphong nvarchar(255) null,
    tinhtrangphong nvarchar(255) null,
    giatientheogio decimal(10,2) null,
	giatientheongay decimal(10,2) null,
	idtang int null,
    FOREIGN KEY (idtang) REFERENCES Tang(id) ,
	idkhachsan int null,
    FOREIGN KEY (idkhachsan) REFERENCES KhachSan(id) 
);
INSERT INTO Phong (sophong, songuoi, loaiphong, tinhtrangphong, giatientheogio, giatientheongay, idtang, idkhachsan)
VALUES (201, 2, N'Phòng Đơn', N'còn trống', 100000.00, 500000.00, 1, 1),
       (202, 4, N'Phòng Đôi', N'còn trống', 120000.00, 600000.00, 1, 1),
       (203, 3, N'Phòng VIP', N'còn trống', 150000.00, 800000.00, 1, 1),
	   (204, 3, N'Phòng VIP', N'còn trống', 150000.00, 800000.00, 1, 1),
	   (205, 2, N'Phòng Đơn', N'còn trống', 100000.00, 500000.00, 1, 1),
       (206, 4, N'Phòng Đôi', N'còn trống', 120000.00, 600000.00, 1, 1),
       (207, 3, N'Phòng VIP', N'còn trống', 150000.00, 800000.00, 1, 1),
	   (208, 3, N'Phòng VIP', N'còn trống', 150000.00, 800000.00, 1, 1);
create table ThietBi
(
    id INT IDENTITY(1,1) PRIMARY KEY,
	tenthietbi nvarchar(255) null,
    giathietbi decimal(10,2) null,
    ngaymua datetime null,
	soluongcon int null,
	image text null,
    mota nvarchar(255) null
);
create table ThietBiPhong
(
    id INT IDENTITY(1,1) PRIMARY KEY,
    ngayduavao datetime null,
    soluongduavao int null,
	idphong INT,
    idthietbi INT,
    FOREIGN KEY (idphong) REFERENCES Phong(id),
    FOREIGN KEY (idthietbi) REFERENCES ThietBi(id)
);
create table KhachHang
(
    id INT IDENTITY(1,1) PRIMARY KEY,
	hovaten nvarchar(255) null,
    sodienthoai nvarchar(255) null,
    email nvarchar(255) null,
    cccd nvarchar(255) null,
    tinh nvarchar(255) null,
    huyen nvarchar(255) null,
    phuong nvarchar(255) null,
    taikhoan nvarchar(255) null,
    matkhau nvarchar(255) null,
    trangthai nvarchar(255) null
);
select * from datphong
create table ChucVu
(
    id INT IDENTITY(1,1) PRIMARY KEY,
	tenchucvu nvarchar(255) null
);
INSERT INTO ChucVu (tenchucvu)
VALUES (N'Quản lý'),
       (N'Nhân viên lễ tân'),
       (N'Nhân viên buồng phòng');

create table BoPhan
(
    id INT IDENTITY(1,1) PRIMARY KEY,
    tenbophan nvarchar(255) null,
    mota text null,
    image text
);
INSERT INTO BoPhan (tenbophan, mota, image)
VALUES (N'Quản lý', N'Phụ trách các hoạt động của khách sạn', 'quản_lý.png'),
       (N'Kế toán', N'Thực hiện công việc kế toán và tài chính', 'ke_toan.png'),
       (N'Nhân sự', N'Chịu trách nhiệm về các vấn đề nhân sự', 'nhan_su.png');

create table ViTriBoPhan
(
    id INT IDENTITY(1,1) PRIMARY KEY,
	tenvitri nvarchar(255) null,
    mota text null,
    luong decimal(10,2) null,
    idbophan int null,
	foreign key (idbophan) references BoPhan(id)
);
INSERT INTO ViTriBoPhan (tenvitri, mota, luong, idbophan)
VALUES (N'Trưởng phòng', N'Quản lý và điều hành hoạt động của bộ phận', 15000000, 1),
       (N'Nhân viên kế toán', N'Thực hiện công việc kế toán và tài chính', 10000000, 2),
       (N'Quản lý nhân sự', N'Chịu trách nhiệm về các vấn đề nhân sự', 12000000, 3);

create table NhanVien 
(
    id INT IDENTITY(1,1) PRIMARY KEY,
	hovaten nvarchar(255) null,
    sodienthoai nvarchar(255) null,
    tinh nvarchar(255) null,
    huyen nvarchar(255) null,
    phuong nvarchar(255) null,
    taikhoan nvarchar(255) null,
    matkhau nvarchar(255) null,
    trangthai nvarchar(255) null,
    solanvipham int null,
    image text null,
	cccd nvarchar(255) null,
	gioitinh nvarchar(255) NULL,
	ngaysinh date NULL,
    idchucvu int null,
    idvitribophan int null,
    idbophan int null,
	foreign key (idbophan) references BoPhan(id),
    foreign key (idchucvu) references ChucVu(id),
	foreign key (idvitribophan) references ViTriBoPhan(id),
	idkhachsan int null,
    FOREIGN KEY (idkhachsan) REFERENCES KhachSan(id) 
);
select * from nhanvien
select * from ChucVu
--- xem giua
DECLARE @nhanvienhientai INT = 2;
DECLARE @nhanvienduocchon INT = 1;
SELECT COUNT(*) AS so_luong_cuoc_tro_chuyen
FROM CuocHoiThoai
WHERE EXISTS (
    SELECT 1
    FROM NguoiThamGia
    WHERE CuocHoiThoai.id = NguoiThamGia.cuochoithoaiid
    AND NguoiThamGia.nhanvienthamgiaid = @nhanvienhientai
)
AND EXISTS (
    SELECT 1
    FROM NguoiThamGia
    WHERE CuocHoiThoai.id = NguoiThamGia.cuochoithoaiid
    AND NguoiThamGia.nhanvienthamgiaid = @nhanvienduocchon
)
AND CuocHoiThoai.loaihoithoai = '1-1';

CREATE TABLE CuocHoiThoai (
    id INT IDENTITY(1,1) PRIMARY KEY,
    tieude NVARCHAR(40),
    nhanvientaoid INT,
    loaihoithoai NVARCHAR(45),
    duoctaovao DATETIME DEFAULT GETDATE(),
    duoccaonhatvao DATETIME DEFAULT GETDATE(),
    daxoavao DATETIME DEFAULT GETDATE(),
	FOREIGN KEY (nhanvientaoid) REFERENCES NhanVien(id)
);
INSERT INTO CuocHoiThoai (tieude, nhanvientaoid, loaihoithoai)
VALUES
    ('Cuộc trò chuyện 1', 1, N'1-1'),
    ('Cuộc trò chuyện 3', 2, N'nhóm');
								
	CREATE TABLE NguoiThamGia (
    id INT IDENTITY(1,1) PRIMARY KEY,
    cuochoithoaiid INT,
    nhanvienthamgiaid INT,
    duoctaovao DATETIME DEFAULT GETDATE(),
    duoccapnhatvao DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (cuochoithoaiid) REFERENCES CuocHoiThoai(id),
    FOREIGN KEY (nhanvienthamgiaid) REFERENCES NhanVien(id)
);
INSERT INTO NguoiThamGia (cuochoithoaiid, nhanvienthamgiaid)
VALUES
    (1, 1),
    (1, 2),
    (2, 1),
    (2, 2),
    (2, 3);
									SELECT ch.id, ch.tieude, ch.duoctaovao,ch.loaihoithoai
                                    FROM CuocHoiThoai ch
                                    JOIN NguoiThamGia nt ON ch.id = nt.cuochoithoaiid
                                    WHERE nt.nhanvienthamgiaid = @nhanvienthamgiaid
                                    ORDER BY ch.duoctaovao DESC
CREATE TABLE TinNhan (
    id INT IDENTITY(1,1) PRIMARY KEY,
    cuochoithoaiid INT,
    nhanvienguiid INT,
    loaitinnhan NVARCHAR(10),
    noidung NVARCHAR(255),
    duoctaovao DATETIME DEFAULT GETDATE(),
    daxoavao DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (cuochoithoaiid) REFERENCES CuocHoiThoai(id),
    FOREIGN KEY (nhanvienguiid) REFERENCES NhanVien(id)
);
select * from TinNhan
DECLARE @ConversationID INT = 1;

-- Query to retrieve the latest messages for a specific conversation
SELECT TN.*
FROM TinNhan TN
WHERE TN.cuochoithoaiid = @ConversationID
AND TN.duoctaovao = (
    SELECT MAX(duoctaovao)
    FROM TinNhan
    WHERE cuochoithoaiid = @ConversationID
);

INSERT INTO TinNhan (cuochoithoaiid, nhanvienguiid, loaitinnhan, noidung)
VALUES
    (1, 1, ' nhân', 'Nội s'),
    (1, 2, ' nhân', 'Nội dung tin nhắn cá nhân 2'),
    (2, 3, 'Tin nhắn nhóm', 'Nội dung tin nhắn nhóm 1');
CREATE TABLE Icon (
    id INT IDENTITY(1,1) PRIMARY KEY,
    icons NVARCHAR(MAX),
    thoigianthem DATETIME DEFAULT GETDATE()
);
CREATE TABLE TinNhanIcon (
    id INT IDENTITY(1,1) PRIMARY KEY,
	thoigiannhan DATETIME DEFAULT GETDATE(),
    tinnhanid INT
	FOREIGN KEY (tinnhanid) REFERENCES TinNhan(id),
	iconid INT
	FOREIGN KEY (iconid) REFERENCES Icon(id)
);
INSERT INTO icon (icons, thoigianthem) VALUES (N'👋', GETDATE());
INSERT INTO icon (icons, thoigianthem) VALUES (N'🙌', GETDATE());
INSERT INTO icon (icons, thoigianthem) VALUES (N'👏', GETDATE());
INSERT INTO icon (icons, thoigianthem) VALUES (N'🤝', GETDATE());
INSERT INTO icon (icons, thoigianthem) VALUES (N'🙂', GETDATE());
INSERT INTO icon (icons, thoigianthem) VALUES (N'😊', GETDATE());
INSERT INTO icon (icons, thoigianthem) VALUES (N'👍', GETDATE());
INSERT INTO icon (icons, thoigianthem) VALUES (N'🥰', GETDATE());
INSERT INTO icon (icons, thoigianthem) VALUES (N'😂', GETDATE());
INSERT INTO icon (icons, thoigianthem) VALUES (N'❤️', GETDATE());
INSERT INTO icon (icons, thoigianthem) VALUES (N'😍', GETDATE());
INSERT INTO icon (icons, thoigianthem) VALUES (N'😎', GETDATE());
INSERT INTO icon (icons, thoigianthem) VALUES (N'🤗', GETDATE());
INSERT INTO icon (icons, thoigianthem) VALUES (N'🤩', GETDATE());
INSERT INTO icon (icons, thoigianthem) VALUES (N'😇', GETDATE());
INSERT INTO icon (icons, thoigianthem) VALUES (N'😎', GETDATE());
INSERT INTO icon (icons, thoigianthem) VALUES (N'🤗', GETDATE());
INSERT INTO icon (icons, thoigianthem) VALUES (N'😌', GETDATE());

INSERT INTO NhanVien (hovaten, sodienthoai, tinh, huyen, phuong, taikhoan, matkhau, trangthai, solanvipham, image, cccd, gioitinh, ngaysinh, idchucvu, idvitribophan, idbophan, idkhachsan)
VALUES (N'Võ Văn Chính', '0373449865', N'Quảng Trị', N'Triệu Phong', N'Triệu Đại', 'admin', 'chinh@2003#', N'còn làm việc', 0, 'linkanh1.jpg', '123456789', N'Nam', '2003-03-01', 1, 1, 1, 1),
       (N'Võ Văn Chính', '0373449865', N'Quảng Trị', N'Triệu Phong', N'Triệu Đại', 'amin1', '123@', N'còn làm việc', 0, 'linkanh2.jpg', '123456789', N'Nam', '2003-03-01', 1, 1, 1, 2),
	   (N'Võ Văn Chính', '0373449865', N'Quảng Trị', N'Triệu Phong', N'Triệu Đại', 'amin1', '1234@', N'còn làm việc', 0, 'linkanh2.jpg', '123456789', N'Nam', '2003-03-01', 1, 1, 1, 3)
	  
create table HopDongLaoDong (
    id INT IDENTITY(1,1) PRIMARY KEY,
    loaihopdong NVARCHAR(255) NULL,
    ngaybatdau DATE NULL,
    ngayketthuc DATE NULL,
    idnhanvien INT NULL,
    FOREIGN KEY (idnhanvien) REFERENCES NhanVien(id)
);

create table KhenThuong
(
    id INT IDENTITY(1,1) PRIMARY KEY,
	ngaykhenthuong datetime null,
    lydo nvarchar(255) null,
	idnhanvien int null,
	foreign key(idnhanvien) references NhanVien(id)
);
create table KyLuat
(
    id INT IDENTITY(1,1) PRIMARY KEY,
	ngaykyluat datetime null,
    lydo nvarchar(255) null,
    idnhanvien int null,
	foreign key(idnhanvien) references NhanVien(id)

);
create table LoaiDatPhong
(	
    id INT IDENTITY(1,1) PRIMARY KEY,
    loaidatphong nvarchar(255) null
);


create table DatPhong
(
    id INT IDENTITY(1,1) PRIMARY KEY,
    ngaydat datetime null,
    ngaydukientra datetime null,
    tiendatcoc decimal(10,2) null,
    trangthai nvarchar(255) null,
	hinhthucthue nvarchar(255) null,
    idloaidatphong int null,
    idkhachhang int null,
    idphong int null,
	idthoigian int null,
    foreign key(idphong) references Phong(id),
    foreign key(idkhachhang) references KhachHang(id),
	FOREIGN KEY (idloaidatphong) REFERENCES LoaiDatPhong(id),
	FOREIGN KEY(idthoigian) references ThoiGian(id),
);

select * from khachhang
CREATE TABLE MaGiamGia (
    id INT IDENTITY(1,1) PRIMARY KEY,
    magiamgia VARCHAR(20) UNIQUE,
    mota VARCHAR(255),
	soluongdatphongtoithieu INT,
	tongtientoithieu DECIMAL(10,2),
	thoigiandatphong DECIMAL(10, 2),
	phantramgiamgia DECIMAL(10, 2),
	solansudungtoida int ,
	solandasudung int,
    ngaybatdau DATETIME,
    ngayketthuc DATETIME
);
CREATE TABLE SuDungMaGiamGia (
    id INT IDENTITY(1,1) PRIMARY KEY,
    idmagiamgia INT,	
    iddatphong INT,
    ngaysudung DATE,
    FOREIGN KEY (idmagiamgia) REFERENCES MaGiamGia(id),
    FOREIGN KEY (iddatphong) REFERENCES DatPhong(id)
);

create table LichSuThanhToan
(
    id INT IDENTITY(1,1) PRIMARY KEY,
	ngaythanhtoan datetime null,
    sotienthanhtoan decimal(10,2) null,
    hinhthucthanhtoan nvarchar(255) null,
	trangthai nvarchar(255) null,
	phantramgiamgia DECIMAL(10, 2),
    iddatphong int null,
    idnhanvien int null,
	foreign key(idnhanvien) references NhanVien(id),
    foreign key (iddatphong) references DatPhong(id)
);
SELECT MONTH(ngaythanhtoan) AS month, SUM(sotienthanhtoan) AS total 
FROM LichSuThanhToan 
GROUP BY MONTH(ngaythanhtoan) 
ORDER BY MONTH(ngaythanhtoan);
select * from lichsuthanhtoan
select * from datphong

create table HuyDatPhong
(
    id INT IDENTITY(1,1) PRIMARY KEY,
    ngayhuy datetime null,
    lydo nvarchar(255) null,
	iddatphong int null,
    idnhanvien int null,
	foreign key(idnhanvien) references NhanVien(id),
    foreign key (iddatphong) references DatPhong(id)
);
create table NhanPhong
(
    id INT IDENTITY(1,1) PRIMARY KEY,
    ngaynhanphong datetime null,
    iddatphong int null,
    idnhanvien int null,
	foreign key(idnhanvien) references NhanVien(id),
    foreign key(iddatphong) references DatPhong(id)
);

create table ChuyenPhong
(
    id INT IDENTITY(1,1) PRIMARY KEY,
    ngaychuyen datetime null,
    lydo nvarchar(255) null,
    idkhachhang int null,
    idnhanvien int null,
    idphongcu int null,
    idphongmoi int null,
    foreign key(idkhachhang) references KhachHang(id),
	foreign key(idnhanvien) references NhanVien(id),
    foreign key(idphongmoi) references Phong(id)
);
create table TraPhong
(	
    id INT IDENTITY(1,1) PRIMARY KEY,
    ngaytra datetime null,
    idnhanvien int null,
    iddatphong int null,
    foreign key(idnhanvien) references NhanVien(id),
    foreign key (iddatphong) references DatPhong(id)
);

create table NgayLe
(
    id INT IDENTITY(1,1) PRIMARY KEY,
	ngayles datetime null,
    tenngayle nvarchar(255) null,
    mota nvarchar(255) null
);
SELECT ngayles, tenngayle, mota
FROM NgayLe
WHERE ngayles = '08/03/2024'


CREATE TABLE ChinhSachGia (
    id INT IDENTITY(1,1) PRIMARY KEY,
    tenchinhsach NVARCHAR(255) NOT NULL,
    ngaybatdau DATE NOT NULL,
    ngayketthuc DATE NOT NULL,
	idngayle int,
    dieuchinhgiaphong DECIMAL(18, 2) NOT NULL,
    foreign key(idngayle) references NgayLe(id)
);

select * from phong where loaiphong = N'Phòng đơn'
select * from LichSuThanhToan
create table SanPham
(
    id INT IDENTITY(1,1) PRIMARY KEY,
	tensanpham nvarchar(255) null,
    mota text null,
    giaban decimal(10,2) null,
    soluongcon int null,
    trangthai nvarchar(255) null,
    image text null,
	idkhachsan int null,
    FOREIGN KEY (idkhachsan) REFERENCES KhachSan(id)
);
INSERT INTO SanPham (tensanpham, mota, giaban, soluongcon, trangthai, image, idkhachsan)
VALUES
(N'Gà quay', N'Description for Product A', 25000, 100, N'còn bán', 'https://i-giadinh.vnecdn.net/2022/02/11/Buoc-8-8-4440-1644565411.jpg', 1),
(N'Mì xào lòng bò', N'Description for Product B',30000, 100, N'còn bán', 'https://cdn.tgdd.vn/2021/03/CookRecipe/Avatar/mi-xao-trung-rau-cu-nam-thumbnail.jpg', 1),
(N'Coca', N'Description for Product C', 18000, 120, N'còn bán', 'https://songseafoodgrill.vn/wp-content/uploads/2022/03/Coca-2.png', 1);
select * from sanpham
create table ThueSanPham
(
    id INT IDENTITY(1,1) PRIMARY KEY,
	soluong int null,
    thanhtien decimal(10,2) null,
    idsanpham int null,
    idnhanvien int null,
    iddatphong int null,
    foreign key(idsanpham) references SanPham(id),
    foreign key(idnhanvien) references NhanVien(id),
    foreign key(iddatphong) references DatPhong(id)
);
SELECT COUNT(*) FROM SuDungMaGiamGia WHERE idMaGiamGia = @MaGiamGia AND idDatPhong IN (SELECT id FROM DatPhong WHERE idkhachhang = @IdKhachHang)

SELECT Phong.id, Phong.sophong, 
MAX(Phong.giatientheogio) AS giatientheogio, 
MAX(Phong.giatientheongay) AS giatientheongay, 
Phong.idkhachsan, Phong.loaiphong, Phong.songuoi
FROM DatPhong
LEFT JOIN Phong ON DatPhong.idphong = Phong.id
WHERE Phong.idkhachsan = 1 AND Phong.loaiphong = N'Phòng đơn' AND Phong.songuoi = 2
GROUP BY Phong.id, Phong.sophong, Phong.idkhachsan, Phong.loaiphong, Phong.songuoi
HAVING COUNT(DatPhong.idphong) > 1

select * from phong , datphong where phong.id = datphong.idphong
select * from datphong
select * from DatPhong left join KhachHang on datphong.idkhachhang = khachhang.id where DatPhong.idphong = @idphong and DatPhong.trangthai= N'đã đặt online'
select * from DatPhong left join KhachHang on datphong.idkhachhang = khachhang.id where DatPhong.idphong = 2 and DatPhong.trangthai= N'đã đặt online'
create table GiamGia
(
	id INT IDENTITY(1,1) PRIMARY KEY,
	solandatphong int null,
	phantramgiamgia decimal(10,2) null,
	ngaythemgiamgia datetime null,
	idkhachhang int null,
	idquydinh int null,
	iddatphong INT,
	FOREIGN KEY (iddatphong) REFERENCES DatPhong(id),
    foreign key(idkhachhang) references KhachHang(id),
	foreign key(idquydinh) references QuyDinhGiamGia(id)
);