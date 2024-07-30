using PagedList;
using QuanLyKhachSan_MVC.NET;

namespace Model.Models
{
    public class Modeldata
    {
        public UserProfileViewModel userProfileViewModel { get; set; }

        public Tang tang { get; set; }
        public List<Tang> listtang { get; set; }
        public IPagedList<Tang> PagedTTang { get; set; }
        public Likes likes { get; set; }
        public List<Likes> listLikes { get; set; }
        public IPagedList<Likes> PagedTLikes { get; set; }
        public Phong phong { get; set; }
        public List<Phong> listphong { get; set; }
        public List<Phong> listphongtrangthai { get; set; }
        public IPagedList<Phong> PagedTPhong { get; set; }
        public BinhLuan binhLuan { get; set; }
        public List<BinhLuan> listBinhLuan { get; set; }
        public IPagedList<BinhLuan> PagedBinhLuan { get; set; }
        public ChucVu chucVu { get; set; }
        public List<ChucVu> listchucVu { get; set; }
        public IPagedList<ChucVu> PagedTChucVu { get; set; }

        public NhanVien nhanVien { get; set; }
        public List<NhanVien> listnhanVien { get; set; }
        public IPagedList<NhanVien> PagedTNhanVien { get; set; }

        public DatPhong datPhong { get; set; }
        public List<DatPhong> listdatPhong { get; set; }
        public IPagedList<DatPhong> PagedTDatPhong { get; set; }
        public KhachHang khachhang { get; set; }
        public List<KhachHang> listkhachHangs { get; set; }
        public IPagedList<KhachHang> PagedTKhachHang { get; set; }
        public SanPham sanPham { get; set; }
        public List<SanPham> listsanPham { get; set; }
        public IPagedList<SanPham> PagedTSanPham { get; set; }
        public ThueSanPham thueSanPham { get; set; }
        public List<ThueSanPham> listthueSanPham { get; set; }
        public IPagedList<ThueSanPham> PagedTThueSanPham { get; set; }
        public float tongtienhueSanPham { get; set; }

        public LichSuThanhToan lichSuThanhToan { get; set; }
        public List<LichSuThanhToan> listlichSuThanhToan { get; set; }
        public IPagedList<LichSuThanhToan> PagedTLichSuThanhToan { get; set; }
        public ThietBi thietBi { get; set; }
        public List<ThietBi> listThietBi { get; set; }
        public IPagedList<ThietBi> PagedTThietBi { get; set; }
        public ThietBiPhong thietBiphong { get; set; }
        public List<ThietBiPhong> listThietBiphong { get; set; }
        public IPagedList<ThietBiPhong> PagedThietBiPhong { get; set; }
        public ThoiGian thoigian { get; set; }
        public List<ThoiGian> listThoiGian { get; set; }
        public IPagedList<ThoiGian> PagedTThoiGian { get; set; }

        public MaGiamGia magiamGia { get; set; }
        public List<MaGiamGia> listmaGiamGia { get; set; }
        public IPagedList<MaGiamGia> PagedTmaGiamGia { get; set; }
        public HopDongLaoDong hopDongLaoDong { get; set; }
        public List<HopDongLaoDong> listhopDongLaoDong { get; set; }
        public IPagedList<HopDongLaoDong> PagedThopDongLaoDong { get; set; }
        public KhachSan khachSan { get; set; }
        public List<KhachSan> listKhachSan { get; set; }
        public IPagedList<KhachSan> PagedTKhachSan { get; set; }
        public PhongKhachSan phongKhachSan { get; set; }
        public List<PhongKhachSan> listPhongKhachSan { get; set; }
        public IPagedList<PhongKhachSan> PagedPhongKhachSan { get; set; }

        public List<GiamGiaNgayLe> giamGiaNgayLelist { get; set; }
        public GiamGiaNgayLe giamGiaNgayle { get; set; }
        public ChuyenPhong chuyenPhong { get; set; }
        public List<ChuyenPhong> listChuyenPhong { get; set; }
        public IPagedList<ChuyenPhong> PagedTChuyenPhong { get; set; }
        public HuyDatPhong huyDatPhong { get; set; }
        public List<HuyDatPhong> listHuyDatPhong { get; set; }
        public IPagedList<HuyDatPhong> PagedTHuyDatPhong { get; set; }

        public CuocHoiThoai cuocHoiThoai { get; set; }
        public List<CuocHoiThoai> cuocHoiThoais { get; set; }
        public IPagedList<CuocHoiThoai> cuocHoiThoaispage { get; set; }
        public TinNhan tinNhan { get; set; }
        public List<TinNhan> TinNhans { get; set; }
        public IPagedList<TinNhan> TinNhanspage { get; set; }

        public LikesBinhLuan likesBinhLuan { get; set; }
        public List<LikesBinhLuan> likesBinhLuans { get; set; }
        public IPagedList<LikesBinhLuan> likesBinhLuanspage { get; set; }


        public LoaiDichVu loaiDichVu { get; set; }
        public List<LoaiDichVu> loaiDichVus { get; set; }
        public IPagedList<LoaiDichVu> loaiDichVuspage { get; set; }

        public int TotalRentedQuantity { get; set; }

        public float Tongdoanhthutungthang { get; set; }
        public int Tonglikebinhluanthichbuyid { get; set; }
        public int Tonglikebinhluankhongthichbuyid { get; set; }

        public int Tonglikebinhluankhongthichbuyidrely { get; set; }

        public string Ten { get; set; }
        public string Image { get; set; }

        public string NoiDungTinNhan { get; set; }
        public string loaitinnhan { get; set; }
        public string daxem { get; set; }


        public DateTime ThoiGianNhan { get; set; }



    }
}