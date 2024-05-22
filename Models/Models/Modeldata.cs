using PagedList;
namespace Model.Models
{
    public class Modeldata
    {
        public Tang tang { get; set; }
        public List<Tang> listtang { get; set; }
        public IPagedList<Tang> PagedTTang { get; set; }
        public Phong phong { get; set; }
        public List<Phong> listphong { get; set; }
        public List<Phong> listphongtrangthai { get; set; }
        public IPagedList<Phong> PagedTPhong { get; set; }
        public BoPhan boPhan { get; set; }
        public List<BoPhan> listbophan { get; set; }
        public IPagedList<BoPhan> PagedTBoPhan { get; set; }
        public ChucVu chucVu { get; set; }
        public List<ChucVu> listchucVu { get; set; }
        public IPagedList<ChucVu> PagedTChucVu { get; set; }
        public ViTriBoPhan viTriBoPhan { get; set; }
        public List<ViTriBoPhan> listviTriBoPhan { get; set; }
        public IPagedList<ViTriBoPhan> PagedTViTriBoPhan { get; set; }
        public NhanVien nhanVien { get; set; }
        public List<NhanVien> listnhanVien { get; set; }
        public IPagedList<NhanVien> PagedTNhanVien { get; set; }
        public LoaiDatPhong loaiDatPhong { get; set; }
        public List<LoaiDatPhong> listloaiDatPhong { get; set; }
        public IPagedList<LoaiDatPhong> PagedTLoaiDatPhong { get; set; }
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
        public TraPhong traphong { get; set; }
        public List<TraPhong> listtraphong { get; set; }
        public IPagedList<TraPhong> PagedTTraPhong { get; set; }
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
        public List<NgayLe> listngayle { get; set; }
        public NgayLe ngayLe { get; set; }
        public List<ChinhSachGia> chinhSachGialist { get; set; }
        public ChinhSachGia chinhSachGia { get; set; }
        public ChuyenPhong chuyenPhong { get; set; }
        public List<ChuyenPhong> listChuyenPhong { get; set; }
        public IPagedList<ChuyenPhong> PagedTChuyenPhong { get; set; }
        public HuyDatPhong huyDatPhong { get; set; }
        public List<HuyDatPhong> listHuyDatPhong { get; set; }
        public IPagedList<HuyDatPhong> PagedTHuyDatPhong { get; set; }

    }
}