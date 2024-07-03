create database QuanLyKhachSan;
use QuanLyKhachSan

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
VALUES (N'Hotel Sofwer', 5, N'Địa chỉ A', N'Thành phố A', N'Quốc gia A', 'vvc132003@gmail.com', '0123456789', N'Loại hình A', 'GP123456', '2023-01-15')

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
('10:00', 20, '18:00', 20, 'Time slot 1', 1)

create table Tang
(
    id INT IDENTITY(1,1) PRIMARY KEY,
    tentang nvarchar(255) null,
	idkhachsan INT NULL,
    FOREIGN KEY (idkhachsan) REFERENCES KhachSan(id)
);
INSERT INTO Tang (tentang, idkhachsan)
VALUES (N'Tầng 1', 1),
       (N'Tầng 2', 1),
       (N'Tầng 3', 1);
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
    trangthai nvarchar(255) null,
	idtaikhoangoogle nvarchar(255)
);

CREATE TABLE Likes
(
    id INT IDENTITY(1,1) PRIMARY KEY,
    idphong INT,    
    idkhachhang INT, 
    thoigianlike DATETIME DEFAULT GETDATE(),
    icons NVARCHAR(MAX),
    FOREIGN KEY (idphong) REFERENCES Phong(id),
    FOREIGN KEY (idkhachhang) REFERENCES KhachHang(id)
);
select * from DatPhong

CREATE TABLE BinhLuan (
    id INT IDENTITY(1,1) PRIMARY KEY,
    idkhachhang INT,
    noidung TEXT,
    thoigianbinhluan DATETIME DEFAULT GETDATE(),
	trangthai nvarchar(100),
	idphong INT,    
	FOREIGN KEY (idphong) REFERENCES Phong(id),
    FOREIGN KEY (idkhachhang) REFERENCES KhachHang(id)
);
select * from BinhLuan


INSERT INTO BinhLuan (idkhachhang, noidung, trangthai, idphong)
VALUES (1, N'Đánh giá tuyệt vời cho căn phòng này!', N'Đã duyệt', 3);
INSERT INTO BinhLuan (idkhachhang, noidung, trangthai, idphong)
VALUES (2, N'Đánh giá tuyệt vời cho căn phòng này!', N'Đã duyệt', 3);
INSERT INTO BinhLuan (idkhachhang, noidung, trangthai, idphong)
VALUES (3, N'Đánh giá tuyệt vời cho căn phòng này!', N'Đã duyệt', 3);

create table ChucVu
(
    id INT IDENTITY(1,1) PRIMARY KEY,
	tenchucvu nvarchar(255) null
);
INSERT INTO ChucVu (tenchucvu)
VALUES (N'Quản lý'),
       (N'Nhân viên lễ tân'),
       (N'Nhân viên buồng phòng');


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
    foreign key (idchucvu) references ChucVu(id),
	idkhachsan int null,
    FOREIGN KEY (idkhachsan) REFERENCES KhachSan(id) 
);
INSERT INTO NhanVien (hovaten, sodienthoai, tinh, huyen, phuong, taikhoan, matkhau, trangthai, solanvipham, image, cccd, gioitinh, ngaysinh, idchucvu, idkhachsan)
VALUES 
    (N'Võ Văn Chính', '0373449865', N'Quảng Trị', N'Triệu Phong', N'Triệu Đại', N'admin', 'AQAAAAIAAYagAAAAEAmxUAJDJ7mQ4uYsWq2JxL9y9d7lRRIpzCA1Rp9M3lDJh1oFRGvIXyx0wE6iy9WVeQ==', N'Hoạt động', 0, null, '123456789', 'Nam', '2003-03-01', 1, 1);

	select * from NhanVien
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

								
CREATE TABLE NguoiThamGia (
    id INT IDENTITY(1,1) PRIMARY KEY,
    cuochoithoaiid INT,
    nhanvienthamgiaid INT,
    duoctaovao DATETIME DEFAULT GETDATE(),
    duoccapnhatvao DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (cuochoithoaiid) REFERENCES CuocHoiThoai(id),
    FOREIGN KEY (nhanvienthamgiaid) REFERENCES NhanVien(id)
);

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


create table DatPhong
(
    id INT IDENTITY(1,1) PRIMARY KEY,
    ngaydat datetime null,
    ngaydukientra datetime null,
    tiendatcoc decimal(10,2) null,
    trangthai nvarchar(255) null,
	hinhthucthue nvarchar(255) null,
    loaidatphong nvarchar(255) null,
    idkhachhang int null,
    idphong int null,
	idthoigian int null,
    foreign key(idphong) references Phong(id),
    foreign key(idkhachhang) references KhachHang(id),
	FOREIGN KEY(idthoigian) references ThoiGian(id),
);

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


create table GiamGiaNgayLe
(
	id INT IDENTITY(1,1) PRIMARY KEY,
    tenchinhsach NVARCHAR(255) NOT NULL,
	ngayle datetime,
    ngaybatdau DATE NOT NULL,
    ngayketthuc DATE NOT NULL,
	tenngayle nvarchar(255),
    dieuchinhgiaphong DECIMAL(18, 2) NOT NULL,
	dieuchinhgiasanpham DECIMAL(18, 2)
);


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